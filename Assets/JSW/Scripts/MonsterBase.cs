using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterBase : MonoBehaviour
{
    //몬스터 공격범위 설정
    [SerializeField]
    private CircleCollider2D attackArea = null;

    //몬스터 스크립터블 오브젝트 정보
    [SerializeField]
    public MonsterScriptableObjects monsterDB = null;

    [SerializeField]
    private List<GameObject> isAttackMonster = new List<GameObject>();

    private MonsterState monsterState = MonsterState.Idle;

    private Animator animator = null;

    private void Awake()
    {
        MonsterAwakeSetting();
    }
    private void Start()
    {
        StartCoroutine(CorutineMonsterPattern());
        
    }

    private IEnumerator CorutineMonsterPattern()//몬스터 패턴 정의
    {
        while (MonsterState.Dead != monsterState)
        {
            //공격 가능 몬스터가 있을 경우
            if (isAttackMonster.Count > 0)
            {
                monsterState = MonsterState.Attack;
                animator.SetBool("IsAttack", true);
                Attack();
            }
            else
            {
                Move();
                transform.position += new Vector3(-0.003f, 0);
                animator.SetFloat("IsIdle", 1);
            }

            yield return new WaitForFixedUpdate();

            if (monsterState == MonsterState.Attack)
            {
                monsterState = MonsterState.Idle;
                animator.SetBool("IsAttack", false);
            }
        }

        //죽은 상태로 변경

    }

    //몬스터 패턴을 정의해서 해당 패턴대로 실행하도록 실행
    //기본 상태일 경우 그냥 상대 본진을 향해 움직임
    //적이나 목표물에 도착할 경우 공격모드로 전환

    //공격범위내에 온 몬스터를 감지해서 해당 몬스터를 List에 넣음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("몬스터를 감지했습니다.");
        AddListIsAttackMonster(collision.gameObject);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutListIsAttackMonster(collision.gameObject);
    }

    private void AddListIsAttackMonster(GameObject collision)// 공격 가능 몬스터를 List에 추가함
    {
        if (collision.layer == (int)LayerNum.Enemy) isAttackMonster.Add(collision.gameObject);
    }

    private void OutListIsAttackMonster(GameObject collision)//리스트에 해당 몬스터가 있는지 확인한 후 있으면 그 부분을 리스트에서 삭제함
    {
        if (collision.layer == (int)LayerNum.Enemy)
        {
            if (isAttackMonster.Contains(collision.gameObject)) isAttackMonster.Remove(collision.gameObject);
        }
    }

    private void MonsterAwakeSetting()//몬스터 초기세팅
    {
        //몬스터 공격 범위 설정
        attackArea.radius = monsterDB.IsAttackArea;
        animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {

    }
    public virtual void Move()
    {

    }

    //Colider에 몬스터가 닿으면 공격함

    //몬스터 기본 공격
    //몬스터 이동 기능
    public enum LayerNum { Enemy = 7, Team = 8 }
    private enum MonsterState { Idle = 0, Walk = 1, Attack = 2, Dead = 3 } //기본 상태, 걷기, 공격, 죽음
}
