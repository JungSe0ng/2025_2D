using BasicMonsterState;
using UnityEngine;
using MonsterEnum;
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

        dicState.Add(EMonsterState.Idle, idle);
        dicState.Add(EMonsterState.Walk, walk);
        dicState.Add(EMonsterState.Attack, attack);
        dicState.Add(EMonsterState.Dead, dead);

        machine = new StateMachine<MonsterBase>(this, dicState[EMonsterState.Idle]);
        machine.SetState(dicState[EMonsterState.Walk]);
    }
}
