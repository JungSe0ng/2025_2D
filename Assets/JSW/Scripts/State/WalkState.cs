using UnityEngine;

public class WalkState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;
    private TestNavi testNavi = null;
    // private Rigidbody2D rb2d = null;

    //������ ����
    private Vector2 moveVir = Vector2.zero;

    //��ǥ�� ��ġ��
    private Vector2 targetList = Vector2.zero;

    private int moveNodeListNum = 1;

    private float adjustNum = 1f;
    public WalkState(MonsterBase monster)
    {
        this.monster = monster;
    }
    //������ ��� astar�� ���� �����ϱ�
    //walk �ִϸ��̼� �߰��ϱ�
    //walk�ִϸ��̼� ���� �� �ִϸ��̼� false�ϱ�
    public void OperateEnter(MonsterBase sender)
    {
        Debug.Log("��ũ ����");
        testNavi = monster.gameObject.GetComponent<TestNavi>();
        moveNodeListNum = 1;
        testNavi.PathFinding();
        monster.animator.SetFloat("IsIdle", 1);

    }

    public void OperateExit(MonsterBase sender)
    {
        Debug.Log("��ũ ����");
        monster.animator.SetFloat("IsIdle", 0);
    }

    public void OperateUpdate(MonsterBase sender)
    {
        MoveTarget();
    }

    private void MoveTarget() //Ÿ������ �̵�
    {
        //���� ��ġ�� ��ǥ��ġ�� �޾Ƽ� �ش� �������� �̵��ϰ� �ش� ��ġ�� �̵��� �������� ���� ��ǥ�� ������Ʈ �Ѵ�. ���� ����Ʈ�� ���� ������ �ݺ��Ѵ�.
        //���� �������� ������ �� ���
        if (testNavi.FinalNodeList.Count <= 0) return;
        //�̷��� �Ǿ��� �� �׸��ξ��
        if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) return;

        //targetList�����ϱ�
        targetList.x = testNavi.FinalNodeList[moveNodeListNum].x;
        targetList.y = testNavi.FinalNodeList[moveNodeListNum].y;

        Vector2 currentNode = new Vector2(testNavi.FinalNodeList[moveNodeListNum - 1].x, testNavi.FinalNodeList[moveNodeListNum - 1].y);

        //�̵��� ������ ����

        moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
        moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
        //�ö󰥶��� ��ġ���� �� �ְ� ���������� ��ġ���� ���δ�.
        //�����Ҷ� �߷°��� 1 �ö󰥶� 1 �׳� or ������ �� �߰� ��ġ �ο�

        DecreaseSpeed();
        //���� ���� ��ġ��� ���� ����Ʈ ���ڸ� ������Ʈ�Ѵ�.
        if (Mathf.Abs(monster.transform.position.x - targetList.x) < 0.2f)
        {
            moveNodeListNum++;

            //���������� �����ϸ� �������� ����
            if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) { adjustNum = 0; }
        }

        Jump();
        monster.rb2d.linearVelocity = moveVir * monster.monsterDB.MoveSpeed * adjustNum;
        Debug.Log($"{moveVir}");
        PlayerRotation();
    }
    private void Jump()
    {
        if (testNavi.FinalNodeList[moveNodeListNum].nodeType == NodeType.Jump && testNavi.FinalNodeList[(moveNodeListNum == 1) ? 0 : moveNodeListNum - 1].nodeType == NodeType.Jump)
        {
            monster.rb2d.AddForce(new Vector2(0, 0.5f) * 10); Debug.Log("�����߽��ϴ�."); monster.rb2d.gravityScale = 2;
        }
        else
        {
            monster.rb2d.gravityScale = 3f;
        }
    }
    private void DecreaseSpeed() //������ �� ����
    {
        if (moveVir.y < 0) adjustNum = 0.5f;
        else if (moveVir.y >= 1) adjustNum = 1.1f;
        else adjustNum = 1f;
    }
    //���� ����
    private void PlayerRotation() // �÷��̾� ȸ���� ����
    {
        //������ �� ����
        int go = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;

        //������ ������ ����
        int back = 0;
        if (moveNodeListNum <= 1) { back = 0; }
        else
        {
            back = testNavi.FinalNodeList[moveNodeListNum - 1].y - testNavi.FinalNodeList[moveNodeListNum - 2].y;
        }

        //������忡�� ����� ���Ⱚ�� ����̰� ������忡���� ����� ��� ������ �߰������� �����Ѵ�.
        //������ �� ��� �׻� 0�� ���� 
        //���� ����� �÷��̾�� 45������ ȸ���� �����ϴ�.

        //������� ������ 0�̸鼭 ���� ��尡 1�� ��� �ش� ������ 25���� �����Ѵ�.
        //������� ������ 1�̸鼭 ���� ��尡 0�� ��� �ش� ������ 25���� �����Ѵ�.
        if ((back == 0 && go == 1))
        {
            monster.transform.rotation = Quaternion.Euler(0, 0, 25);
        }
        else if ((back == 1 && go == 0))
        {
            monster.transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if ((back == 0 && go == -1) || (back == -1 && go == 0))
        {
            monster.transform.rotation = Quaternion.Euler(0, 0, -15);
        }
        else if (go >= 1) { monster.transform.rotation = Quaternion.Euler(0, 0, 45); }
        else if (go < 0) { monster.transform.rotation = Quaternion.Euler(0, 0, -45); }
        else { monster.transform.rotation = Quaternion.Euler(0, 0, 0); }
        // Debug.Log(go + " "+back);
    }

}

//���� ��尡 �������� Ȯ���� �ؾ���
