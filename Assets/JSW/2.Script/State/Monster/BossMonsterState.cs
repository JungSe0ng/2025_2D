using UnityEngine;
using System.Collections;
using Unity.Mathematics;

namespace BossMonsterState
{
    // Idle
    public class BossMonsterIdle : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public BossMonsterIdle(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() { }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // BrustShoot
    public class BossMonsterBrustShoot : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;

        protected IBullet[] laser = null; 
        protected int cnt = 0;
        public BossMonsterBrustShoot(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() {
            //연속으로 2번 방문인지 확인 후 연속으로 방문했으면 다음 state를 jumpfly로 변경해준다.
            //연속 방문이 아닌경우 cooltime으로 보내버린다.

            //플레이어 방향을 기준으로 5개의 레이저를 발사한다. 맨처음 생성자 부분에서 레이저를 생산한다.

            cnt++;
            if(cnt>=2){
                baseMonster.nextState = EMonsterState.JumpFly;
                baseMonster.StatePatttern(EMonsterState.CoolTime);
            }else{
                baseMonster.StatePatttern(EMonsterState.CoolTime);
            }
         }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // JumpFly
    public class BossMonsterJumpFly : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public BossMonsterJumpFly(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() { }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // Dead
    public class BossMonsterDead : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public BossMonsterDead(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() { }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // Walk
    public class BossMonsterWalk : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        protected Vector3 targetPos;
        public BossMonsterWalk(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() {

            //플레이어한테 이동할건데 시간이 지나면 가속도 붙어서 빨리 이동이됨 그리고 애니메이션 속도도 증가
            targetPos = baseMonster.IsAttackMonster[0].transform.position;
         }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { 

            //플레이어한테 이동할건데 시간이 지나면 가속도 붙어서 빨리 이동이됨 그리고 애니메이션 속도도 증가
            if(baseMonster.IsAttackMonster.Count<=0){
                Debug.LogError("보스가 플레이어를 찾을 수 없습니다.");
                return;
            }
            //거리조절절
            if(math.abs(baseMonster.transform.position.x - targetPos.x)<baseMonster.MonsterDB.StopDistance){
                baseMonster.nextState = EMonsterState.BrustShoot;
                baseMonster.StatePatttern(EMonsterState.CoolTime);
            }
            baseMonster.APath.FindPathTarget(ref targetPos);
        }
    }

    public class BossMonsterCoolTime : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        protected float coolTime = 0;
        public BossMonsterCoolTime(BaseMonster baseMonster) { this.baseMonster = baseMonster; }
        public virtual void OperateEnter() { 
            //다음 쿨타임까지 기다림 중간에 죽지 않았으면 계속 쿨타임모드
        }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() {
            if(coolTime>=1.0f){
                coolTime = 0;
                baseMonster.StatePatttern(baseMonster.nextState);
            }
            coolTime += Time.deltaTime;
         }
    }
} 