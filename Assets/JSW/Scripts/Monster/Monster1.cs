using BasicMonsterState;
using UnityEngine;

public class Monster1 : MonsterBase
{
    protected override void IStateStartSetting()
    {
        base.IStateStartSetting();
        //상태를 생성 후 Dictionary로 관리
        IState<MonsterBase> idle = new BasicMonsterIdle(this);
        IState<MonsterBase> walk = new BasicMonsterWalk(this);
        IState<MonsterBase> attack = new BasicMonsterAttack(this);
        IState<MonsterBase> dead = new BasicMonsterDead(this);

        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Attack, attack);
        dicState.Add(MonsterState.Dead, dead);

        machine = new StateMachine<MonsterBase>(this, dicState[MonsterState.Idle]);
        machine.SetState(dicState[MonsterState.Walk]);
    }
}
