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


        dicState.Add(EMonsterState.Idle, idle);
        dicState.Add(EMonsterState.Attack, attack);
        dicState.Add(EMonsterState.Trace, trace);
        dicState.Add(EMonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[EMonsterState.Idle]);
    }

    protected override IEnumerator CorutinePattern()
    {
        while (dicState[EMonsterState.Dead] != machine.CurState)
        {

            //공격 범위내에 들어온 몬스터를 찾았다?
            if (isAttackMonster.Count > 0)
            {
                StatePatttern(EMonsterState.Attack);
            }
            else
            {
                StatePatttern(EMonsterState.Trace);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}






