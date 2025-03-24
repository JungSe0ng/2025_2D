using UnityEngine;

public class DeadState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;
    public DeadState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void OperateEnter(MonsterBase sender)
    {
       
    }

    public void OperateExit(MonsterBase sender)
    {
 
    }

    public void OperateUpdate(MonsterBase sender)
    {
   
    }

}
