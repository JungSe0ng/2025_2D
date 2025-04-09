using UnityEngine;

public class AttackState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;

    public AttackState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void OperateEnter(MonsterBase sender)
    {
        Debug.Log("���ݸ�忡 ����");
    }

    public void OperateExit(MonsterBase sender)
    {

    }

    public void OperateUpdate(MonsterBase sender)
    {

    }

}
