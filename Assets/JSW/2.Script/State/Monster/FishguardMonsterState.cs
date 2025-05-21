using BaseMonsterState;
using UnityEngine;

namespace FishguardMonsterState
{
    // Idle
    public class FishguardMonsterIdle : BaseMonsterIdle, IState<BaseMonster>
    {
        public FishguardMonsterIdle(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }

    // Walk
    public class FishguardMonsterWalk : BaseMonsterWalk, IState<BaseMonster>
    {
        public FishguardMonsterWalk(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }

    // Run
    public class FishguardMonsterRun : BaseMonsterRun, IState<BaseMonster>
    {
        public FishguardMonsterRun(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }

    // CoolTime (기본 구조 추가)
    public class FishguardMonsterCoolTime : BaseMonsterCoolTime, IState<BaseMonster>
    {
        public FishguardMonsterCoolTime(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }

    // Attack
    public class FishguardMonsterAttack : BaseMonsterAttack, IState<BaseMonster>
    {
        public FishguardMonsterAttack(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }

    // Dead
    public class FishguardMonsterDead : BaseMonsterDead, IState<BaseMonster>
    {
        public FishguardMonsterDead(BaseMonster baseMonster) : base(baseMonster) {}
        public new void OperateEnter(BaseMonster sender) { base.OperateEnter(sender); }
        public new void OperateExit(BaseMonster sender) { base.OperateExit(sender); }
        public new void OperateUpdate(BaseMonster sender) { base.OperateUpdate(sender); }
    }
}