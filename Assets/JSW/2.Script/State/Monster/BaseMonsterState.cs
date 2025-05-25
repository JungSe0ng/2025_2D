using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;
namespace NormalMonsterState
{
    //기본 idle -> trace(정찰)-> 적발견 -> walk(타겟) -> attack(타겟) ->dead
    //위를 공격 못함 stopdistance는 x값만 생각하도록 변경경


    // Idle
    public class NormalMonsterIdle : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public NormalMonsterIdle(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public virtual void OperateEnter() { }
        public virtual void OperateExit() { }
        public virtual void OperateUpdate() { }
    }

    // Walk
    public class NormalMonsterWalk : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        private bool isStop = true;
        public NormalMonsterWalk(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public virtual void OperateEnter()
        {
            //이동 목적지 설정
            //baseMonster.APath.PathFind(baseMonster.IsAttackMonster[0].transform.position);

            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);
            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));
            baseMonster.StartCoroutine(StopWalk());
        }

        public virtual void OperateExit()
        {
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), false);
            isStop = false;
        }

        private IEnumerator StopWalk()
        {
            while (isStop)
            {
                if (baseMonster.IsAttackMonster.Count < 0) break;
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual void OperateUpdate()
        {

        }
    }

    // CoolTime (기본 구조)
    public class NormalMonsterCoolTime : IState<BaseMonster>
    {
        protected BaseMonster baseMonster;
        public NormalMonsterCoolTime(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }
        public void OperateEnter() { }
        public void OperateExit() { }
        public void OperateUpdate() { }
    }

    public class NormalMonsterTrace : IState<BaseMonster>
    {
        protected BaseMonster baseMonster = null;
        public float traceNums = 5f;               // 최대 정찰 거리
        public LayerMask obstacleLayer;            // 벽 체크용
        public float waitTime = 0.5f;              // 도착 후 대기 시간
        private bool movingRight = true;
        public float distanceThreshold = 0.5f;
        private IEnumerator traceCorutine = null;
        private Vector2 destination = Vector2.zero;
        public NormalMonsterTrace(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
            traceCorutine = PatrolLoop();
            obstacleLayer = LayerMask.NameToLayer("ObstacleLayer");
        }

        public virtual void OperateEnter()
        {
            baseMonster.StartCoroutine(traceCorutine);
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), true);
        }

        public virtual void OperateExit()
        {
            baseMonster.StopCoroutine(traceCorutine);
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsWalk.ToString(), false);
        }

        public virtual void OperateUpdate()
        {
            if (destination == null) return;
            //Debug.Log("이동중");
           // baseMonster.APath.PathFind((Vector3)destination);
        }
        private IEnumerator PatrolLoop()
        {
            while (true)
            {
                // 1. 이동 방향 설정
                Vector2 dir = movingRight ? Vector2.right : Vector2.left;

                // 2. Raycast로 벽까지의 거리 측정
                RaycastHit2D hit = Physics2D.Raycast(baseMonster.transform.position, dir, int.MaxValue, obstacleLayer);
                float moveDistance = hit.collider != null ? hit.distance : traceNums;
                // 3. 목적지 설정
                destination = (Vector2)baseMonster.transform.position + dir * moveDistance;
                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                }else{
                    Debug.Log("인식되는 벽이 없어서 그냥 5이동");
                    Debug.Log(destination);
                }

                //목적지로 이동
                // 5. 목적지 도달까지 대기
                while (Vector3.Distance(baseMonster.transform.position , destination) > distanceThreshold)
                {      
                    yield return null;
                }
                //Debug.Log(baseMonster.APath.targetDis+" " +destination+"목적지 입니다.");

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
    public class NormalMonsterAttack : IState<BaseMonster>
    {
        protected BaseMonster baseMonster = null;
        protected bool isStop = true;
        public NormalMonsterAttack(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public virtual void OperateEnter()
        {
            //타겟 지점으로 이동


            isStop = true;
            baseMonster.StartCoroutine(baseMonster.CorutineVir(isStop));

            //override를 진행해서 아래에서 재정의해서 사용이 필요함 공격 애니메이션 타이밍이 다름
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsAttack.ToString(), true);
        }

        public virtual void OperateExit()
        {
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsAttack.ToString(), false);
            isStop = false;
        }

        public virtual void OperateUpdate()
        {
            if (baseMonster.IsAttackMonster.Count <= 0) return;
           // baseMonster.APath.PathFind(baseMonster.IsAttackMonster[0].transform.position);
        }


    }

    // Dead
    public class NormalMonsterDead : IState<BaseMonster>
    {
        protected BaseMonster baseMonster = null;

        public NormalMonsterDead(BaseMonster baseMonster)
        {
            this.baseMonster = baseMonster;
        }

        public virtual void OperateEnter()
        {
            baseMonster.MonsterAnimator.SetBool(NormalMonsterAnim.IsDead.ToString(), true);
            //
        }
        public virtual void OperateExit()
        {
            //비활성화 모드로 돌아감..

        }
        public virtual void OperateUpdate() { }
    }
}