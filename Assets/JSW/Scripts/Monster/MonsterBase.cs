using BasicMonsterState;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
public class MonsterBase : StatePattern<MonsterState, MonsterBase>, IProduct
{
    //���� �ִϸ�����
    public Animator animator = null;
    public TestNavi navi = null;
    public Rigidbody2D rb2d = null;

    //���� ���ݹ��� ����
    [SerializeField]
    private CircleCollider2D attackArea = null;

    //���� ��ũ���ͺ� ������Ʈ ����
    [SerializeField]
    public MonsterScriptableObjects monsterDB = null;

    [SerializeField]
    private List<GameObject> isAttackMonster = new List<GameObject>();



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

    public string codeName { get; set; }

    private void Awake()
    {
        navi = GetComponent<TestNavi>();
        MonsterAwakeSetting();
    }
    private void Start()
    {
        StartCoroutine(CorutinePattern());
    }

    private void Update()
    {
        UpdateSetting();
    }

    private void MonsterAwakeSetting()//���� �ʱ⼼��
    {
        //���� ���� ���� ����
        attackArea.radius = monsterDB.IsAttackArea; ;
        rb2d = GetComponent<Rigidbody2D>();
        navi = GetComponent<TestNavi>();
        IStateStartSetting();
    }

    //�ش� �ڽĿ��� OVERRIDE�� ��������
    protected override void IStateStartSetting() { }
   
    protected override IEnumerator CorutinePattern()
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

    protected override void UpdateSetting()
    {
        if (machine != null)
        {
            machine.DoOperateUpdate();
        }
    }
    public override void StatePatttern(MonsterState state)
    {
        machine.SetState(dicState[state]);
    }


    //���� ������ �����ؼ� �ش� ���ϴ�� �����ϵ��� ����
    //�⺻ ������ ��� �׳� ��� ������ ���� ������
    //���̳� ��ǥ���� ������ ��� ���ݸ��� ��ȯ

    //���ݹ������� �� ���͸� �����ؼ� �ش� ���͸� List�� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    public void Initialize()
    {
        //���Ͱ� ��ȯ�Ǿ��� ���
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
