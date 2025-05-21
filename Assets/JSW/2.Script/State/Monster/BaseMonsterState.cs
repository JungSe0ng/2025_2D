using UnityEngine;

namespace BaseMonsterState
{
    // Idle
    public class BaseMonsterIdle : MonoBehaviour, IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public BaseMonsterIdle(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter(BaseMonster sender)
        {

        }

        public void OperateExit(BaseMonster sender)
        {

        }

        public void OperateUpdate(BaseMonster sender)
        {
        }
    }

    // Walk
    public class BaseMonsterWalk : MonoBehaviour, IState<BaseMonster>
    {
        private float moveSpeed = 3.5f; // 베이스 몬스터의 이동 속도

        public BaseMonsterWalk(BaseMonster BaseMonster)
        {
        }

        public void OperateEnter(BaseMonster sender)
        {
        }

        public void OperateExit(BaseMonster sender)
        {
        }

        public void OperateUpdate(BaseMonster sender)
        {
        }
    }

    // Run
    public class BaseMonsterRun : MonoBehaviour, IState<BaseMonster>
    {
        private BaseMonster baseMonster;
        private float moveSpeed = 3.5f; // 베이스 몬스터의 이동 속도

        public BaseMonsterRun(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter(BaseMonster sender)
        {
            baseMonster.AgentTargetSetting();
            Debug.Log("적에게 이동중입니다.");
            baseMonster.MonsterAnimator.SetBool("IsRun", true);
        }

        public void OperateExit(BaseMonster sender)
        {
            baseMonster.MonsterAnimator.SetBool("IsRun", false);
            baseMonster.Agent.isStopped = true;
        }

        public void OperateUpdate(BaseMonster sender)
        {
        }
    }

    // CoolTime (기본 구조)
    public class BaseMonsterCoolTime : MonoBehaviour, IState<BaseMonster>
    {
        private BaseMonster baseMonster;
        private float coolTime = 2.0f; // 쿨타임 예시
        private float timer = 0f;
        public BaseMonsterCoolTime(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }
        public void OperateEnter(BaseMonster sender)
        {
            timer = 0f;
        }
        public void OperateExit(BaseMonster sender)
        {
        }
        public void OperateUpdate(BaseMonster sender)
        {
            timer += Time.deltaTime;
            if (timer >= coolTime)
            {
            }
        }
    }

    // Attack
    public class BaseMonsterAttack : MonoBehaviour, IState<BaseMonster>
    {
        private BaseMonster baseMonster = null;
        private float attackTimer = 0f;
        private float attackDelay = 1.5f; // 공격 딜레이

        public BaseMonsterAttack(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter(BaseMonster sender)
        {
            attackTimer = 0f;
            baseMonster.MonsterAnimator.SetBool("IsAttack", true);
        }

        public void OperateExit(BaseMonster sender)
        {
            baseMonster.MonsterAnimator.SetBool("IsAttack", false);
        }

        public void OperateUpdate(BaseMonster sender)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                Attack();
                attackTimer = 0f;
            }
        }

        private void Attack()
        {
        }
    }

    // Dead
    public class BaseMonsterDead : MonoBehaviour, IState<BaseMonster>
    {
        private BaseMonster baseMonster = null;

        public BaseMonsterDead(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter(BaseMonster sender)
        {
        }

        public void OperateExit(BaseMonster sender)
        {
        }

        public void OperateUpdate(BaseMonster sender)
        {
        }
    }
}