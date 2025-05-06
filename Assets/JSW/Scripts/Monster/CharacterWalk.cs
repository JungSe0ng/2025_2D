using UnityEngine;

public class CharacterWalk : MonoBehaviour
{
    protected MonsterBase monsterBase = null;
    
    protected TestNavi testNavi = null;

    //캐릭터 이동
    protected Vector2 moveVir = Vector2.zero;

    //목표의 위치로
    protected Vector2 targetList = Vector2.zero;

    protected int moveNodeListNum = 1;

    protected float adjustNum = 1f;
    protected Animator animator = null;
    protected Rigidbody2D rb2d = null;

    protected void MoveTarget() //타겟으로 이동
    {
        //현재 위치와 목표위치를 받아서 해당 방향으로 이동하고 해당 위치로 이동이 끝나면 다음 목표를 설정한다. 이 과정을 계속 반복한다.
        //현재 노드가 없으면 리턴
        if (testNavi.FinalNodeList.Count <= 0) return;
        //이미 끝났으면 리턴
        if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) return;

        //targetList설정하기
        targetList.x = testNavi.FinalNodeList[moveNodeListNum].x;
        targetList.y = testNavi.FinalNodeList[moveNodeListNum].y;
        
        Vector2 currentNode = new Vector2(testNavi.FinalNodeList[moveNodeListNum - 1].x, testNavi.FinalNodeList[moveNodeListNum - 1].y);

        moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
        moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
        DecreaseSpeed();
        //현재 위치가 목표 위치면 다음 노드 번호를 증가시킨다.
        if (Mathf.Abs(monsterBase.transform.position.x - targetList.x) < 0.2f)
        {
            moveNodeListNum++;

            //마지막이면 속도를 줄임
            if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) { adjustNum = 0; }
        }

        Jump();

        //현재 위치가 목표 위치면 다음 노드 번호를 증가시킨다.
        rb2d.linearVelocity = moveVir * monsterBase.MonsterDB.MoveSpeed * adjustNum;
        PlayerRotation();
    }

    private void Jump()
    {
        if (testNavi.FinalNodeList[moveNodeListNum].nodeType == NodeType.Jump && testNavi.FinalNodeList[(moveNodeListNum == 1) ? 0 : moveNodeListNum - 1].nodeType == NodeType.Jump)
        {
            rb2d.AddForce(new Vector2(0, 0.5f) * 10); rb2d.gravityScale = 2;
        }
        else
        {
            rb2d.gravityScale = 3f;
        }
    }
    private void DecreaseSpeed() //속도를 조절
    {
        if (moveVir.y < 0) adjustNum = 0.5f;
        else if (moveVir.y >= 1) adjustNum = 1.1f;
        else adjustNum = 1f;
    }
    //캐릭터 회전
    private void PlayerRotation() // 플레이어 회전 설정
    {
        //위로 가는지 확인
        int go = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;

        //이전에 가던 방향 확인
        int back = 0;
        if (moveNodeListNum <= 1) { back = 0; }
        else
        {
            back = testNavi.FinalNodeList[moveNodeListNum - 1].y - testNavi.FinalNodeList[moveNodeListNum - 2].y;
        }

        //이전 방향이 0이면서 현재 방향이 1이면 해당 방향으로 25도를 회전한다.
        //이전 방향이 1이면서 현재 방향이 0이면 해당 방향으로 45도를 회전한다.
        //이전 방향이 0이면서 현재 방향이 -1이면 해당 방향으로 -15도를 회전한다.
        //이전 방향이 -1이면서 현재 방향이 0이면 해당 방향으로 -15도를 회전한다.
        if ((back == 0 && go == 1))
        {
            monsterBase.transform.rotation = Quaternion.Euler(0, 0, 25);
        }
        else if ((back == 1 && go == 0))
        {
            monsterBase.transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if ((back == 0 && go == -1) || (back == -1 && go == 0))
        {
            monsterBase.transform.rotation = Quaternion.Euler(0, 0, -15);
        }
        else if (go >= 1) { monsterBase.transform.rotation = Quaternion.Euler(0, 0, 45); }
        else if (go < 0) { monsterBase.transform.rotation = Quaternion.Euler(0, 0, -45); }
        else { monsterBase.transform.rotation = Quaternion.Euler(0, 0, 0); }
        Debug.LogWarning(moveNodeListNum+"  " +go + "  " + back);
    }
}
