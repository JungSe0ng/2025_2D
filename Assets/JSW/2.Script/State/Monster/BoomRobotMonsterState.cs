using UnityEngine;
using System.Collections;
using NormalMonsterState;

namespace BoomRobotMonsterState
{
    // Idle
    public class BoomRobotMonsterIdle : NormalMonsterIdle
    {
        public BoomRobotMonsterIdle(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
        }

        public override void OperateExit() {
            base.OperateExit();
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }
    // Attack
    public class BoomRobotMonsterAttack : NormalMonsterAttack
    {
        private float boomTime =5.0f;
        public bool isBoom = false;
        public BoomRobotMonsterAttack(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));

            //override를 진행해서 아래에서 재정의해서 사용이 필요함 공격 애니메이션 타이밍이 다름
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);
            //countdown시작해서 boom
            isBoom = false;
            baseMonster.StartCoroutine(CorutineBoom());
        }

        public override void OperateExit() {
            base.OperateExit();
            isBoom = false;
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }

        private IEnumerator CorutineBoom(){
            float time =0;
            while(time <boomTime||!isBoom){
                time += 1f;
                if(time >= 2.0f)
                yield return new WaitForSeconds(1f);    
                isBoom = true;
            }
            //터짐
            Boom();
        }
        private void Boom(){
            baseMonster.StatePatttern(EMonsterState.Dead);
            Debug.Log("자폭 모드 작동");
        }
    }


    public class BoomRobotMonsterTrace : NormalMonsterState.NormalMonsterTrace
    {
        public BoomRobotMonsterTrace(BaseMonster baseMonster) : base(baseMonster) { }

    }
    // Dead
    public class BoomRobotMonsterDead : NormalMonsterDead
    {
        public BoomRobotMonsterDead(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
            Debug.Log("자폭했습니다.");
            baseMonster.StatePatttern(EMonsterState.Idle);
        }

        public override void OperateExit() {
            base.OperateExit();
            baseMonster.gameObject.SetActive(false);
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }
}
