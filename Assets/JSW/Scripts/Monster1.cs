using UnityEngine;

public class Monster1 : MonsterBase
{
    public override void Attack()
    {
        base.Attack();
        Debug.Log("������");
    }

    public override void Move()
    {
        base.Move();
        Debug.Log("�����̴���");
    }
}
