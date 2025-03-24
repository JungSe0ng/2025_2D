using UnityEngine;

public class IdleState : MonoBehaviour, IState<MonsterBase>
{

    private MonsterBase monsterBase = null;

    public IdleState(MonsterBase monsterBase)
    {
        this.monsterBase = monsterBase;
    }

    public void OperateEnter(MonsterBase sender)
    {
        // throw new System.NotImplementedException();
        Debug.Log("idle ����");
    }

    public void OperateExit(MonsterBase sender)
    {
        Debug.Log("idle ����");
    }

    public void OperateUpdate(MonsterBase sender)
    {
        Debug.Log(1);
    }
}
