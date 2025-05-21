using UnityEngine;
using System.Collections;
using System.Diagnostics;
namespace BaseMonsterState
{
    //기본 idle -> trace(정찰)-> 적발견 -> walk(타겟) -> attack(타겟) ->dead
    //위를 공격 못함 stopdistance는 x값만 생각하도록 변경경


    // Idle
    public class BaseMonsterIdle : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public BaseMonsterIdle(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter() { }
        public void OperateExit() { }
        public void OperateUpdate() { }
    }

    // Walk
    public class BaseMonsterWalk : IState<BaseMonster>
    {
        private BaseMonster baseMonster;
        private bool isStop = true;
        public BaseMonsterWalk(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter()
        {
            baseMonster.Agent.SetDestination(baseMonster.IsAttackMonster[0].transform.position);
            baseMonster.Agent.isStopped = false;
            baseMonster.Agent.stoppingDistance = baseMonster.MonsterDB.StopDistance;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);
            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));
            baseMonster.StartCoroutine(StopWalk());
        }

        public void OperateExit()
        {
            baseMonster.Agent.isStopped = true;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), false);
            isStop = false;
        }

        private IEnumerator StopWalk(){
            while(isStop){
                if(baseMonster.IsAttackMonster.Count<0)break;
                yield return new WaitForFixedUpdate();
            }
        }

        public void OperateUpdate() {
            
        }
    }

    // CoolTime (기본 구조)
    public class BaseMonsterCoolTime : IState<BaseMonster>
    {
        private BaseMonster baseMonster;
        public BaseMonsterCoolTime(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }
        public void OperateEnter() { }
        public void OperateExit() { }
        public void OperateUpdate() { }
    }

    public class BaseMonsterTrace : IState<BaseMonster>
    {
        private BaseMonster baseMonster = null;
        public float traceNums = 5f;               // 최대 정찰 거리
        public LayerMask obstacleLayer;            // 벽 체크용
        public float waitTime = 0.5f;              // 도착 후 대기 시간
        private bool movingRight = true;
        public float distanceThreshold = 0.1f;
        private IEnumerator traceCorutine = null;

        public BaseMonsterTrace(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
            traceCorutine = PatrolLoop();
        }

        public void OperateEnter()
        {      
            baseMonster.StartCoroutine(traceCorutine);
            baseMonster.Agent.isStopped = false;
            baseMonster.Agent.stoppingDistance = 0;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);
        }

        public void OperateExit()
        {
            baseMonster.StopCoroutine(traceCorutine);
            baseMonster.Agent.isStopped = true;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), false);
        }

        public void OperateUpdate() { }
        private IEnumerator PatrolLoop()
        {
            while (true)
            {
                // 1. 이동 방향 설정
                Vector2 dir = movingRight ? Vector2.right : Vector2.left;

                // 2. Raycast로 벽까지의 거리 측정
                RaycastHit2D hit = Physics2D.Raycast(baseMonster.transform.position, dir, traceNums, obstacleLayer);
                float moveDistance = hit.collider != null ? hit.distance : traceNums;

                // 3. 목적지 설정
                Vector2 destination = (Vector2)baseMonster.transform.position + dir * moveDistance;

                // 4. 이동 시작
                baseMonster.Agent.SetDestination(destination);

                // 5. 목적지 도달까지 대기
                while (baseMonster.Agent.pathPending || baseMonster.Agent.remainingDistance > distanceThreshold)
                {
                    yield return null;
                }

                // 6. 방향 반전
                movingRight = !movingRight;

                // 7. Sprite Flip 처리 (선택)
                if (baseMonster.SpriteRender_img.TryGetComponent(out SpriteRenderer sr))
                {
                    sr.flipX = !movingRight;
                    baseMonster.FlipXposChange(movingRight);
                }

                // 8. 대기 시간
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    // Attack
    public class BaseMonsterAttack : IState<BaseMonster>
    {
        private BaseMonster baseMonster = null;
        private bool isStop = true;
        public BaseMonsterAttack(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter()
        {
            baseMonster.Agent.isStopped = false;
            baseMonster.Agent.SetDestination(baseMonster.IsAttackMonster[0].transform.position);
            baseMonster.Agent.stoppingDistance = baseMonster.MonsterDB.StopDistance;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsAttack.ToString(), true);
            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));

        }

        public void OperateExit()
        {
            baseMonster.Agent.isStopped = true;
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsAttack.ToString(), false);
            isStop = false;
        }

        public void OperateUpdate() { }
    }

    // Dead
    public class BaseMonsterDead : IState<BaseMonster>
    {
        private BaseMonster baseMonster = null;

        public BaseMonsterDead(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public void OperateEnter() { }
        public void OperateExit() { }
        public void OperateUpdate() { }
    }
}