using UnityEngine;

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
            //근데 만약 해당 부분이 이동할 수없는 부분이라면?
            baseMonster.APath.FindPathTarget(baseMonster.IsAttackMonster[0].transform.position,baseMonster.transform.position.x - baseMonster.IsAttackMonster[0].transform.position.x < 0 ? -1 : 1);
            //Debug.Log(baseMonster.transform.position.x - baseMonster.IsAttackMonster[0].transform.position.x < 0 ? -1 : 1);
        }
    }

    public class FlameRobotMonsterStateCoolTime : NormalMonsterState.NormalMonsterCoolTime
    {
        public FlameRobotMonsterStateCoolTime(BaseMonster baseMonster) : base(baseMonster) { }
    }

    public class FlameRobotMonsterStateAttack : NormalMonsterState.NormalMonsterAttack
    {
        protected Vector3 dir;
        public FlameRobotMonsterStateAttack(BaseMonster baseMonster) : base(baseMonster) { }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }

        public override void OperateUpdate()
        {
            //내 현재 위치와 몬스터 위치를 비교해서 내 위치가 몬스터보다 오른쪽에 있으면 몬스터의 x-1위치 ,y 로 목표물 설정을 한다.

            //오른쪽 위치 값으로 이동하도록 변경한다. 근데 여긴 패턴이라서..

           baseMonster.APath.FindPathTarget(baseMonster.IsAttackMonster[0].transform.position,baseMonster.transform.position.x - baseMonster.IsAttackMonster[0].transform.position.x < 0 ? -1 : 1);

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
