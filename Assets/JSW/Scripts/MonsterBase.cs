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

    [SerializeField]
    private int moveNodeListNum = 1;

    [SerializeField]
    private float moveSpeed = 1f;

    public Animator animator = null;

    //astar알고리즘
    private TestNavi testNavi = null;

    //움직일 방향
    private Vector2 moveVir = Vector2.zero;

    //목표물 위치값
    private Vector2 targetList = Vector2.zero;

    private bool isResetPath = false;

    public Rigidbody2D rb2d = null;

    //상태들을 미리 생성해서 저장해놓고 필요할 때 상태를 꺼내오는 방식으로 사용
    private Dictionary<MonsterState, IState<MonsterBase>> dicState = new Dictionary<MonsterState, IState<MonsterBase>>();


    private StateMachine<MonsterBase> machine = null;

    //몬스터 생명
    private float hp = 100.0f;
    public float Hp
    {

        private get { return hp; }

        set
        {
            hp = value;
            //체력이 0미만으로 내려가면 죽은 상태로 변경
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

    private void MonsterAwakeSetting()//몬스터 초기세팅
    {
        //몬스터 공격 범위 설정
        attackArea.radius = monsterDB.IsAttackArea;
        testNavi = GetComponent<TestNavi>();
        rb2d = GetComponent<Rigidbody2D>();


        //상태를 생성 후 Dictionary로 관리
        IState<MonsterBase> idle = new IdleState(this);
        IState<MonsterBase> walk = new WalkState(this);
        IState<MonsterBase> dead = new DeadState(this);


        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Dead, dead);


        machine = new StateMachine<MonsterBase>(this, dicState[MonsterState.Idle]);
        machine.SetState(dicState[MonsterState.Walk]);
    }


    private IEnumerator CorutineMonsterPattern()//몬스터 패턴 정의
    {
        while (dicState[MonsterState.Dead] != machine.CurState)
        {
            //공격 가능 몬스터가 있을 경우
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
                //공격 후 다시 walk로 변경 부분을 추가하기
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


    //몬스터 패턴을 정의해서 해당 패턴대로 실행하도록 실행
    //기본 상태일 경우 그냥 상대 본진을 향해 움직임
    //적이나 목표물에 도착할 경우 공격모드로 전환

    //공격범위내에 온 몬스터를 감지해서 해당 몬스터를 List에 넣음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("몬스터를 감지했습니다.");
        AddListIsAttackMonster(collision.gameObject);
        Debug.Log(collision.gameObject.layer);
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
    //몬스터 기본 공격
    //몬스터 이동 기능
    public enum LayerNum { Enemy = 7, Team = 8 }

}
