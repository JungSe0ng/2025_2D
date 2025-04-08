using UnityEngine;

public class Monster1 : MonsterBase
{

    //각자 몬스터들을  attack에서 재정의를 하면 ZombiAttack은 거기서 재정의를 한다.
    //monsterbase->attackstate->attack
    public override void Attack()//자식에서 공격부분을 재정의한다.
    {
        Debug.Log("공격중");
        animator.SetBool("IsAttack", true);
    }

    public override void Move()
    {
        
        //Debug.Log("움직이는중");
    }
}
