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
            monsterBase.animator.SetBool("IsAttack", true);
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("공격 상태에 종료했습니다.");
            monsterBase.animator.SetBool("IsAttack", false);
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

    public class BasicMonsterWalk : CharacterWalk, IState<MonsterBase>
    {
        public BasicMonsterWalk(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
            testNavi = monsterBase.GetComponent<TestNavi>();
        }

        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("걷기 상태에 진입했습니다.");
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

    }
}
