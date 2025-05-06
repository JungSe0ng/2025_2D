using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(int x, int y, NodeType nodeType = NodeType.NotWalk) { this.nodeType = nodeType; this.x = x; this.y = y; }

    public Node ParentNode;
    public NodeType nodeType = NodeType.NotWalk;
    // G : 시작점으로부터 이동한 거리, H : |x|+|y| 맨하튼 거리로 목표지점까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public enum NodeType { NotWalk = 0, WalkAble = 1, Jump = 2 };

public class TestNavi : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;

    private int sizeX, sizeY;
    private Node[,] NodeArray;
    private Node StartNode, TargetNode, CurNode;
    private List<Node> OpenList, ClosedList;


    private void Start()
    {
        // PathFinding();
        startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }
    private void Awake()
    {

    }
    //astar알고리즘은 layer가 walkable이면 해당 타일로 이동이 가능하다.
    //false면 이동불가능한 타일이다
    public void PathFinding()
    {
        // NodeArray의 크기 설정하고, isWalkAble, x, y 저장
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                NodeArray[i, j] = new Node(i + bottomLeft.x, j + bottomLeft.y);
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (i == 5 && j == 0) Debug.Log(LayerMask.LayerToName(col.gameObject.layer));
                    for (int k = 0; k < Enum.GetValues(typeof(NodeType)).Length; k++)
                    {
                        if (LayerMask.LayerToName(col.gameObject.layer) == Enum.GetName(typeof(NodeType), k))
                        {
                            NodeArray[i, j].nodeType = (NodeType)k;
                            break;
                        }
                    }
                }
            }
        }
      //PrintNodeType();


        // 시작과 끝, 오픈리스트와 클로즈리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        //Node 초기화
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 오픈리스트에 있는 F를 비교 F가 같으면 H를 비교해서 작은것을 오픈리스트에 넣는다
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 종료
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                //for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                return;
            }


            // 대각선
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // 클로즈리스트에 있으면 제외하고, 대각선이 아니면서, 오픈리스트에 없다면
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);

        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 범위 체크 후, 대각선이 아니면서, 클로즈리스트에 없다면
        bool istrue = (checkX >= bottomLeft.x && checkX < topRight.x + 1) ? true : false;

        if (checkX >= bottomLeft.x &&
            checkX < topRight.x + 1 &&
            checkY >= bottomLeft.y &&
            checkY < topRight.y + 1 &&
            NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].nodeType != NodeType.NotWalk &&
            !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {

            // 대각선으로 가면, 대각선G를 더하고, 직선이면 대각선이 아니면 G, H, ParentNode를 설정하고 오픈리스트에 추가
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // 댵명에 대각선이 아니면 G를 더하고, 대각선이면 G를 더하고, 직선이면 G를 더하고, H를 설정하고, ParentNode를 설정하고, 오픈리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }

    private void PrintNodeType()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Debug.Log($"x : {i}    y: {j}     {NodeArray[i, j].nodeType.ToString()}");

            }
        }
    }
    public bool IsEnumDefined<T>(int value) where T : Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }
}
