using UnityEngine;

public class Monster1 : MonsterBase
{

    //���� ���͵���  attack���� �����Ǹ� �ϸ� ZombiAttack�� �ű⼭ �����Ǹ� �Ѵ�.
    //monsterbase->attackstate->attack
    public override void Attack()//�ڽĿ��� ���ݺκ��� �������Ѵ�.
    {
        Debug.Log("������");
        animator.SetBool("IsAttack", true);
    }

    public override void Move()
    {
        
        //Debug.Log("�����̴���");
    }
}
