using UnityEngine;

public class WalkState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;
    private TestNavi testNavi = null;
    private Rigidbody2D rb2d = null;

    //움직일 방향
    private Vector2 moveVir = Vector2.zero;

    //목표물 위치값
    private Vector2 targetList = Vector2.zero;

    private int moveNodeListNum = 1;

    private float adjustNum = 1f;
    public WalkState(MonsterBase monster)
    {
        this.monster = monster;
    }
    //시작할 경우 astar로 방향 설정하기
    //walk 애니메이션 추가하기
    //walk애니메이션 종료 시 애니메이션 false하기
    public void OperateEnter(MonsterBase sender)
    {
        Debug.Log("워크 시작");
        testNavi = monster.gameObject.GetComponent<TestNavi>();
        moveNodeListNum = 1;
        testNavi.PathFinding();
        monster.animator.SetFloat("IsIdle", 1);

    }

    public void OperateExit(MonsterBase sender)
    {
        Debug.Log("워크 종료");
        monster.animator.SetFloat("IsIdle", 0);
    }

    public void OperateUpdate(MonsterBase sender)
    {
        MoveTarget();
    }

    private void MoveTarget() //타겟으로 이동
    {
        //현재 위치와 목표위치를 받아서 해당 방향으로 이동하고 해당 위치로 이동이 끝났으면 다음 목표를 업데이트 한다. 최종 리스트에 닿을 때까지 반복한다.
        //최종 목적지가 선정이 된 경우
        if (testNavi.FinalNodeList.Count <= 0) return;
        //이렇게 되었을 때 그만두어라
        if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) return;

        //targetList선택하기
        targetList.x = testNavi.FinalNodeList[moveNodeListNum].x;
        targetList.y = testNavi.FinalNodeList[moveNodeListNum].y;


        //이동할 방향을 선택
        moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
        moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
        //올라갈때는 수치값을 더 주고 내려갈때는 수치값을 줄인다.
        if (moveVir.y >= 1)
        {
            Debug.Log("올라가는중"); adjustNum =0.7f;
        }
        else if (moveVir.y < 0) { adjustNum = 0.3f; Debug.Log("감속중"); } else { adjustNum = 0.3f; }

        monster.rb2d.linearVelocity = moveVir*monster.monsterDB.MoveSpeed*adjustNum;

        //만약 같은 위치라면 최종 리스트 숫자를 업데이트한다.
        if(Mathf.Abs(monster.transform.position.x-targetList.x)<0.1f)
        //if (Vector2.Distance((Vector2)monster.transform.position, targetList) < 1f)
        {
            Debug.Log("목표물에 도착했습니다.");
            //monster.transform.position = targetList;
            moveNodeListNum++;
        }
        else
        {
            
            Debug.Log($"{(Vector2)monster.transform.position}  {targetList}   {Vector2.Distance((Vector2)monster.transform.position, targetList)} /  {moveNodeListNum}   {monster.rb2d.linearVelocity.x}  {monster.rb2d.linearVelocity.y}  {adjustNum}");
            //Debug.Log(moveNodeListNum);
        }
    }
}
