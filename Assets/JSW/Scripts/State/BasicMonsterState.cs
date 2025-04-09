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
            Debug.Log("Idle상태에 진입했습니다.");
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("Idle상태에 종료합니다.");
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
            Debug.Log("공격상태에 진입했습니다.");
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("공격 상태에 종료했습니다.");
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

        //움직일 방향
        private Vector2 moveVir = Vector2.zero;

        //목표물 위치값
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
            Debug.Log("걷기 상태에 진입했습니다.");
            testNavi = monsterBase.navi;
            moveNodeListNum = 1;
            testNavi.PathFinding();
            monsterBase.animator.SetFloat("IsIdle", 1);
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("걷기 상태에 종료 했습니다.");
            monsterBase.animator.SetFloat("IsIdle", 0);
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

            Vector2 currentNode = new Vector2(testNavi.FinalNodeList[moveNodeListNum - 1].x, testNavi.FinalNodeList[moveNodeListNum - 1].y);

            moveVir.x = testNavi.FinalNodeList[moveNodeListNum].x - testNavi.FinalNodeList[moveNodeListNum - 1].x;
            moveVir.y = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;
            DecreaseSpeed();
            //만약 같은 위치라면 최종 리스트 숫자를 업데이트한다.
            if (Mathf.Abs(monsterBase.transform.position.x - targetList.x) < 0.2f)
            {
                moveNodeListNum++;

                //도착지점에 도착하면 움직임을 멈춤
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
        private void DecreaseSpeed() //내려갈 때 감속
        {
            if (moveVir.y < 0) adjustNum = 0.5f;
            else if (moveVir.y >= 1) adjustNum = 1.1f;
            else adjustNum = 1f;
        }
        //추후 수정
        private void PlayerRotation() // 플레이어 회전값 보정
        {
            //앞으로 갈 방향
            int go = testNavi.FinalNodeList[moveNodeListNum].y - testNavi.FinalNodeList[moveNodeListNum - 1].y;

            //이전에 갔었던 방향
            int back = 0;
            if (moveNodeListNum <= 1) { back = 0; }
            else
            {
                back = testNavi.FinalNodeList[moveNodeListNum - 1].y - testNavi.FinalNodeList[moveNodeListNum - 2].y;
            }

            //이전노드에서 변경된 방향값이 상승이고 다음노드에서는 상승일 경우 각도를 중간값으로 변경한다.
            //앞으로 갈 경우 항상 0을 고정 
            //위로 갈경우 플레이어는 45도까지 회전이 가능하다.

            //이전노드 방향이 0이면서 다음 노드가 1일 경우 해당 각도는 25도로 변경한다.
            //이전노드 방향이 1이면서 다음 노드가 0일 경우 해당 각도는 25도로 변경한다.
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
