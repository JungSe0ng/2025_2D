using UnityEngine;

public enum LayerNum
{
    Tower =3,Monster =6, Bullet =7, Human =8
}
public enum TowerState
{
    Idle = 0, Attack =1, CoolTime =2, UpGrade =3, DeActive =4
}
public enum MonsterState
{
    Idle = 0, Walk = 1, Attack =2, Trace =3,  CoolTime =4, Dead =5
}

public enum NormalMonsterAnim{
    IsWalk, IsDead, IsAttack
}

public enum TowerBuildState{
    Empty = 0, Bulid =1
}

public enum MonsterMode{
    Normal, WaveMode
}