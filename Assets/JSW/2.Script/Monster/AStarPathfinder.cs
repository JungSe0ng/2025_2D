using System.Collections.Generic;
using UnityEngine;

public class AStar2DPathfinder :MonoBehaviour
{
    private int width, height;
    private float nodeRadius = 0.4f;
    private LayerMask obstacleMask;
    private Node[,] grid;
    private Vector2 originWorldPos;

    public AStar2DPathfinder(int width, int height, LayerMask obstacleMask)
    {
        this.width = width;
        this.height = height;
        this.obstacleMask = obstacleMask;
    }

    // Node 클래스 정의
    public class Node
    {
        public bool walkable;
        public Vector2Int gridPos;
        public Vector2 worldPos;
        public int gCost, hCost;
        public int fCost => gCost + hCost;
        public Node parent;

        public Node(bool walkable, Vector2Int gridPos, Vector2 worldPos)
        {
            this.walkable = walkable;
            this.gridPos = gridPos;
            this.worldPos = worldPos;
        }
    }

    // 목적지 Vector2 좌표를 받아 이동 경로 반환
    public List<Vector2> GetPathToTarget(Vector2 startWorldPos, Vector2 targetWorldPos)
    {
        GenerateGrid(startWorldPos);
        Node startNode = WorldToNode(startWorldPos);
        Node targetNode = WorldToNode(targetWorldPos);
        return FindPath(startNode, targetNode);
    }

    // 플레이어 Transform을 받아 이동 경로 반환
    public List<Vector2> GetPathToPlayer(Vector2 startWorldPos, Transform playerTransform)
    {
        return GetPathToTarget(startWorldPos, playerTransform.position);
    }

    // 그리드 생성 (몬스터 기준으로 중심 생성)
    private void GenerateGrid(Vector2 center)
    {
        grid = new Node[width, height];
        originWorldPos = center - new Vector2(width / 2, height / 2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = originWorldPos + new Vector2(x + 0.5f, y + 0.5f);
                bool walkable = !Physics2D.OverlapCircle(worldPos, nodeRadius, obstacleMask);
                grid[x, y] = new Node(walkable, new Vector2Int(x, y), worldPos);
            }
        }
    }

    // A* 경로 탐색 (성능 최적화용 간단 힙 구조 PriorityQueue 대체)
    private List<Vector2> FindPath(Node start, Node target)
    {
        var open = new SimplePriorityQueue<Node>();
        var closed = new HashSet<Node>();

        start.gCost = 0;
        start.hCost = GetDistance(start, target);
        open.Enqueue(start, start.fCost);

        while (open.Count > 0)
        {
            Node current = open.Dequeue();
            closed.Add(current);

            if (current == target)
                return RetracePath(start, target);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || closed.Contains(neighbor)) continue;

                int newCost = current.gCost + GetDistance(current, neighbor);
                if (newCost < neighbor.gCost || !open.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, target);
                    neighbor.parent = current;

                    if (!open.Contains(neighbor))
                        open.Enqueue(neighbor, neighbor.fCost);
                }
            }
        }

        return null;
    }

    private List<Vector2> RetracePath(Node start, Node end)
    {
        List<Vector2> path = new();
        Node current = end;
        while (current != start)
        {
            path.Add(current.worldPos);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private Node WorldToNode(Vector2 worldPos)
    {
        Vector2 relative = worldPos - originWorldPos;
        int x = Mathf.Clamp(Mathf.FloorToInt(relative.x), 0, width - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(relative.y), 0, height - 1);
        return grid[x, y];
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new();
        Vector2Int[] dirs = {
            new(0, 1), new(1, 0), new(0, -1), new(-1, 0), // 직선
            new(1, 1), new(-1, 1), new(1, -1), new(-1, -1) // 대각선
        };

        foreach (var d in dirs)
        {
            int nx = node.gridPos.x + d.x;
            int ny = node.gridPos.y + d.y;
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                // 코너 회피 (대각선일 경우 양옆이 막혀있으면 제외)
                if (Mathf.Abs(d.x) + Mathf.Abs(d.y) == 2)
                {
                    if (!grid[node.gridPos.x + d.x, node.gridPos.y].walkable ||
                        !grid[node.gridPos.x, node.gridPos.y + d.y].walkable)
                        continue;
                }
                neighbors.Add(grid[nx, ny]);
            }
        }
        return neighbors;
    }

    private int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridPos.x - b.gridPos.x);
        int dy = Mathf.Abs(a.gridPos.y - b.gridPos.y);
        return dx > dy ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
    }
}

// 간단한 최소 힙 기반 PriorityQueue 구현 (성능용)
public class SimplePriorityQueue<T>
{
    private List<(T item, int priority)> heap = new();
    public int Count => heap.Count;

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        int c = heap.Count - 1;
        while (c > 0)
        {
            int p = (c - 1) / 2;
            if (heap[c].priority >= heap[p].priority) break;
            (heap[c], heap[p]) = (heap[p], heap[c]);
            c = p;
        }
    }

    public T Dequeue()
    {
        int li = heap.Count - 1;
        (heap[0], heap[li]) = (heap[li], heap[0]);
        T ret = heap[li].item;
        heap.RemoveAt(li);
        li--;
        int p = 0;
        while (true)
        {
            int c = p * 2 + 1;
            if (c > li) break;
            int rc = c + 1;
            if (rc <= li && heap[rc].priority < heap[c].priority) c = rc;
            if (heap[p].priority <= heap[c].priority) break;
            (heap[p], heap[c]) = (heap[c], heap[p]);
            p = c;
        }
        return ret;
    }

    public bool Contains(T item)
    {
        return heap.Exists(e => EqualityComparer<T>.Default.Equals(e.item, item));
    }
}
