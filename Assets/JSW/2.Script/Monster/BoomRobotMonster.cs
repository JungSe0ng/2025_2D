using UnityEngine;
using BoomRobotMonsterState;
using System.Collections.Generic;
public class BoomRobotMonster : NormalMonster
{

    protected override void IStateStartSetting()
    {

        base.IStateStartSetting();
        IState<BaseMonster> idle = new BoomRobotMonsterIdle(this);
        IState<BaseMonster> walk = new BoomRobotMonsterWalk(this);
        IState<BaseMonster> trace = new BoomRobotMonsterTrace(this);
        IState<BaseMonster> attack = new BoomRobotMonsterAttack(this);
        IState<BaseMonster> dead = new BoomRobotMonsterDead(this);


        dicState.Add(MonsterState.Idle, idle);

        dicState.Add(MonsterState.Walk, walk);
        dicState.Add(MonsterState.Attack, attack);
        dicState.Add(MonsterState.Trace, trace);
        dicState.Add(MonsterState.Dead, dead);

        machine = new StateMachine<BaseMonster>(this, dicState[MonsterState.Idle]);
    }
    public Transform player;
    public Transform monster;
    public LayerMask obstacleMask;
    public float moveSpeed = 2f;
    public int gridSize = 21;

    private AStar2DPathfinder pathfinder;
    private List<Vector2> currentPath = null;
    private int pathIndex = 0;

    void Start()
    {
        pathfinder = new AStar2DPathfinder(gridSize, gridSize, obstacleMask);
    }

    // 플레이어 위치로 경로 요청
    public void MoveToPlayer()
    {
        currentPath = pathfinder.GetPathToPlayer(monster.position, player);
        pathIndex = 0;
    }

    // 특정 좌표로 경로 요청
    public void MoveToPosition(Vector2 targetPos)
    {
        currentPath = pathfinder.GetPathToTarget(monster.position, targetPos);
        pathIndex = 0;
    }

    void Update()
    {
        if (currentPath == null || pathIndex >= currentPath.Count) return;
        
        Vector2 target = currentPath[pathIndex];
        monster.position = Vector2.MoveTowards(monster.position, target, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(monster.position, target) < 0.05f)
        {
            pathIndex++;
        }
    }

    // 예시 키 입력으로 테스트
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MoveToPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MoveToPosition(new Vector2(10, 5));
        }
    }
}


