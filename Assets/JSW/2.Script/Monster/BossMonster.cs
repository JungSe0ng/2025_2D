using UnityEngine;
using BossMonsterState;
using System.Collections;

public class BossMonster : NormalMonster
{
    //레이저
    private GameObject laser = null;
    public GameObject Laser { get { return laser; } } 

    //미사일
    private GameObject missile = null;
    public GameObject Missile { get { return missile; } }

    //총알 부모 위치
    private Transform bulletParent = null;
    public Transform BulletParent { get { return bulletParent; } }

    protected override void IStateStartSetting()
    {
        base.IStateStartSetting();
        IState<BaseMonster> idle = new BossMonsterIdle(this);
        IState<BaseMonster> walk = new BossMonsterWalk(this);
        IState<BaseMonster> brustShoot = new BossMonsterBrustShoot(this);
        IState<BaseMonster> jumpFly = new BossMonsterJumpFly(this);
        IState<BaseMonster> coolTime = new BossMonsterCoolTime(this);
        IState<BaseMonster> dead = new BossMonsterDead(this);

        dicState.Add(EMonsterState.Idle, idle);
        dicState.Add(EMonsterState.Walk, walk);
        dicState.Add(EMonsterState.BrustShoot, brustShoot);
        dicState.Add(EMonsterState.JumpFly, jumpFly);
        dicState.Add(EMonsterState.CoolTime, coolTime);
        dicState.Add(EMonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[EMonsterState.Idle]);
    }

    protected override IEnumerator CorutinePattern()
    {
            StatePatttern(EMonsterState.Idle);
            yield return new WaitForSeconds(1f);
            StatePatttern(EMonsterState.Walk);
    }
} 