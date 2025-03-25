using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class MonsterPool : Singleton<MonsterPool>
{
    //�ִ� 6����
    public Dictionary<int, Queue<GameObject>> monsterDic = new Dictionary<int, Queue<GameObject>>();

    //Ȯ�ο� ���͵�
    [SerializeField]
    public MonsterBase[] monsters = new MonsterBase[3];

    //�ʹݿ� �����ϴ� ���� ����
    private int maxMonsterNum = 50;

    //������ ��� �߰� �����ϴ� ���� ����
    private int plusMonsterNum = 10;

    //���⼭ �̸� ������ 0���� ������ ������ 
    private void Start()
    {
        StartMonsterSetting(monsters);
    }

    public void StartMonsterSetting(MonsterBase[] monsters)//start�� �ϸ� ���͵��� �ڵ� �����ϴ� �ڵ带 ����
    {
        GameObject obj = null;
        foreach (MonsterBase monster in monsters)
        {
            if (monster == null) continue;
            //���� ���͸� 50������ ���� �����ؼ� ��Ȱ��ȭ
            for (int i = 0; i < maxMonsterNum; i++)
            {
                obj = Instantiate(monster).gameObject;
                InputQue(monster.monsterDB.MonsterCodeName, obj);
            }
        }
        //PrintQue();
    }

    public void InputQue(int monsterNum, GameObject monster)//���͸� que�� �ֱ� ��� ���� �� ��Ȱ��ȭ�� ���Ѽ� �־����
    {
        //�ش� Ű���� ���� ���
        if (monsterDic.TryGetValue(monsterNum, out Queue<GameObject> value))
        {
            //Debug.Log("�ش� ���Ͱ� �־ que�� �����߽��ϴ�.");
            value.Enqueue(monster);
        }

        //�ش� ���Ͱ� ���� ��� �ش� que�� ������
        if (!monsterDic.ContainsKey(monsterNum)) { MonsterInput(monsterNum, monster); };

        ObjectReset(monster);
    }

    private void ObjectReset(GameObject obj)//��ü ���� ���� �ʱ�ȭ �� ��Ȱ��ȭ
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.parent = transform;
        obj.name = obj.GetComponent<MonsterBase>().monsterDB.MonsterName;
        obj.SetActive(false);
    }

    //�ӽ� que�� �����ϰ� �ش� ���͸� ť�� ����
    public void MonsterInput(int monsterNum, GameObject monster)
    {
        Debug.Log("�ش� ť�� �����ϰ� ���͸� �־����ϴ�.");
        Queue<GameObject> que = new Queue<GameObject>();
        que.Enqueue(monster);
        monsterDic.Add(monsterNum, que);
    }

    public GameObject OutPutMonster(int monsterNum)//���� ����
    {

        //�ش� ���Ͱ� dictionary�� �����ϴ��� Ȯ��
        if (monsterDic.ContainsKey(monsterNum))
        {
            GameObject obj = null;
            //�ش� que�� �����ϴ��� Ȯ���ϰ� ������ �����ϰ� ������ �����޼��� �� ������
            if (monsterDic[monsterNum].Count > 1)
            {
                obj = monsterDic[monsterNum].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                //���͸� �߰��� 10���� ���� �� ������ 1������ ��ȯ
                obj = monsterDic[monsterNum].Dequeue();
                GameObject instanceObj = null;
                for (int i = 0; i < plusMonsterNum; i++)
                {
                    instanceObj = Instantiate(obj);
                    InputQue(monsterNum, instanceObj);
                }
                obj.SetActive(true);
                return obj ;
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
