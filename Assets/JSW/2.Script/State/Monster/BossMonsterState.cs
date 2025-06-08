using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine.Video;
using Unity.VisualScripting;
namespace BossMonsterState
{
    // Idle
    public class BossMonsterIdle : IState<BaseMonster>
    {
        protected BaseMonster bossMonster;
        public BossMonsterIdle(BaseMonster bossMonster) { this.bossMonster = bossMonster; }
        public virtual void OperateEnter()
        {

        }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }
    // BrustShoot
    public class BossMonsterBrustShoot : IState<BaseMonster>
    {
        protected BossMonster bossMonster;

        protected Queue<IBullet> laserArr = null;

        protected int instanceMax = 5;
        protected int cnt = 0;
        protected IBullet laser = null;
        // 5방향 기본 벡터 캐싱
        private static readonly Vector3[] baseDirections = new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, -1, 0),
            new Vector3(0, -1, 0)
        };
        public BossMonsterBrustShoot(BaseMonster bossMonster)
        {
            this.bossMonster = bossMonster.GetComponent<BossMonster>();
            laserArr = new Queue<IBullet>();
            InstanceLaser();
        }
        public virtual void OperateEnter()
        {
            //연속으로 2번 방문인지 확인 후 연속으로 방문했으면 다음 state를 jumpfly로 변경해준다.
            //연속 방문이 아닌경우 cooltime으로 보내버린다.
            Debug.Log("보스가 레이저 모드를 시작합니다.");
            //플레이어 방향을 기준으로 5개의 레이저를 발사한다. 맨처음 생성자 부분에서 레이저를 생산한다.
            ShootLaser();

            //발사 이후 다음 단계를 위한 코루틴 단계를 실행한다.
            bossMonster.StartCoroutine(FinishShootLaser());
        }
          public  void OOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("애니메이션 상태 종료됨: " + stateInfo.shortNameHash);
    }

        private IEnumerator FinishShootLaser()
        {
            AnimatorStateInfo info = bossMonster.MonsterAnimator.GetCurrentAnimatorStateInfo(0);
            while (bossMonster.MonsterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || !bossMonster.IsDead())
            {
                Debug.Log(bossMonster.MonsterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                if (info.IsName("Attack") && info.normalizedTime >= 1f)
                {
                    Debug.LogError("애니메이션 끝");
                }
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log("레이저 난사를 종료합니다.");
            if (!bossMonster.IsDead())
                cnt++;
            if (cnt >= 2)
            {
                bossMonster.nextState = EMonsterState.JumpFly;
                bossMonster.CoolTime.MaxCoolTime = 1.0f;
                bossMonster.StatePatttern(EMonsterState.CoolTime);
                cnt = 0;
            }
            else
            {
                //레이저 난사를 한다. 난사를 한 후 쿨타임으로 보내버리고 다시 돌아오면 다시 난사 후 쿨타임으로 보내버린다. 그다음 행선지는 junp로 보내버린다.
                //공통이 난사이니 난사를 진행하고 다음 진행을 안내하면 되겠다./
                bossMonster.StatePatttern(EMonsterState.CoolTime);
            }
        }


        //플레이어 방향을 계싼해서 해당 방향 5군데를 미리 선정하고 그 방향대로 보내버리면 되곘다.
        //플레이어 방향 계산-> 플레이어 위치 받아서  내위치랑 비교해서 내가 오른쪽이면 좌측 방향 5군데를 계산한다. 내가 좌측이면 우측 방향을 기준으로 5군데를 계산한다.
        //계산 기준은 본인 (0,1) (1,1) (1,0) , (1,-1), (0,-1) 이렇게 5군데를 계산한다.
        private void ShootLaser()
        {
            bossMonster.MonsterAnimator.SetFloat("IsAttack", 1);
            if (bossMonster.IsAttackMonster.Count == 0) return;
            Vector3 playerPos = bossMonster.IsAttackMonster[0].transform.position;
            Vector3 bossPos = bossMonster.transform.position;
            int xDir = bossPos.x < playerPos.x ? 1 : -1;
            for (int i = 0; i < baseDirections.Length; i++)
            {
                Vector3 dir = baseDirections[i];
                if (dir.x != 0) dir.x *= xDir;
                IBullet laser = OutLaser(i);
                if (laser != null)
                {
                    laser.Shoot(dir.normalized);
                }
            }
        }
        public void InstanceLaser() //레이저 생성
        {
            for (int i = 0; i < instanceMax; i++)
            {
                laser = Object.Instantiate(bossMonster.Laser, bossMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
                InLaser(laser);
            }
        }
        //레이저를 꺼낼 수는 방식을 
        private IBullet OutLaser(int num)
        {
            if (laserArr.Count <= 0)
            {
                return Object.Instantiate(bossMonster.Laser, bossMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
            }
            IBullet laser = laserArr.Dequeue();
            laser.OutBullet(bossMonster.BulletParent, Vector3.zero, Quaternion.identity);
            return laser;
        }

        private void InLaser(IBullet laser)
        {
            laser.InBullet(bossMonster.BulletParent);
            laserArr.Enqueue(laser);
        }

        public virtual void OperateExit()
        {
            //애니메이션 종료
            bossMonster.MonsterAnimator.SetFloat("IsAttack", 0);
            Debug.Log("보스가 레이저 모드를 종료합니다.");
        }
        public virtual void OperateUpdate() { }
    }

    // JumpFly
    public class BossMonsterJumpFly : IState<BaseMonster>
    {
        protected BaseMonster bossMonster;
        private Vector3 mainPos = Vector3.zero;
        public BossMonsterJumpFly(BaseMonster bossMonster) { this.bossMonster = bossMonster; }


        public virtual void OperateEnter()
        {
            //점프 애니메이션을 실행하고 점프애니메이션이 끝나면 바로 FlyAnimation을 실행한다. 목표 지점 가운데에 도달하면 FlyAnimation을 종료한다.
            bossMonster.StartCoroutine(FinishJump());
            Debug.Log("보스가 점프 모드를 시작합니다.");
        }

        private IEnumerator FinishJump() //JumpAnimation이 끝나면 바로 FlyAnimation을 실행한다.
        {
            bossMonster.MonsterAnimator.SetFloat("IsJump", 1);
            while (bossMonster.MonsterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || !bossMonster.IsDead())
            {
                yield return new WaitForSeconds(0.1f);
            }
            bossMonster.MonsterAnimator.SetFloat("IsJump", 2);
            bossMonster.StartCoroutine(FinishFly());
        }

        private IEnumerator FinishFly() //FlyAnimation이 끝나면 바로 목표 지점 가운데에 도달하면 다음 단계로 넘어간다.
        {
            //목표지점
            while (Vector3.Distance(bossMonster.transform.position, mainPos) > 0.1f || !bossMonster.IsDead())
            {
                //목표물 지점으로 날아감.
                bossMonster.transform.position = Vector3.MoveTowards(
                    bossMonster.transform.position,
                    mainPos,
                    1.0f * Time.deltaTime
                );
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("목표 지점에 도착했습니다. 착지합니다.");
            bossMonster.MonsterAnimator.SetFloat("IsJump", 3);


            while (bossMonster.MonsterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || !bossMonster.IsDead())
            {
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log("착지를 종료합니다. 다음 쿨타임으로 넘어갑니다.");

            //목표물에 도착을 했으면 다음 단계로 쿨타임으로 갔다가 미사일 모드로 변경한다.
            bossMonster.nextState = EMonsterState.Missile;
            bossMonster.StatePatttern(EMonsterState.CoolTime);
        }
        public virtual void OperateExit()
        {
            Debug.Log("보스가 점프 모드를 종료합니다.");
        }
        public virtual void OperateUpdate() { }
    }

    // Dead
    public class BossMonsterDead : IState<BaseMonster>
    {
        protected BaseMonster bossMonster;
        public BossMonsterDead(BaseMonster bossMonster) { this.bossMonster = bossMonster; }
        public virtual void OperateEnter() { }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // Walk
    public class BossMonsterWalk : IState<BaseMonster>
    {
        protected BaseMonster bossMonster;
        protected Vector3 targetPos;
        public BossMonsterWalk(BaseMonster bossMonster) { this.bossMonster = bossMonster; }
        public virtual void OperateEnter()
        {
            Debug.Log("보스가 플레이한테 이동하는 추적모드를 시작합니다.");
            bossMonster.MonsterAnimator.SetBool("IsWalk", true);
            //플레이어한테 이동할건데 시간이 지나면 가속도 붙어서 빨리 이동이됨 그리고 애니메이션 속도도 증가
            targetPos = bossMonster.IsAttackMonster[0].transform.position;
        }
        public virtual void OperateExit()
        {
            bossMonster.MonsterAnimator.SetBool("IsWalk", false);
            Debug.Log("보스가 플레이한테 이동하는 추적모드를 종료합니다.");
        }
        public virtual void OperateUpdate()
        {

            //플레이어한테 이동할건데 시간이 지나면 가속도 붙어서 빨리 이동이됨 그리고 애니메이션 속도도 증가
            if (bossMonster.IsAttackMonster.Count <= 0)
            {
                Debug.LogError("보스가 플레이어를 찾을 수 없습니다.");
                return;
            }
            //플레이어한테 움직인다.
            bossMonster.transform.position = Vector3.MoveTowards(bossMonster.transform.position, targetPos, 1.0f * Time.deltaTime);
            //거리조절절
            if (Vector3.Distance(bossMonster.transform.position, targetPos) < bossMonster.MonsterDB.StopDistance)
            {
                bossMonster.nextState = EMonsterState.BrustShoot;
                bossMonster.StatePatttern(EMonsterState.CoolTime);
            }
            bossMonster.APath.FindPathTarget(ref targetPos);
        }
    }

    public class BossMonsterCoolTime : IState<BaseMonster>
    {
        protected BaseMonster bossMonster;
        protected float coolTime = 0;
        private float maxCoolTime = 1.0f;
        public float MaxCoolTime { set { maxCoolTime = value; } }
        public BossMonsterCoolTime(BaseMonster bossMonster) { this.bossMonster = bossMonster; }
        public virtual void OperateEnter()
        {
            //다음 쿨타임까지 기다림 중간에 죽지 않았으면 계속 쿨타임모드
            Debug.Log("보스가 쿨타임 모드를 시작합니다.");
        }
        public virtual void OperateExit()
        {
            Debug.Log("보스가 쿨타임 모드를 종료합니다.");
        }
        public virtual void OperateUpdate()
        {
            if (coolTime >= maxCoolTime)
            {
                coolTime = 0;
                bossMonster.StatePatttern(bossMonster.nextState);
            }
            coolTime += Time.deltaTime;
        }
    }

    public class BossMonsterMissile : IState<BaseMonster>
    {
        protected BossMonster bossMonster = null;
        protected Queue<IBullet> missileArr = null;

        protected int instanceMax = 5;
        protected int cnt = 0;

        private IBullet missile = null;

        public BossMonsterMissile(BaseMonster bossMonster)
        {
            this.bossMonster = bossMonster.GetComponent<BossMonster>();
            missileArr = new Queue<IBullet>();
            InstanceMissile();
        }
        public virtual void OperateEnter()
        {
            //시작을 하면 연속으로 5번을 발사하게 한다. 쿨타임 0.1초를 기준으로 발사한다. 총알은 유도탄이 되어 있다.
            Debug.Log("보스가 미사일 모드를 시작합니다.");
            ShootMissile();

        }
        private IEnumerator CorutineMissile()
        {
            //애니메이션을 실행한다.
            bossMonster.MonsterAnimator.SetFloat("IsAttack", 2);

            //0.1초를 간격으로 미사일을 발사한다.
            for (int i = 0; i < 5; i++)
            {
                ShootMissile();
                yield return new WaitForSeconds(0.1f);
            }
            //다음 루트로 이동을 지시한다.
            bossMonster.nextState = EMonsterState.Walk;
            bossMonster.StatePatttern(EMonsterState.CoolTime);
        }

        //플레이어 방향을 계싼해서 해당 방향 5군데를 미리 선정하고 그 방향대로 보내버리면 되곘다.
        //플레이어 방향 계산-> 플레이어 위치 받아서  내위치랑 비교해서 내가 오른쪽이면 좌측 방향 5군데를 계산한다. 내가 좌측이면 우측 방향을 기준으로 5군데를 계산한다.
        //계산 기준은 본인 (0,1) (1,1) (1,0) , (1,-1), (0,-1) 이렇게 5군데를 계산한다.
        private void ShootMissile() //미사일 하나 꺼내서 미사일을 다음 좌표로 쏘라고 명령한다.
        {
            OutMissile(0).Shoot(bossMonster.IsAttackMonster[0].transform.position);
        }
        private void InstanceMissile() //레이저 생성
        {
            for (int i = 0; i < instanceMax; i++)
            {
                missile = Object.Instantiate(bossMonster.Laser, bossMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
                InMissile(missile);
            }
        }
        //레이저를 꺼낼 수는 방식을 
        private IBullet OutMissile(int num)
        {
            if (missileArr.Count <= 0)
            {
                return Object.Instantiate(bossMonster.Laser, bossMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
            }
            missile = missileArr.Dequeue();
            missile.OutBullet(bossMonster.BulletParent, Vector3.zero, Quaternion.identity);
            return missile;
        }

        private void InMissile(IBullet laser)
        {
            missile.InBullet(bossMonster.BulletParent);
            missileArr.Enqueue(missile);
        }

        public virtual void OperateExit()
        {
            //애니메이션 종료
            bossMonster.MonsterAnimator.SetFloat("IsAttack", 0);
            Debug.Log("보스가 미사일 모드를 종료합니다.");
        }
        public virtual void OperateUpdate() { }
    }

}