using UnityEngine;

namespace BasicMonsterState
{
    public class BasicMonsterIdle : MonoBehaviour, IState<MonsterBase>
    {
        private MonsterBase monsterBase;
        public BasicMonsterIdle(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }

        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("Idle���¿� �����߽��ϴ�.");
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("Idle���¿� �����մϴ�.");
        }

        public void OperateUpdate(MonsterBase sender)
        {
           
        }
    }
    public class BasicMonsterAttack : MonoBehaviour, IState<MonsterBase>
    {
        private MonsterBase monsterBase = null;
        public BasicMonsterAttack(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }
        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("���ݻ��¿� �����߽��ϴ�.");
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("���� ���¿� �����߽��ϴ�.");
        }

        public void OperateUpdate(MonsterBase sender)
        {

        }
    }
    public class BasicMonsterDead : MonoBehaviour, IState<MonsterBase>
    {
        private MonsterBase monsterBase = null;

        public BasicMonsterDead(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }

        public void OperateEnter(MonsterBase sender)
        {

        }

        public void OperateExit(MonsterBase sender)
        {

        }

        public void OperateUpdate(MonsterBase sender)
        {

        }
    }

    public class BasicMonsterWalk : MonoBehaviour, IState<MonsterBase>
    {
 
        private TestNavi testNavi = null;

        //������ ����
        private Vector2 moveVir = Vector2.zero;

        //��ǥ�� ��ġ��
        private Vector2 targetList = Vector2.zero;

        private int moveNodeListNum = 1;

        private float adjustNum = 1f;

        private MonsterBase monsterBase = null;

        public BasicMonsterWalk(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }

        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("�ȱ� ���¿� �����߽��ϴ�.");
            testNavi = monsterBase.navi;
            moveNodeListNum = 1;
            testNavi.PathFinding();
            monsterBase.animator.SetFloat("IsIdle", 1);
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("�ȱ� ���¿� ���� �߽��ϴ�.");
            monsterBase.animator.SetFloat("IsIdle", 0);
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

            moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
            moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
            DecreaseSpeed();
            //���� ���� ��ġ��� ���� ����Ʈ ���ڸ� ������Ʈ�Ѵ�.
            if (Mathf.Abs(monsterBase.transform.position.x - targetList.x) < 0.2f)
            {
                moveNodeListNum++;

                //���������� �����ϸ� �������� ����
                if (moveNodeListNum == testNavi.FinalNodeList.Count - 1) { adjustNum = 0; }
            }

            Jump();
            monsterBase.rb2d.linearVelocity = moveVir * monsterBase.monsterDB.MoveSpeed * adjustNum;
            PlayerRotation();
        }

        private void Jump()
        {
            if (testNavi.FinalNodeList[moveNodeListNum].nodeType == NodeType.Jump && testNavi.FinalNodeList[(moveNodeListNum == 1) ? 0 : moveNodeListNum - 1].nodeType == NodeType.Jump)
            {
                monsterBase.rb2d.AddForce(new Vector2(0, 0.5f) * 10); monsterBase.rb2d.gravityScale = 2;
            }
            else
            {
                monsterBase.rb2d.gravityScale = 3f;
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
        }
    }
}
