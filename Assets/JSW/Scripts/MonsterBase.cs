using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterBase : MonoBehaviour
{
    //���� ���ݹ��� ����
    [SerializeField]
    private CircleCollider2D attackArea = null;

    //���� ��ũ���ͺ� ������Ʈ ����
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

    private IEnumerator CorutineMonsterPattern()//���� ���� ����
    {
        while (MonsterState.Dead != monsterState)
        {
            //���� ���� ���Ͱ� ���� ���
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

        //���� ���·� ����

    }

    //���� ������ �����ؼ� �ش� ���ϴ�� �����ϵ��� ����
    //�⺻ ������ ��� �׳� ��� ������ ���� ������
    //���̳� ��ǥ���� ������ ��� ���ݸ��� ��ȯ

    //���ݹ������� �� ���͸� �����ؼ� �ش� ���͸� List�� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("���͸� �����߽��ϴ�.");
        AddListIsAttackMonster(collision.gameObject);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutListIsAttackMonster(collision.gameObject);
    }

    private void AddListIsAttackMonster(GameObject collision)// ���� ���� ���͸� List�� �߰���
    {
        if (collision.layer == (int)LayerNum.Enemy) isAttackMonster.Add(collision.gameObject);
    }

    private void OutListIsAttackMonster(GameObject collision)//����Ʈ�� �ش� ���Ͱ� �ִ��� Ȯ���� �� ������ �� �κ��� ����Ʈ���� ������
    {
        if (collision.layer == (int)LayerNum.Enemy)
        {
            if (isAttackMonster.Contains(collision.gameObject)) isAttackMonster.Remove(collision.gameObject);
        }
    }

    private void MonsterAwakeSetting()//���� �ʱ⼼��
    {
        //���� ���� ���� ����
        attackArea.radius = monsterDB.IsAttackArea;
        animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {

    }
    public virtual void Move()
    {

    }

    //Colider�� ���Ͱ� ������ ������

    //���� �⺻ ����
    //���� �̵� ���
    public enum LayerNum { Enemy = 7, Team = 8 }
    private enum MonsterState { Idle = 0, Walk = 1, Attack = 2, Dead = 3 } //�⺻ ����, �ȱ�, ����, ����
}
