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
        Debug.Log("공격모드에 진입");
    }

    public void OperateExit(MonsterBase sender)
    {

    }

    public void OperateUpdate(MonsterBase sender)
    {

    }

}
