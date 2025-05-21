using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
public class BaseMonster : StatePattern<MonsterState, BaseMonster>, IProduct
{

    //colider 접근 범위 설정
    [SerializeField] private BoxCollider attackArea = null;

    //몬스터 데이터 접근
    [SerializeField] private MonsterScriptableObjects monsterDB = null;
    public MonsterScriptableObjects MonsterDB { get { return monsterDB; } }

    //공격 몬스터 리스트
    [SerializeField]
    private List<GameObject> isAttackMonster = new List<GameObject>();

    [SerializeField] private NavMeshAgent agent = null;
    public NavMeshAgent Agent { get { return agent; } }

    public MonsterMode mode = MonsterMode.Normal;

    private GameObject waveTarget = null;

    private IEnumerator monsterPatternCorutine = null;

    [SerializeField] private Animator monsterAnimator = null;
    public Animator MonsterAnimator { get { return monsterAnimator; } }
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

    private void MonsterAwakeSetting()//몬스터 초기세팅
    {
        //몬스터 공격 범위 설정
        attackArea.size = new Vector3(monsterDB.IsAttackArea, 10, monsterDB.IsAttackArea);
        IStateStartSetting();
    }
    //몬스터가 생성되었을 경우
    public void Initialize()
    {
        StatePatttern(MonsterState.Idle);
    }

    //몬스터 활성화
    void OnEnable()
    {
        StatePatttern(MonsterState.Idle);
        monsterPatternCorutine = CorutinePattern();
        StartCoroutine(monsterPatternCorutine);
    }

    //몬스터 비활성화
    void OnDisable()
    {
        StopCoroutine(monsterPatternCorutine);
    }


    //
    public override void StatePatttern(MonsterState state)
    {
        machine.SetState(dicState[state]);
    }

    protected override IEnumerator CorutinePattern()
    {
        while (dicState[MonsterState.Dead] != machine.CurState)
        {

            //만약에 WaveMode이거나 목표물이 정해진다면? Run
            //공격범위 내에 있으면 Attack모드로 변경한다.
            //아무것도 아니면 Idle상태로 전환
            if (isAttackMonster.Count > 0)
            {
                float dis = Vector3.Distance(isAttackMonster[0].transform.position, transform.position);
                //대상 거리가 가까운 경우
                if (dis < 1f) StatePatttern(MonsterState.Attack);
                if (dis > 1f) StatePatttern(MonsterState.Run);
            }
            else if (mode == MonsterMode.WaveMode)
            {
                StatePatttern(MonsterState.Run);
            }
            else
            {
                StatePatttern(MonsterState.Idle);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //몬스터 상태 시작 설정
    protected override void IStateStartSetting()
    {

    }

    protected override void UpdateSetting()
    {
        if (machine != null)
        {
            machine.DoOperateUpdate();
        }

    }

    //타워주변에 있는 몬스터를 감지하여 해당 몬스터를 List에 추가
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("타겟을 감지했습니다.");
        AddListIsAttackTarget(collision.gameObject);
    }
    private void OnTriggerExit(Collider collision)
    {
        OutListIsAttackTarget(collision.gameObject);
    }

    private void AddListIsAttackTarget(GameObject collision)// 타워 주변 몬스터를 List에 추가함
    {
        if (IsAttackTargetLayer(collision)) isAttackMonster.Add(collision.gameObject);
    }

    private void OutListIsAttackTarget(GameObject collision)//리스트에 해당 몬스터가 있는지 확인 후 있다면 이 부분에서 리스트에서 제거함
    {
        if (IsAttackTargetLayer(collision))
        {
            if (isAttackMonster.Contains(collision.gameObject)) isAttackMonster.Remove(collision.gameObject);
        }
    }

    //공격 가능 Layer인지 탐색
    private bool IsAttackTargetLayer(GameObject collision)
    {
        if (collision.layer == (int)LayerNum.Human) return true;
        if (collision.layer == (int)LayerNum.Tower) return true;
        return false;
    }

    //wave에서 소환된 몬스터들이 목표지점을 향해 공격합니다.
    // wave에서 소환된 몬스터들이 목표지점을 향해 이동하고, 목표와의 거리를 디버그합니다.
    public void WaveAttackMode(GameObject target)
    {
        waveTarget = target;
        mode = MonsterMode.WaveMode;
    }


    //Agent Run세팅
    public void AgentTargetSetting()
    {
        if (isAttackMonster.Count > 0) AgentSet(isAttackMonster[0]);
        else AgentSet(waveTarget);
    }

    //Agent 일반 목표물 설정
    public void AgentSet(GameObject obj)
    {
        agent.SetDestination(obj.transform.position);
    }

    //colider를 사용해서 몬스터 감지
}
