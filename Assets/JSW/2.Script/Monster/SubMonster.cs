using UnityEngine;
using SubMonsterState;
using System.Collections;

public class SubMonster : NormalMonster
{
    public bool isChangeState = false;
    [SerializeField] private GameObject missile = null;
    public GameObject Missile { get { return missile; } }

    //총알 부모 위치
    [SerializeField] private Transform bulletParent = null;
    public Transform BulletParent { get { return bulletParent; } }

    protected override void IStateStartSetting()
    {
        base.IStateStartSetting();
        IState<BaseMonster> idle = new SubMonsterStateIdle(this);
        IState<BaseMonster> walk = new SubMonsterStateWalk(this);
        IState<BaseMonster> attack = new SubMonsterStateAttack(this);
        IState<BaseMonster> dead = new SubMonsterStateDead(this);
        IState<BaseMonster> coolTime = new SubMonsterStateCoolTime(this);
        IState<BaseMonster> trace = new SubMonsterStateTrace(this);

        dicState.Add(EMonsterState.Idle, idle);
        dicState.Add(EMonsterState.Walk, walk);
        dicState.Add(EMonsterState.CoolTime, coolTime);
        dicState.Add(EMonsterState.Attack, attack);
        dicState.Add(EMonsterState.Dead, dead);
        dicState.Add(EMonsterState.Trace, trace);

        machine = new StateMachine<BaseMonster>(this, dicState[EMonsterState.Idle]);
    }
    protected override IEnumerator CorutinePattern()
    {
        while (dicState[EMonsterState.Dead] != machine.CurState)
        {
            if (!isChangeState)
            {
                //공격 범위내에 들어온 몬스터를 찾았다?
                if (isAttackMonster.Count > 0)
                {
                    //거리가 멀면 해당 몬스터로 이동한다. or 거리가 공격 범위내에 있다? 그럼 공격한다.
                    float dis = Vector3.Distance(isAttackMonster[0].transform.position, transform.position);

                    //거리가 1미만으로 이동하면 공격모드로 전환한다.?
                    if (dis < monsterDB.IsAttackArea)
                    {
                        StatePatttern(EMonsterState.Attack);
                    }
                    else if (dis > monsterDB.IsAttackArea) StatePatttern(EMonsterState.Walk);

                }
                else
                {
                    StatePatttern(EMonsterState.Trace);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
}