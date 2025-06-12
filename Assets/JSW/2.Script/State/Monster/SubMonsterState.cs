using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SubMonsterState
{
    public class SubMonsterStateIdle : NormalMonsterState.NormalMonsterIdle
    {
        public SubMonsterStateIdle(BaseMonster baseMonster) : base(baseMonster) { }
    }

    public class SubMonsterStateWalk : NormalMonsterState.NormalMonsterWalk
    {
        private float time = 0.0f;
        public SubMonsterStateWalk(BaseMonster baseMonster) : base(baseMonster) { }

        public override void OperateEnter()
        {
            //애니메이션 변경이 따로 없고 쿨타임도 따로 필요없고
        }
        public override void OperateUpdate()
        {
            // 필요시 여기에 targetPos 사용
            if (baseMonster.IsAttackMonster.Count < 0) baseMonster.StatePatttern(EMonsterState.Idle);
            if (baseMonster.IsAttackMonster.Count > 0)
            {
                targetPos = baseMonster.IsAttackMonster[0].transform.position;
                baseMonster.APath.FindPathTarget(ref targetPos, 0);
            }
        }
    }

    public class SubMonsterStateCoolTime : NormalMonsterState.NormalMonsterCoolTime
    {
        private float cooltime = 0;
        protected Vector3 targetPos;
        protected SubMonster subMonster;
        public SubMonsterStateCoolTime(BaseMonster baseMonster) : base(baseMonster)
        {
            subMonster = baseMonster.GetComponent<SubMonster>();
        }
        public override void OperateEnter()
        {
            base.OperateEnter();
            Debug.Log("쿨타임 모드에 진입했습니다.");
            //1초후 그냥 보내면 된다.
            baseMonster.StartCoroutine(NextIdleTime());
        }
        public override void OperateExit()
        {
            base.OperateExit();
        }
        public override void OperateUpdate()
        {
            base.OperateUpdate();
            if (baseMonster.IsAttackMonster.Count > 0)
            {
                targetPos = baseMonster.IsAttackMonster[0].transform.position;
                baseMonster.APath.FindPathTarget(ref targetPos);
            }
     
        }
        private IEnumerator NextIdleTime()
        {
            yield return new WaitForSeconds(1.0f);
            subMonster.isChangeState = false;
            baseMonster.StatePatttern(EMonsterState.Idle);
        }
    }

    public class SubMonsterStateAttack : NormalMonsterState.NormalMonsterAttack
    {
        protected Vector3 dir;
        protected float coolTime = 0.0f;
        protected Queue<IBullet> missileArr = new Queue<IBullet>();
        protected int instanceMax = 5;
        protected int cnt = 0;
        protected IBullet missile = null;
        protected bool isStop = true;
        protected SubMonster subMonster;
        public SubMonsterStateAttack(BaseMonster baseMonster) : base(baseMonster)
        {
            subMonster = baseMonster as SubMonster;
            InstanceMissile();
        }
        public override void OperateEnter()
        {
            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));
            Debug.Log("공격모드 전환");
            baseMonster.StartCoroutine(NextCoolTime());
            //발사가 끝나면 쿨타임으로 보내버린다.

        }
        public override void OperateExit()
        {
            subMonster.isChangeState = true;
            baseMonster.MonsterAnimator.SetBool("IsAttack", false);
            Debug.Log("공격모드 종료");

        }
        public override void OperateUpdate()
        {

        }

        private IEnumerator NextCoolTime()
        {
            baseMonster.MonsterAnimator.SetBool("IsAttack", true);
            Debug.Log("발사");
            //d애니메이션이 종료되면 다음단계로 넘어간다.
            yield return new WaitForSeconds(0.83f);
            ShootMissile();
            subMonster.isChangeState = true;
            baseMonster.StatePatttern(EMonsterState.CoolTime);
        }

        private void ShootMissile() //미사일 하나 꺼내서 미사일을 다음 좌표로 쏘라고 명령한다.
        {
            if (baseMonster.IsAttackMonster.Count > 0)
            {
                Vector3 targetPos = baseMonster.IsAttackMonster[0].transform.position;
                Vector3 direction = targetPos - baseMonster.transform.position;
                OutMissile().Shoot(direction);
                Debug.Log("미사일 발사: " + direction);
            }
        }
        private void InstanceMissile() //레이저 생성
        {
            for (int i = 0; i < instanceMax; i++)
            {
                missile = Object.Instantiate(subMonster.Missile, baseMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
                InMissile(missile);
            }
        }
        //레이저를 꺼낼 수는 방식을 
        private IBullet OutMissile()
        {
            if (missileArr.Count <= 0)
            {
                return Object.Instantiate(subMonster.Missile, baseMonster.transform.position, Quaternion.identity).GetComponent<IBullet>();
            }
            missile = missileArr.Dequeue();
            missile.OutBullet(subMonster.BulletParent, baseMonster.transform.position, Quaternion.identity);
            return missile;
        }

        private void InMissile(IBullet laser)
        {
            missile.InBullet(subMonster.BulletParent);
            missileArr.Enqueue(missile);
        }
    }
    public class SubMonsterStateTrace : NormalMonsterState.NormalMonsterTrace
    {
        public SubMonsterStateTrace(BaseMonster baseMonster) : base(baseMonster) { }
    }

    public class SubMonsterStateDead : NormalMonsterState.NormalMonsterDead
    {
        public SubMonsterStateDead(BaseMonster baseMonster) : base(baseMonster) { }
        public override void OperateEnter()
        {
            baseMonster.StartCoroutine(CoutineDead());
        }
        protected IEnumerator CoutineDead()
        {
            Debug.Log("쥭어용");
            baseMonster.MonsterAnimator.SetBool("IsDead", true);
            //d애니메이션이 종료되면 다음단계로 넘어간다.
            yield return new WaitForSeconds(2.0f);
            Debug.Log("멈춤");
            baseMonster.MonsterAnimator.speed = 0;
            yield return new WaitForSeconds(0.2f);
            baseMonster.gameObject.SetActive(false);
        }
    }
}