using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using PlayerEnum;
public class PlayerBase : StatePattern<EPlayerState, PlayerBase>, IProduct
{
    // 플레이어 공격범위 콜라이더
    [SerializeField]
    private CircleCollider2D attackArea = null;

    // 플레이어 스크립터블 오브젝트 데이터
    [SerializeField]
    private PlayerScriptableObjects playerDB = null;
    public PlayerScriptableObjects PlayerDB { get { return playerDB; } }

    [SerializeField]
    private List<GameObject> isAttackPlayer = new List<GameObject>();

    // 플레이어 HP
    private float hp = 100.0f;
    public float Hp
    {
        private get { return hp; }
        set
        {
            hp = value;
            //체력이 0이하라면 상태를 Dead로 전환
            if (hp < 0.0f) StatePatttern(EPlayerState.Dead);
        }
    }

    public string codeName { get; set; }

    private void Awake()
    {
        PlayerAwakeSetting();
    }
    private void Start()
    {
        StartCoroutine(CorutinePattern());
    }

    private void Update()
    {
        UpdateSetting();
    }

    private void PlayerAwakeSetting()//플레이어 초기설정
    {
        //공격 범위 반영
        attackArea.radius = playerDB.IsAttackArea;
        IStateStartSetting();
    }

    //해당 자식에서 OVERRIDE로 구현하세요
    protected override void IStateStartSetting() { }
   
    protected override IEnumerator CorutinePattern()
    {
        while (dicState[EPlayerState.Dead] != machine.CurState)
        {
            //공격 대상 리스트가 비어있지 않으면 공격 상태로
            if (isAttackPlayer.Count > 0)
            {
                StatePatttern(EPlayerState.Attack);
            }
            else
            {
                StatePatttern(EPlayerState.Walk);
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

    public override void StatePatttern(EPlayerState state)
    {
        
        machine.SetState(dicState[state]);
    }

    //공격 대상이 들어오면 해당 오브젝트를 리스트에 추가
    //기본 구현은 그냥 리스트에 추가만 함
    //레이어 마스킹 등은 필요에 따라 변형

    //트리거로 들어온 오브젝트를 리스트에 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddListIsAttackPlayer(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutListIsAttackPlayer(collision.gameObject);
    }

    private void AddListIsAttackPlayer(GameObject collision)// 공격 대상 오브젝트를 리스트에 추가
    {
        //if (collision.layer == (int)LayerNum.Enemy) isAttackPlayer.Add(collision.gameObject);
    }

    private void OutListIsAttackPlayer(GameObject collision)//리스트에 해당 오브젝트가 있으면 리스트에서 삭제
    {
       /* if (collision.layer == (int)LayerNum.Enemy)
        {
            Debug.Log("LIST에서 제거됨");
            if (isAttackPlayer.Contains(collision.gameObject)) isAttackPlayer.Remove(collision.gameObject);
        }*/
    }

    public void Initialize()
    {
        //오브젝트가 초기화될 때 호출
    }


} 