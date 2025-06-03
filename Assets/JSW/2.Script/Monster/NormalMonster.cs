using UnityEngine;
using System.Collections;
public class NormalMonster : BaseMonster, IProduct
{

    public override void Initialize()
    {
        base.Initialize();
    }
    protected override IEnumerator CorutinePattern()
    {
        while (dicState[EMonsterState.Dead] != machine.CurState)
        {

            //공격 범위내에 들어온 몬스터를 찾았다?
            if (isAttackMonster.Count > 0)
            {
                //거리가 멀면 해당 몬스터로 이동한다. or 거리가 공격 범위내에 있다? 그럼 공격한다.
                float dis = Vector3.Distance(isAttackMonster[0].transform.position, transform.position);

                //거리가 1미만으로 이동하면 공격모드로 전환한다.?
                if (dis < monsterDB.IsAttackArea) StatePatttern(EMonsterState.Attack);
                if (dis > monsterDB.IsAttackArea) StatePatttern(EMonsterState.Walk);
            }
            else
            {
                StatePatttern(EMonsterState.Trace);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
