using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEditor.Search;
public class MonsterPool : MonoBehaviour
{
    //�ִ� 6����
    public Dictionary<int, Queue<GameObject>> monsterDic = new Dictionary<int, Queue<GameObject>>();
    
    //Ȯ�ο� ���͵�
    [SerializeField]
    public MonsterBase[] monsters = new MonsterBase[10];

    //���⼭ �̸� ������ 0���� ������ ������ 
    private void Start()
    {
        //Test();
    }
    //que�� �ߵ����� Ȯ�ο�
    private void Test()
    {
        Debug.Log("start");
        for (int i = 0; i < monsters.Length; i++)
        {
            InputQue(monsters[i].monsterDB.MonsterCodeName, monsters[i].gameObject);
            Debug.Log(monsterDic.Count);
        }
        PrintQue();
    }


    public void InputQue(int monsterNum, GameObject monster)//���͸� que�� �ֱ�
    {
        
        //�ش� Ű���� ���� ���
        if(monsterDic.TryGetValue(monsterNum, out Queue<GameObject> value))
        {
            Debug.Log("�ش� ���Ͱ� �־ que�� �����߽��ϴ�.");
            value.Enqueue(monster);
        }

        //�ش� ���Ͱ� ���� ��� �ش� que�� ������
        if (!monsterDic.ContainsKey(monsterNum)) { MonsterInput(monsterNum, monster);};
    }
    //�ӽ� que�� �����ϰ� �ش� ���͸� ť�� ����
    private void MonsterInput(int monsterNum, GameObject monster)
    {
        Debug.Log("�ش� ť�� �����ϰ� ���͸� �־����ϴ�.");
        Queue<GameObject> que = new Queue<GameObject>();
        que.Enqueue(monster);
        monsterDic.Add(monsterNum, que);
    }

    public GameObject OutPutMonster(int monsterNum)//���� ����
    {
        //�ش� ���Ͱ� dictionary�� �����ϴ��� Ȯ��
        if (monsterDic.ContainsKey(monsterNum)) {

            //�ش� que�� �����ϴ��� Ȯ���ϰ� ������ �����ϰ� ������ �����޼��� �� ������
            if (monsterDic[monsterNum].Count > 1)
            {
                return monsterDic[monsterNum].Dequeue();
            }
            else
            {

                return null; 
            }
        };

        //�ƹ��͵� ���� ���
        Debug.LogWarning("������ ���Ͱ� ��� ������");

        return null;
    }
    private void PrintQue()
    {
        foreach (int v in monsterDic.Keys)
        {
            Debug.Log($"Ű���� {v}  ������ {monsterDic[v].Count}");
        }
    }
}
