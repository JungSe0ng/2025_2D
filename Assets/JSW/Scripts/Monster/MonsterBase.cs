using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using MonsterEnum;
public class MonsterBase : StatePattern<EMonsterState, MonsterBase>, IProduct
{
    // 몬스터 공격범위 콜라이더
    [SerializeField]
    private CircleCollider2D attackArea = null;

    // 몬스터 스크립터블 오브젝트 데이터
    [SerializeField]
    private MonsterScriptableObjects monsterDB = null;
    public MonsterScriptableObjects MonsterDB { get { return monsterDB; } }

    [SerializeField]
    private List<GameObject> isAttackMonster = new List<GameObject>();

    // 몬스터 HP
    private float hp = 100.0f;
    public float Hp
    {
        private get { return hp; }
        set
        {
            hp = value;
            // 체력이 0 이하라면 상태를 Dead로 전환
            
            if (hp < 0.0f) StatePatttern(EMonsterState.Dead);
        }
    }

    public string codeName { get; set; }

    private void Awake()
    {
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

    private void MonsterAwakeSetting()// 몬스터 초기설정
    {
        // 공격 범위 반영
        attackArea.radius = monsterDB.IsAttackArea;
        IStateStartSetting();
    }

    // 해당 자식에서 OVERRIDE로 구현하세요
    protected override void IStateStartSetting() { }
   
    protected override IEnumerator CorutinePattern()
    {
        
        while (dicState[EMonsterState.Dead] != machine.CurState)
        {
            // 공격 대상 리스트가 비어있지 않으면 공격 상태로
            if (isAttackMonster.Count > 0)
            {
                StatePatttern(EMonsterState.Attack);
            }
            else
            {
                StatePatttern(EMonsterState.Walk);
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

    // 공격 대상이 들어오면 해당 오브젝트를 리스트에 추가
    // 기본 구현은 그냥 리스트에 추가만 함
    // 레이어 마스킹 등은 필요에 따라 변형

    // 트리거로 들어온 오브젝트를 리스트에 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddListIsAttackMonster(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutListIsAttackMonster(collision.gameObject);
    }

    private void AddListIsAttackMonster(GameObject collision)// 공격 대상 오브젝트를 리스트에 추가
    {
        if (collision.layer == (int)LayerNum.Enemy) isAttackMonster.Add(collision.gameObject);
    }

    private void OutListIsAttackMonster(GameObject collision)// 리스트에 해당 오브젝트가 있으면 리스트에서 삭제
    {
        if (collision.layer == (int)LayerNum.Enemy)
        {
            Debug.Log("LIST에서 제거됨");
            if (isAttackMonster.Contains(collision.gameObject)) isAttackMonster.Remove(collision.gameObject);
        }
    }

    public void Initialize()
    {
        // 오브젝트가 초기화될 때 호출
    }

    public override void StatePatttern(EMonsterState state)
    {
        machine.SetState(dicState[state]);
    }

    public enum Direction
    {
        Left = -1,
        Right = 1,
    }
    // 몬스터 기본 레이어
    // 몬스터 이동 관련
    public enum LayerNum { Enemy = 7, Team = 8 }

}
