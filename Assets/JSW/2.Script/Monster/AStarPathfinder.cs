using System.Collections.Generic;
using UnityEngine;

public class MonsterAStarPathfinder
{
    private AStarGrid grid;

    public MonsterAStarPathfinder(AStarGrid grid)
    {
        this.grid = grid;
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new();
        HashSet<Node> closedSet = new();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in GetNeighbours(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newMovementCost = current.gCost + GetDistance(current, neighbor);
                if (newMovementCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
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
            path.Add(current.worldPosition);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = node.gridX + dx;
                int checkY = node.gridY + dy;

                if (checkX >= 0 && checkX < grid.GetGrid().GetLength(0) &&
                    checkY >= 0 && checkY < grid.GetGrid().GetLength(1))
                {
                    neighbours.Add(grid.GetGrid()[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    private int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        return (dstX > dstY) ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
    }
}


public class AStarGrid
{
    private Node[,] grid;
    private int gridSizeX, gridSizeY;
    private float nodeRadius;
    private LayerMask unwalkableMask;
    private Vector2 gridWorldSize;
    private Vector2 origin;

    public AStarGrid(Vector2 origin, Vector2 size, float nodeRadius, LayerMask unwalkableMask)
    {
        this.origin = origin;
        this.gridWorldSize = size;
        this.nodeRadius = nodeRadius;
        this.unwalkableMask = unwalkableMask;
        this.gridSizeX = Mathf.RoundToInt(size.x / (nodeRadius * 2));
        this.gridSizeY = Mathf.RoundToInt(size.y / (nodeRadius * 2));

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 bottomLeft = origin - new Vector2(gridWorldSize.x / 2, gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = bottomLeft + new Vector2(x * nodeRadius * 2 + nodeRadius, y * nodeRadius * 2 + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                if(walkable == false)Debug.Log(x+" "+y+" 가 벽");
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x - (origin.x - gridWorldSize.x / 2)) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y - (origin.y - gridWorldSize.y / 2)) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Node[,] GetGrid() => grid;
    public int GetMaxSize() => gridSizeX * gridSizeY;
}


public class Node
{
    public Vector2 worldPosition;
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost => gCost + hCost;

    public Node(bool walkable, Vector2 worldPosition, int x, int y)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = x;
        this.gridY = y;
    }
}
