using UnityEngine;

public class WalkState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;
    private TestNavi testNavi = null;
    //������ ����
    private Vector2 moveVir = Vector2.zero;

    //��ǥ�� ��ġ��
    private Vector2 targetList = Vector2.zero;

    private Rigidbody2D rb2d = null;

    private float moveSpeed = 20f;
    private int moveNodeListNum = 1;

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
        if (moveNodeListNum == testNavi.FinalNodeList.Count-1) return;
 
        //targetList�����ϱ�
        targetList.x = testNavi.FinalNodeList[moveNodeListNum].x;
        targetList.y = testNavi.FinalNodeList[moveNodeListNum].y;


        //�̵��� ������ ����
        moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x- testNavi.FinalNodeList[moveNodeListNum-1].x;
        moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y- testNavi.FinalNodeList[moveNodeListNum-1].y;
        
        //

        //Debug.Log(moveNodeListNum+" "+moveVir.x + ", " + moveVir.y);
        monster.rb2d.MovePosition(monster.rb2d.position+moveVir*moveSpeed*Time.deltaTime);
        //  monster.transform.position = Vector2.MoveTowards(monster.transform.position, moveVir, moveSpeed * Time.deltaTime);

        //���� ���� ��ġ��� ���� ����Ʈ ���ڸ� ������Ʈ�Ѵ�.
        if (Vector2.Distance((Vector2)monster.transform.position, targetList) < 0.3f)
        {
            Debug.Log("��ǥ���� �����߽��ϴ�.");
            monster.transform.position = targetList;
            moveNodeListNum++;
        }
        else
        {
            Debug.Log($"{(Vector2)monster.transform.position}  {targetList}   {Vector2.Distance((Vector2)monster.transform.position, targetList)}");
        }
    }
}
