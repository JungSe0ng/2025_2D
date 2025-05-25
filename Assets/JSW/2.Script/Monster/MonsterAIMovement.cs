using UnityEngine;
using System.Collections.Generic;

public class MonsterMovementController : MonoBehaviour
{
    [SerializeField] private float nodeRadius = 0.25f;
    [SerializeField] private Vector2 gridSize = new Vector2(10, 10);
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float stopDistance = 1.0f; // 거리 유지 변수

    private List<Vector2> path;
    private int targetIndex = 0;
    public Transform target;
    private AStarGrid grid;
    private MonsterAStarPathfinder pathfinder;

    private void Start()
    {
      //  target = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateGrid();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDistance)
        {
            GenerateGrid();
            path = pathfinder.FindPath(transform.position, target.position);
            if (path != null) StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }

    private void GenerateGrid()
    {
        grid = new AStarGrid((Vector2)transform.position, gridSize, nodeRadius, wallMask);
        pathfinder = new MonsterAStarPathfinder(grid);
    }

    private System.Collections.IEnumerator FollowPath()
    {
        if (path == null || path.Count == 0) yield break;

        targetIndex = 0;
        while (targetIndex < path.Count)
        {
            Vector2 targetPos = path[targetIndex];
            while ((Vector2)transform.position != targetPos)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            targetIndex++;
        }
    }
}
