using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(int x, int y, NodeType nodeType = NodeType.NotWalk) { this.nodeType = nodeType; this.x = x; this.y = y; }

    public Node ParentNode;
    public NodeType nodeType = NodeType.NotWalk;
    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
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

    public void PathFinding()
    {
        // NodeArray�� ũ�� �����ְ�, isWalkAble, x, y ����
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        //��� ����
        NodeTypeSetting();

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        //Node ����
        InstanceNode();
    }

    private void InstanceNode()
    {
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();
        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            // ������
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
                return;
            }
            // �֢آע�
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }
            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }
    private void NodeTypeSetting()
    {
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
    }
    private void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        bool istrue = (checkX >= bottomLeft.x && checkX < topRight.x + 1) ? true : false;

        if (checkX >= bottomLeft.x &&
            checkX < topRight.x + 1 &&
            checkY >= bottomLeft.y &&
            checkY < topRight.y + 1 &&
            NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].nodeType != NodeType.NotWalk &&
            !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {

            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
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
}
