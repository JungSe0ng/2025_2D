using UnityEngine;
public class MonsterBase : MonoBehaviour
{
    //몬스터 공격범위 설정
    [SerializeField]
    private CircleCollider2D attackArea = null;

    //몬스터 스크립터블 오브젝트 정보
    [SerializeField]
    public MonsterScriptableObjects monsterDB = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌함");
    }
    private void Awake()
    {
        MonsterAwakeSetting();
    }
    private void MonsterAwakeSetting()//몬스터 초기세팅
    {
        //몬스터 공격 범위 설정
        attackArea.radius = monsterDB.IsAttackArea;
    }

    public virtual void Attack()
    {

    }
    public virtual void Move()
    {

    }
    //몬스터 기본 공격
    //몬스터 이동 기능
}
