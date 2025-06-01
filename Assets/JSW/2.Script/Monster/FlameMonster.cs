using UnityEngine;
using FlameRobotMonsterState;
using NormalMonsterState;
using System.Collections.Generic;
using System.Collections;
public class FlameMonster : NormalMonster
{
    protected override void IStateStartSetting()
    {

        base.IStateStartSetting();
        IState<BaseMonster> idle = new NormalMonsterIdle(this);
        IState<BaseMonster> walk = new FlameRobotMonsterStateWalk(this);
        IState<BaseMonster> trace = new FlameRobotMonsterStateTrace(this);
        IState<BaseMonster> attack = new FlameRobotMonsterStateAttack(this);
        IState<BaseMonster> dead = new FlameRobotMonsterStateDead(this);

        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Attack, attack);
        dicState.Add(MonsterState.Trace, trace);
        dicState.Add(MonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[MonsterState.Idle]);

    }
}
