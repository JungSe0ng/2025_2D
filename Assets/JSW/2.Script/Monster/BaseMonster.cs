using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
public class BaseMonster : StatePattern<MonsterState, BaseMonster>, IProduct
{
    //몬스터 데이터 접근
    [SerializeField] protected MonsterScriptableObjects monsterDB = null;
    public MonsterScriptableObjects MonsterDB { get { return monsterDB; } }

    //공격 몬스터 리스트
    [SerializeField]
    protected List<GameObject> isAttackMonster = new List<GameObject>();

    public List<GameObject> IsAttackMonster { get { return isAttackMonster; } }

    protected IEnumerator monsterPatternCorutine = null;

    [SerializeField] protected Animator monsterAnimator = null;
    public Animator MonsterAnimator { get { return monsterAnimator; } }

    [SerializeField] protected SpriteRenderer spriteRenderer_img = null;
    public SpriteRenderer SpriteRender_img { get { return spriteRenderer_img; } }

    [SerializeField] protected CircleCollider2D circleCollider2D = null;

    //protected AstarPathfinder aPath = null;
    //public AstarPathfinder APath {get{return aPath; }}
    private Vector3 xpos = Vector3.zero;
    private float hp = 100.0f;
 
    //몬스터 생명
    public float Hp
    {

        get { return hp; }

        set
        {
            hp = value;
            //체력이 0미만으로 내려가면 죽은 상태로 변경
            if (hp < 0.0f) StatePatttern(MonsterState.Dead);
        }
    }

    private void Awake()
    {
     //   aPath= GetComponent<AstarPathfinder>();
       // Debug.Log(aPath);
        IStateStartSetting();
    }

    //몬스터가 생성되었을 경우
    public virtual void Initialize()
    {
        StatePatttern(MonsterState.Idle);
    }

    //몬스터 활성화
    void OnEnable()
    {
        Debug.Log("시작합니다.");
        StatePatttern(MonsterState.Idle);
        monsterPatternCorutine = CorutinePattern();
        StartCoroutine(monsterPatternCorutine);
        MonsterDataSetting();
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

    //몬스터 상태 시작 설정
    protected override void IStateStartSetting()
    {

        //데이터 값들 설정 적용
        MonsterDataSetting();
    }
    private void MonsterDataSetting()
    {
        hp = monsterDB.MonsterHp;
        circleCollider2D.radius = monsterDB.IsTraceArea;
    }
    protected override IEnumerator CorutinePattern()
    {
        yield break;
    }
    void Update()
    {
        UpdateSetting();
    }
    protected override void UpdateSetting()
    {
        if (machine != null)
        {
            machine.DoOperateUpdate();
        }

    }

    //타워주변에 있는 몬스터를 감지하여 해당 몬스터를 List에 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("타겟을 감지했습니다.");
        AddListIsAttackTarget(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
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
        return false;
    }

    //flip해서 x축이 바뀌면 x값 위치 보정
    public void FlipXposChange(bool isRight)
    {
        // right면 x = 0.35, left면 x = -0.35로 위치 이동
        xpos = spriteRenderer_img.gameObject.transform.localPosition;
        xpos.x = isRight ? monsterDB.XFlipPos : monsterDB.XFlipPos * -1;
        spriteRenderer_img.gameObject.transform.localPosition = xpos;
    }

    public IEnumerator CorutineVir(bool isStop)
    {
        while (isStop&&isAttackMonster.Count>0)
        {
            float dirX = isAttackMonster[0].transform.position.x - transform.position.x;

            if (Mathf.Abs(dirX) > 0.01f) // 거의 같은 위치면 무시
            {
                spriteRenderer_img.flipX = dirX < 0;  // 왼쪽이면 true, 오른쪽이면 false
                FlipXposChange(dirX>0);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //colider를 사용해서 몬스터 감지
}
