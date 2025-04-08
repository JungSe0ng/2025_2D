using UnityEngine;

namespace ZombiState
{
    public class ZombieIdleState : MonoBehaviour, IState<MonsterBase>
    {
        private MonsterBase monsterBase;

        public ZombieIdleState(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }

        public void OperateEnter(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }

        public void OperateExit(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }

        public void OperateUpdate(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ZombieAttackState : MonoBehaviour, IState<MonsterBase>
    {
        private MonsterBase monsterBase;

        public ZombieAttackState(MonsterBase monsterBase)
        {
            this.monsterBase = monsterBase;
        }

        public void OperateEnter(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }

        public void OperateExit(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }

        public void OperateUpdate(MonsterBase sender)
        {
            throw new System.NotImplementedException();
        }
    }
}
