using UnityEngine;
using FishguardMonsterState;
public class FishguardMonster : BaseMonster
{

    protected override void IStateStartSetting()
    {
        base.IStateStartSetting();
        IState<BaseMonster> idle = new FishguardMonsterIdle(this);
        IState<BaseMonster> walk = new FishguardMonsterWalk(this);
        IState<BaseMonster> run = new FishguardMonsterRun(this);
        IState<BaseMonster> attack = new FishguardMonsterAttack(this);
        IState<BaseMonster> dead = new FishguardMonsterDead(this);
        IState<BaseMonster> coolTime = new FishguardMonsterDead(this);

        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Run, run);
        dicState.Add(MonsterState.Attack, attack);
         dicState.Add(MonsterState.Dead, dead);
         dicState.Add(MonsterState.CoolTime, coolTime);

        machine = new StateMachine<BaseMonster>(this, dicState[MonsterState.Idle]);
        machine.SetState(dicState[MonsterState.Idle]);
    }

}
