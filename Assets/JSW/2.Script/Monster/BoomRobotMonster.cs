using UnityEngine;
using BoomRobotMonsterState;
using System.Collections.Generic;
public class BoomRobotMonster : NormalMonster
{

    protected override void IStateStartSetting()
    {

        base.IStateStartSetting();
        IState<BaseMonster> idle = new BoomRobotMonsterIdle(this);
        IState<BaseMonster> walk = new BoomRobotMonsterWalk(this);
        IState<BaseMonster> trace = new BoomRobotMonsterTrace(this);
        IState<BaseMonster> attack = new BoomRobotMonsterAttack(this);
        IState<BaseMonster> dead = new BoomRobotMonsterDead(this);


        dicState.Add(MonsterState.Idle, idle);

        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Attack, attack);
        dicState.Add(MonsterState.Trace, trace);
        dicState.Add(MonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[MonsterState.Idle]);
    }


}




