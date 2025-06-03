using UnityEngine;
using BoomRobotMonsterState;
using System.Collections;
public class BoomRobotMonster : NormalMonster
{

    protected override void IStateStartSetting()
    {

        base.IStateStartSetting();
        IState<BaseMonster> idle = new BoomRobotMonsterIdle(this);
        IState<BaseMonster> trace = new BoomRobotMonsterTrace(this);
        IState<BaseMonster> attack = new BoomRobotMonsterAttack(this);
        IState<BaseMonster> dead = new BoomRobotMonsterDead(this);


        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Attack, attack);
        dicState.Add(MonsterState.Trace, trace);
        dicState.Add(MonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[MonsterState.Idle]);
    }

    protected override IEnumerator CorutinePattern()
    {
        while (dicState[MonsterState.Dead] != machine.CurState)
        {

            //공격 범위내에 들어온 몬스터를 찾았다?
            if (isAttackMonster.Count > 0)
            {
                StatePatttern(MonsterState.Attack);
            }
            else
            {
                StatePatttern(MonsterState.Trace);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}






