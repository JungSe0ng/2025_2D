using UnityEngine;

public class WalkState : MonoBehaviour, IState<MonsterBase>
{
    private MonsterBase monster = null;
    private TestNavi testNavi = null;
    private Rigidbody2D rb2d = null;

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


        //�̵��� ������ ����
        moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
        moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
        //�ö󰥶��� ��ġ���� �� �ְ� ���������� ��ġ���� ���δ�.
        if (moveVir.y >= 1)
        {
            Debug.Log("�ö󰡴���"); adjustNum =0.7f;
        }
        else if (moveVir.y < 0) { adjustNum = 0.3f; Debug.Log("������"); } else { adjustNum = 0.3f; }

        monster.rb2d.linearVelocity = moveVir*monster.monsterDB.MoveSpeed*adjustNum;

        //���� ���� ��ġ��� ���� ����Ʈ ���ڸ� ������Ʈ�Ѵ�.
        if(Mathf.Abs(monster.transform.position.x-targetList.x)<0.1f)
        //if (Vector2.Distance((Vector2)monster.transform.position, targetList) < 1f)
        {
            Debug.Log("��ǥ���� �����߽��ϴ�.");
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
