using UnityEngine;
[CreateAssetMenu(fileName = "PlayerScriptableObjects", menuName = "Scriptable Object/PlayerScriptableObjects", order = 0)]
public class PlayerScriptableObjects : ScriptableObject
{
    // 플레이어 코드 번호
    [SerializeField]
    private int playerCodeName;
    public int PlayerCodeName { get { return playerCodeName; } }

    // 플레이어 이름
    [SerializeField]
    private string playerName;
    public string PlayerName { get { return playerName; } }

    // 플레이어 HP
    [SerializeField]
    private int playerHp;
    public int PlayerHp { get { return playerHp; } }

    // 플레이어 공격력
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    // 플레이어 이동 속도
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    // 플레이어 공격 범위
    [SerializeField]
    private float isAttackArea;
    public float IsAttackArea { get {return isAttackArea;} }
}
