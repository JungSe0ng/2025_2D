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
    }

    public class FlameRobotMonsterStateCoolTime : NormalMonsterState.NormalMonsterCoolTime
    {
        public FlameRobotMonsterStateCoolTime(BaseMonster baseMonster) : base(baseMonster) { }
    }

    public class FlameRobotMonsterStateAttack : NormalMonsterState.NormalMonsterAttack
    {
        public FlameRobotMonsterStateAttack(BaseMonster baseMonster) : base(baseMonster) { }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateExit()
        {
            base.OperateExit();
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
