using UnityEngine;
using System.Collections;
using BaseMonsterState;
public class NormalMonster : BaseMonster, IProduct
{
    
    public override void Initialize()
    {
        base.Initialize();
    }
     protected override void IStateStartSetting()
    {
        
        base.IStateStartSetting();
        IState<BaseMonster> idle = new BaseMonsterIdle(this);
        IState<BaseMonster> walk = new BaseMonsterWalk(this);
        IState<BaseMonster> trace = new BaseMonsterTrace(this);
        IState<BaseMonster> attack = new BaseMonsterAttack(this);
        IState<BaseMonster> dead = new BaseMonsterDead(this);


        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
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
                //거리가 멀면 해당 몬스터로 이동한다. or 거리가 공격 범위내에 있다? 그럼 공격한다.
                float dis = Vector3.Distance(isAttackMonster[0].transform.position, transform.position);
                if (dis < monsterDB.IsAttackArea) StatePatttern(MonsterState.Attack);
                if (dis > monsterDB.IsAttackArea) StatePatttern(MonsterState.Walk);
            }
            else
            {
                StatePatttern(MonsterState.Trace);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
