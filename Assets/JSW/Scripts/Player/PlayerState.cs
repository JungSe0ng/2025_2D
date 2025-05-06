using UnityEngine;

namespace PlayerState
{
    public class PlayerIdle : MonoBehaviour, IState<PlayerBase>
    {
        private PlayerBase playerBase;
        public PlayerIdle(PlayerBase playerBase)
        {
            this.playerBase = playerBase;
        }

        public void OperateEnter(PlayerBase sender)
        {
            Debug.Log("Idle상태로 전환되었습니다.");
        }

        public void OperateExit(PlayerBase sender)
        {
            Debug.Log("Idle상태로 전환합니다.");
        }

        public void OperateUpdate(PlayerBase sender)
        {
           
        }
    }
    public class PlayerAttack : MonoBehaviour, IState<PlayerBase>
    {
        private PlayerBase playerBase = null;
        private Animator animator = null;
        private Rigidbody2D rb2d = null;
        public PlayerAttack(PlayerBase playerBase)
        {
            this.playerBase = playerBase;
            animator = playerBase.gameObject.transform.GetChild(0).GetComponent<Animator>();
            rb2d = playerBase.GetComponent<Rigidbody2D>();
        }
        public void OperateEnter(PlayerBase sender)
        {
            Debug.Log("대기상태로 전환되었습니다.");
            animator.SetBool("IsAttack", true);
            rb2d.freezeRotation = true;
        }

        public void OperateExit(PlayerBase sender)
        {
            Debug.Log("공격 상태로 전환되었습니다.");
            animator.SetBool("IsAttack", false);
            rb2d.freezeRotation = false;
        }

        public void OperateUpdate(PlayerBase sender)
        {

        }
    }
    public class PlayerDead : MonoBehaviour, IState<PlayerBase>
    {
        private PlayerBase playerBase = null;

        public PlayerDead(PlayerBase playerBase)
        {
            this.playerBase = playerBase;
        }

        public void OperateEnter(PlayerBase sender)
        {

        }

        public void OperateExit(PlayerBase sender)
        {

        }

        public void OperateUpdate(PlayerBase sender)
        {

        }
    }

    public class PlayerWalk : CharacterWalk, IState<PlayerBase>
    {

        public PlayerWalk(PlayerBase playerBase)
        {
            ///this.playerBase = playerBase;
            testNavi = playerBase.GetComponent<TestNavi>();
            animator = playerBase.gameObject.transform.GetChild(0).GetComponent<Animator>();
            rb2d = playerBase.GetComponent<Rigidbody2D>();
        }

        public void OperateEnter(PlayerBase sender)
        {
            Debug.Log("걷기 상태로 전환되었습니다.");
            testNavi.PathFinding();
            animator.SetFloat("IsIdle", 1); 
        }

        public void OperateExit(PlayerBase sender)
        {
            Debug.Log("걷기 상태로 전환 합니다.");
            animator.SetFloat("IsIdle", 0);
        }

        public void OperateUpdate(PlayerBase sender)
        {
            MoveTarget();
        }

    }
}
