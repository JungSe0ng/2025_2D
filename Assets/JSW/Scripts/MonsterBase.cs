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

    [SerializeField]
    private int moveNodeListNum = 1;

    [SerializeField]
    private float moveSpeed = 1f;

    public Animator animator = null;

    //astar�˰���
    private TestNavi testNavi = null;

    //������ ����
    private Vector2 moveVir = Vector2.zero;

    //��ǥ�� ��ġ��
    private Vector2 targetList = Vector2.zero;

    private bool isResetPath = false;

    public Rigidbody2D rb2d = null;

    //���µ��� �̸� �����ؼ� �����س��� �ʿ��� �� ���¸� �������� ������� ���
    private Dictionary<MonsterState, IState<MonsterBase>> dicState = new Dictionary<MonsterState, IState<MonsterBase>>();


    private StateMachine<MonsterBase> machine = null;

    //���� ����
    private float hp = 100.0f;
    public float Hp
    {

        private get { return hp; }

        set
        {
            hp = value;
            //ü���� 0�̸����� �������� ���� ���·� ����
            if (hp < 0.0f) StatePatttern(MonsterState.Dead);
        }
    }

    private void Awake()
    {
        MonsterAwakeSetting();
    }
    private void Start()
    {
        StartCoroutine(CorutineMonsterPattern());
    }
    private void Update()
    {
        if (machine != null)
        {
            machine.DoOperateUpdate();
        }
        ConcerFactory con1 = new ConcerFactory();
    }

    private void MonsterAwakeSetting()//���� �ʱ⼼��
    {
        //���� ���� ���� ����
        attackArea.radius = monsterDB.IsAttackArea;
        testNavi = GetComponent<TestNavi>();
        rb2d = GetComponent<Rigidbody2D>();


        //���¸� ���� �� Dictionary�� ����
        IState<MonsterBase> idle = new IdleState(this);
        IState<MonsterBase> walk = new WalkState(this);
        IState<MonsterBase> dead = new DeadState(this);


        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Dead, dead);


        machine = new StateMachine<MonsterBase>(this, dicState[MonsterState.Idle]);
        machine.SetState(dicState[MonsterState.Walk]);
    }


    private IEnumerator CorutineMonsterPattern()//���� ���� ����
    {
        while (dicState[MonsterState.Dead] != machine.CurState)
        {
            //���� ���� ���Ͱ� ���� ���
            if (isAttackMonster.Count > 0)
            {
                StatePatttern(MonsterState.Attack);
            }
            else
            {
                StatePatttern(MonsterState.Walk);
            }

            yield return new WaitForFixedUpdate();
        }
    }
    private void StatePatttern(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Idle:
                machine.SetState(dicState[MonsterState.Idle]);
                break;

            case MonsterState.Walk:
                //���� �� �ٽ� walk�� ���� �κ��� �߰��ϱ�
                Move();
                machine.SetState(dicState[MonsterState.Walk]);
                break;

            case MonsterState.Attack:
                machine.SetState(dicState[MonsterState.Attack]);
                Attack();
                break;

            case MonsterState.Dead:
                machine.SetState(dicState[MonsterState.Dead]);
                break;

        }
    }


    //���� ������ �����ؼ� �ش� ���ϴ�� �����ϵ��� ����
    //�⺻ ������ ��� �׳� ��� ������ ���� ������
    //���̳� ��ǥ���� ������ ��� ���ݸ��� ��ȯ

    //���ݹ������� �� ���͸� �����ؼ� �ش� ���͸� List�� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("���͸� �����߽��ϴ�.");
        AddListIsAttackMonster(collision.gameObject);
        Debug.Log(collision.gameObject.layer);
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


    public virtual void Attack()
    {

    }
    public virtual void Move()
    {


    }

    public enum Direction
    {
        Left = -1,
        Right = 1,
    }
    //���� �⺻ ����
    //���� �̵� ���
    public enum LayerNum { Enemy = 7, Team = 8 }

}
