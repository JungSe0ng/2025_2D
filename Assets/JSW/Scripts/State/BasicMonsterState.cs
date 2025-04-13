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
        private Animator animator = null;
        private Rigidbody2D rb2d = null;
        public BasicMonsterAttack(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
            animator = monsterBase.gameObject.transform.GetChild(0).GetComponent<Animator>();
            rb2d = monsterBase.GetComponent<Rigidbody2D>();
        }
        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("���ݻ��¿� �����߽��ϴ�.");
            animator.SetBool("IsAttack", true);
            rb2d.freezeRotation = true;
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("���� ���¿� �����߽��ϴ�.");
            animator.SetBool("IsAttack", false);
            rb2d.freezeRotation = false;
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
            animator = monsterBase.gameObject.transform.GetChild(0).GetComponent<Animator>();
            rb2d = monsterBase.GetComponent<Rigidbody2D>();
        }

        public void OperateEnter(MonsterBase sender)
        {
            Debug.Log("�ȱ� ���¿� �����߽��ϴ�.");
            testNavi.PathFinding();
            animator.SetFloat("IsIdle", 1); 
        }

        public void OperateExit(MonsterBase sender)
        {
            Debug.Log("�ȱ� ���¿� ���� �߽��ϴ�.");
            animator.SetFloat("IsIdle", 0);
        }

        public void OperateUpdate(MonsterBase sender)
        {
            MoveTarget();
        }

    }
}
