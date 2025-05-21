using UnityEngine;
[CreateAssetMenu(fileName = "MonsterScriptableObjects", menuName = "Scriptable Object/MonsterScriptableObjects", order = 0)]
public class MonsterScriptableObjects : ScriptableObject
{
    //몬스터 코드 번호
    [SerializeField]
    private int monsterCodeName;
    public int MonsterCodeName { get { return monsterCodeName; } }

    //몬스터 이름
    [SerializeField]
    private string monsterName;
    public string MonsterName { get { return monsterName; } }

    //몬스터 체력
    [SerializeField]
    private int monsterHp;
    public int MonsterHp { get { return monsterHp; } }

    //몬스터 공격력
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    //이동 속도
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    //공격 범위 쿨타임
    [SerializeField]
    private float isAttackArea;
    public float IsAttackArea { get {return isAttackArea;} }

     //정찰 범위
    [SerializeField]
    private float isTraceArea;
    public float IsTraceArea { get {return isTraceArea;} }

    [SerializeField]
    private float isIdleCoolTime;
    public float IsCoolTime { get {return isIdleCoolTime;} }

    //공격 쿨타임
    [SerializeField]
    private float attackCoolTime;
    public float AttackCoolTime { get {return attackCoolTime;} }
}
