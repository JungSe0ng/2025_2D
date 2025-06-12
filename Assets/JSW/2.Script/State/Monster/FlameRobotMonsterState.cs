using UnityEngine;
using System.Collections;

namespace FlameRobotMonsterState
{
    public class FlameRobotMonsterStateIdle : NormalMonsterState.NormalMonsterIdle
    {
        public FlameRobotMonsterStateIdle(BaseMonster baseMonster) : base(baseMonster) { }

       
    }

    public class FlameRobotMonsterStateWalk : NormalMonsterState.NormalMonsterWalk
    {
        public FlameRobotMonsterStateWalk(BaseMonster baseMonster) : base(baseMonster) { }

        public override void OperateEnter()
        {
            base.OperateEnter();

        }
        public override void OperateUpdate()
        {
            // 필요시 여기에 targetPos 사용
        }
    }

    public class FlameRobotMonsterStateCoolTime : NormalMonsterState.NormalMonsterCoolTime
    {
        private FlameMonster flame;
        private bool isStop = true;

        private float cooltime = 0;
        protected Vector3 targetPos;
        public FlameRobotMonsterStateCoolTime(BaseMonster baseMonster) : base(baseMonster)
        {
            flame = baseMonster.GetComponent<FlameMonster>();
        }


        public override void OperateEnter()//쿨타임 모드에 진입함 -> 다음 공격모드까지 계산을 진행한다. 몬스터가 범위 내에 있으면 이동은 함 범위 밖 이동 x
        {
            base.OperateEnter();
            Debug.Log("큘타임 모드에 진입했습니다.");
            baseMonster.StartCoroutine(NextIdleTime());
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);

            isStop = true;
            // baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));
        }

        public override void OperateExit()
        {
            base.OperateExit();
            isStop = false;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), false);
        }

        public override void OperateUpdate()//다음 공격모드까지 계산을 진행한다. 몬스터가 범위 내에 있으면 이동은 함 범위 밖 이동 x
        {
            base.OperateUpdate();
            if (baseMonster.IsAttackMonster.Count > 0)
            {
                targetPos = baseMonster.IsAttackMonster[0].transform.position;
                baseMonster.APath.FindPathTarget(ref targetPos, baseMonster.transform.position.x - targetPos.x < 0 ? -1 : 1);
            }
           

        }
        private IEnumerator NextIdleTime()
        {
            Debug.Log(111111);
            yield return new WaitForSeconds(1.0f);
            //다음 쿨타임 state로 전환한다. 쿨타임에서 변경이 안되도록 설정해준다.
            flame.isChangeState = false;
            baseMonster.StatePatttern(EMonsterState.Idle);
        }
    }

    public class FlameRobotMonsterStateAttack : NormalMonsterState.NormalMonsterAttack
    {
        protected Vector3 dir;
        protected float coolTime = 0.0f;
        protected FlameMonster flame;
        protected Vector3 targetPos;

        public FlameRobotMonsterStateAttack(BaseMonster baseMonster) : base(baseMonster)
        {
            flame = baseMonster.GetComponent<FlameMonster>();
        }

        public override void OperateEnter()
        {
            base.OperateEnter();
            //다음 쿨타임까지를 계산하고 쿨타임 시간이 되면 쿨타임 상태로 변경한다. 몬스터의 멈춤 state를 true로 변경해준다.
            Debug.Log("공격모드 전환");
            baseMonster.StartCoroutine(NextCoolTime());
        }


        public override void OperateExit()
        {
            base.OperateExit();
            isStop = false;
        }

        public override void OperateUpdate()
        {

            //몬스터 거리가 멀어지면 idle상태로 전환한다.
            if (baseMonster.IsAttackMonster.Count < 0) baseMonster.StatePatttern(EMonsterState.Idle);

            //몬스터를 찾아서 이동을 한다.
            if (baseMonster.IsAttackMonster.Count > 0)
            {
                targetPos = baseMonster.IsAttackMonster[0].transform.position;
                baseMonster.APath.FindPathTarget(ref targetPos, baseMonster.transform.position.x - targetPos.x < 0 ? -1 : 1);
            }
        }

        //다음 쿨타임을 계산한다.
        private IEnumerator NextCoolTime()
        {
            while (isStop)
            {
                //정해진 쿨타임 시간이 지나면 break;
                if (coolTime >= 10.0f) break;
                coolTime += 1;
                Debug.Log("공격모드 진행중" + coolTime);
                yield return new WaitForSeconds(1.0f);
            }
            coolTime = 0.0f;
            //다음 쿨타임 state로 전환한다. 쿨타임에서 변경이 안되도록 설정해준다.
            flame.isChangeState = true;
            baseMonster.StatePatttern(EMonsterState.CoolTime);
        }
    }

    public class FlameRobotMonsterStateTrace : NormalMonsterState.NormalMonsterTrace
    {
        public FlameRobotMonsterStateTrace(BaseMonster baseMonster) : base(baseMonster) { }
    }

    public class FlameRobotMonsterStateDead : NormalMonsterState.NormalMonsterDead
    {
        public FlameRobotMonsterStateDead(BaseMonster baseMonster) : base(baseMonster) { }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }
    }
}
