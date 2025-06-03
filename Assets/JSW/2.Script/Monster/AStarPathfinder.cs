using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using UnityEngine;

public class AstarPathfinder : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridRange = 100;
    [SerializeField] private float nodeRadius = 0.5f;
    [SerializeField] private float distanceBuffer = 0f;

    private AStarNode[,] grid;
    private List<AStarNode> path = new List<AStarNode>();
    private Vector2 bottomLeft;
    private float nodeDiameter;
    private Vector2 lastGridCenter; // 마지막으로 그리드를 생성한 중심 좌표
    private float regenThreshold = 5.0f; // 일정 거리 이상 이동하면 재생성
    private Vector3 subVector;
    private BaseMonster baseMonster = null;

    private void Start()
    {
        baseMonster = GetComponent<BaseMonster>();
        nodeDiameter = nodeRadius * 2f;
        GenerateGrid();
    }

    public void FindPathTarget(ref Vector3 targetPos, int correction = 0)
    {
        float dist = Vector2.Distance(transform.position, targetPos);
        if (dist < baseMonster.MonsterDB.StopDistance && correction == 0) return;

        // 🧠 플레이어가 이동해서 중심에서 멀어졌는지 확인
        if (Vector2.Distance(transform.position, lastGridCenter) > regenThreshold)
        {
            Debug.Log("좌표를 다시 만들겠습니다.");
            GenerateGrid();
            //해당 좌표가 이동 가능한 좌표인지?
            CheckSidePos(correction, ref targetPos);
            FindPath(targetPos);
        }
        if (path.Count == 0)
        {
            CheckSidePos(correction, ref targetPos);
            FindPath(targetPos);
        }
        FollowPath();
    }

    //만약 현재 위치 좌표에서 이동을 원하는 값이랑 같은 경우.. 위치를 변경하지 않는다.
    private void CheckSidePos(int correction, ref Vector3 targetPos)
    {
        //만약 현재 위치 좌표에서 이동을 원하는 값이랑 같은 경우.. 위치를 변경하지 않는다.
        //타겟 지점에서 변경된 지점 x좌표와 현재 좌표의 차이가 0.1f미만일 경우 실행되지 않게 변경한다.
        if (correction != 1 && correction != -1) return;
        if (Mathf.Abs(targetPos.x - transform.position.x) < 0.1f) return;
        subVector = targetPos;
        targetPos.x += correction * 1.1f;
        AStarNode node = GetClosestNode(targetPos);
        //벽이 아니면 변경된 위치로 변경 벽이면 다른 좌표 -> 그래도 벽이면 원래 좌표
        if (node.isWall)
        {
            Debug.LogError("해당 좌표가 벽입니다.");
            //반대 좌표로 이동한다.
            targetPos.x -= correction * 2.2f;


            node = GetClosestNode(targetPos);

            //그래도 벽이면? 원래 타겟으로 변경한다.
            if (node.isWall)
            {
                targetPos = subVector;
                Debug.Log("모든 좌표가 벽입니다. 몬스터 위치로 이동합니다." + targetPos);
            }

        }
        //Debug.Log(targetPos);
    }

    private void GenerateGrid()
    {
        nodeDiameter = nodeRadius * 2f;

        // 🧩 노드 격자 정렬된 중심으로 설정
        lastGridCenter = new Vector2(
            Mathf.Round(transform.position.x / nodeDiameter) * nodeDiameter,
            Mathf.Round(transform.position.y / nodeDiameter) * nodeDiameter
        );
        bottomLeft = lastGridCenter - new Vector2(gridRange, gridRange) * nodeDiameter;

        grid = new AStarNode[gridRange * 2 + 1, gridRange * 2 + 1];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Vector2 worldPoint = bottomLeft + new Vector2(x * nodeDiameter, y * nodeDiameter);
                bool isWall = Physics2D.OverlapCircle(worldPoint, 0.3f, 1 << 10);

                grid[x, y] = new AStarNode(worldPoint, x, y, isWall);
                grid[x, y].G = int.MaxValue;
            }
        }
        DebugPrintGridWallStates();
        //Debug.LogError("멈춰");
    }

    public void PrintGrid(AStarNode a)
    {
        Debug.Log(a.gridX + " " + a.gridY + " " + a.worldPos);
    }
    private void DebugPrintGridWallStates()
    {
        if (grid == null)
        {
            Debug.LogWarning("⚠️ 그리드가 아직 생성되지 않았습니다.");
            return;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== Grid Wall States ===");

        for (int y = grid.GetLength(1) - 1; y >= 0; y--) // 위에서 아래로 출력
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                AStarNode node = grid[x, y];
                sb.Append(node.isWall ? "[X]" : "[O]");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }


    private void FindPath(Vector3 target)
    {
        path.Clear();

        AStarNode startNode = GetClosestNode(transform.position);
        AStarNode endNode = GetClosestNode(target);
        AStarNode bestSoFarNode = startNode;

        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {

            AStarNode current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < current.F || openList[i].F == current.F && openList[i].H < current.H)
                    current = openList[i];
            }

            openList.Remove(current);
            closedSet.Add(current);

            // 가장 가까운 후보 계속 갱신
            if (current.H < bestSoFarNode.H)
            {
                bestSoFarNode = current;
            }

            if (current == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (AStarNode neighbor in GetNeighbors(current))
            {
                if (neighbor.isWall || closedSet.Contains(neighbor))
                    continue;

                int moveCost = current.G + GetDistance(current, neighbor);
                if (moveCost < neighbor.G || !openList.Contains(neighbor))
                {
                    neighbor.G = moveCost;
                    neighbor.H = GetDistance(neighbor, endNode);
                    neighbor.parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        // 도착하지 못했을 경우: bestSoFar까지의 경로라도 출력
        Debug.Log("❌ 경로를 완전히 찾지 못했지만, 가장 가까운 지점까지 경로를 표시합니다.");
        RetracePath(startNode, bestSoFarNode);
    }
    private void OnDrawGizmos()
    {
        if (path != null && path.Count > 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(path[i].worldPos, path[i + 1].worldPos);
            }
        }
    }
    private void RetracePath(AStarNode startNode, AStarNode endNode)
    {
        AStarNode current = endNode;
        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
    }

    private void FollowPath()
    {
        if (path.Count == 0) return;

        Vector2 targetPos = path[0].worldPos;
        Debug.Log(targetPos + "위치로 이동중");
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * baseMonster.MonsterDB.MoveSpeed);

        //해당 grid에 도착하면 멈춤
        if (Vector2.Distance(transform.position, targetPos) < 0.01f)
        {
            path.RemoveAt(0);
        }
    }

    private AStarNode GetClosestNode(Vector2 worldPos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - bottomLeft.x) / nodeDiameter), 0, grid.GetLength(0) - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - bottomLeft.y) / nodeDiameter), 0, grid.GetLength(1) - 1);
        return grid[x, y];
    }

    private IEnumerable<AStarNode> GetNeighbors(AStarNode node)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = node.gridX + dx;
                int checkY = node.gridY + dy;

                if (checkX >= 0 && checkX < grid.GetLength(0) &&
                    checkY >= 0 && checkY < grid.GetLength(1))
                {
                    AStarNode neighbor = grid[checkX, checkY];

                    // ✅ 1. 기본 벽 체크 (모든 방향 공통)
                    if (neighbor.isWall) continue;

                    // ✅ 2. 대각선일 경우 코너 통과 여부 확인
                    bool isDiagonal = dx != 0 && dy != 0;
                    if (isDiagonal)
                    {
                        AStarNode nodeH = grid[node.gridX + dx, node.gridY]; // 수평
                        AStarNode nodeV = grid[node.gridX, node.gridY + dy]; // 수직

                        if (nodeH.isWall || nodeV.isWall)
                            continue;
                    }

                    yield return neighbor;
                }
            }
        }
    }



    private int GetDistance(AStarNode a, AStarNode b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        return dx > dy ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
    }

    public class AStarNode
    {
        public Vector2 worldPos;
        public int gridX, gridY;
        public bool isWall;

        public int G = int.MaxValue;
        public int H;
        public int F => G + H;
        public AStarNode parent;

        public AStarNode(Vector2 worldPos, int x, int y, bool wall)
        {
            this.worldPos = worldPos;
            gridX = x;
            gridY = y;
            isWall = wall;
        }
    }
}