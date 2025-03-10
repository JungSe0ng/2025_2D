using UnityEngine;

public class Monster1 : MonsterBase
{
    public override void Attack()
    {
        base.Attack();
        Debug.Log("공격중");
    }

    public override void Move()
    {
        base.Move();
        Debug.Log("움직이는중");
    }
}
