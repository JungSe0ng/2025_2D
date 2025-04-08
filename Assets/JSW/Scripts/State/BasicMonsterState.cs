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

        }

        public void OperateExit(MonsterBase sender)
        {
        
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

        }

        public void OperateExit(MonsterBase sender)
        {

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

        private MonsterBase monsterBase = null;

        public BasicMonsterWalk(MonsterBase monsterBase)
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
}
