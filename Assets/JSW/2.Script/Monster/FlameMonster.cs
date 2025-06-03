using UnityEngine;
using FlameRobotMonsterState;
using NormalMonsterState;
using System.Collections.Generic;
using System.Collections;
public class FlameMonster : NormalMonster
{
    public bool isChangeState = false;
    protected override void IStateStartSetting()
    {

        base.IStateStartSetting();
        IState<BaseMonster> idle = new NormalMonsterIdle(this);
        IState<BaseMonster> trace = new FlameRobotMonsterStateTrace(this);
        IState<BaseMonster> attack = new FlameRobotMonsterStateAttack(this);
        IState<BaseMonster> dead = new FlameRobotMonsterStateDead(this);
        IState<BaseMonster> coolTime = new FlameRobotMonsterStateCoolTime(this);

        dicState.Add(EMonsterState.Idle, idle);
        dicState.Add(EMonsterState.CoolTime, coolTime);
        dicState.Add(EMonsterState.Attack, attack);
        dicState.Add(EMonsterState.Trace, trace);
        dicState.Add(EMonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[EMonsterState.Idle]);

    }
    protected override IEnumerator CorutinePattern()
    {
        while (dicState[EMonsterState.Dead] != machine.CurState)
        {
            //탐지 범위내에 들어오면 공격루틴으로 
            if (isAttackMonster.Count > 0)
            {
                //쿨타임에 따라서 공격모드와 쿨타임 모드로 변환됨
                if (isChangeState == false)
                {
                    StatePatttern(EMonsterState.Attack); // -> cooltime모드로 전환시키고 다시 attackMode로 전환하지 말라고 명령을 해둠 cooltime-> idle-> attack
                }
            }
            else
            {
                StatePatttern(EMonsterState.Trace);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
