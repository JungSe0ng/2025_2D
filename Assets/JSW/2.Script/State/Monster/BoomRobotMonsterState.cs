using UnityEngine;
using System.Collections;
using NormalMonsterState;

namespace BoomRobotMonsterState
{
    // Idle
    public class BoomRobotMonsterIdle : NormalMonsterIdle
    {
        public BoomRobotMonsterIdle(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
        }

        public override void OperateExit() {
            base.OperateExit();
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }

    // Walk
    public class BoomRobotMonsterWalk : NormalMonsterWalk
    {
        public BoomRobotMonsterWalk(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
        }

        public override void OperateExit() {
            base.OperateExit();
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }

    // CoolTime
    public class BoomRobotMonsterCoolTime : NormalMonsterCoolTime
    {
        public BoomRobotMonsterCoolTime(BaseMonster baseMonster) : base(baseMonster) {}

        public new void OperateEnter() {
            base.OperateEnter();
        }

        public new void OperateExit() {
            base.OperateExit();
        }

        public new void OperateUpdate() {
            base.OperateUpdate();
        }
    }

    // Trace
    public class BoomRobotMonsterTrace : NormalMonsterTrace
    {
        public BoomRobotMonsterTrace(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
        }

        public override void OperateExit() {
            base.OperateExit();
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }

    // Attack
    public class BoomRobotMonsterAttack : NormalMonsterAttack
    {
        private float boomTime =5.0f;
        public bool isBoom = false;
        public BoomRobotMonsterAttack(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
            //countdown시작해서 boom
            isBoom = false;
            baseMonster.StartCoroutine(CorutineBoom());
        }

        public override void OperateExit() {
            base.OperateExit();
            isBoom = false;
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }

        private IEnumerator CorutineBoom(){
            float time =0;
            while(time <boomTime||!isBoom){
                time += 0.1f;
                yield return new WaitForSeconds(0.1f);    
                isBoom = true;
            }

            //터짐
            Boom();
        }
        private void Boom(){
            baseMonster.StatePatttern(MonsterState.Dead);
            Debug.Log("자폭 모드 작동");
        }
    }

    // Dead
    public class BoomRobotMonsterDead : NormalMonsterDead
    {
        public BoomRobotMonsterDead(BaseMonster baseMonster) : base(baseMonster) {}

        public override void OperateEnter() {
            base.OperateEnter();
            Debug.Log("자폭했습니다.");
        }

        public override void OperateExit() {
            base.OperateExit();
        }

        public override void OperateUpdate() {
            base.OperateUpdate();
        }
    }
}
