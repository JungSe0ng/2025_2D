using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEditor.Search;
public class MonsterPool : MonoBehaviour
{
    //최대 6마리
    public Dictionary<int, Queue<GameObject>> monsterDic = new Dictionary<int, Queue<GameObject>>();
    
    //확인용 몬스터들
    [SerializeField]
    public MonsterBase[] monsters = new MonsterBase[10];

    //여기서 미리 생성함 0보다 작으면 생성함 
    private void Start()
    {
        //Test();
    }
    //que에 잘들어가는지 확인용
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


    public void InputQue(int monsterNum, GameObject monster)//몬스터를 que에 넣기
    {
        
        //해당 키값이 있을 경우
        if(monsterDic.TryGetValue(monsterNum, out Queue<GameObject> value))
        {
            Debug.Log("해당 몬스터가 있어서 que에 삽입했습니다.");
            value.Enqueue(monster);
        }

        //해당 몬스터가 없을 경우 해당 que를 생성함
        if (!monsterDic.ContainsKey(monsterNum)) { MonsterInput(monsterNum, monster);};
    }
    //임시 que를 생성하고 해당 몬스터를 큐에 삽입
    private void MonsterInput(int monsterNum, GameObject monster)
    {
        Debug.Log("해당 큐를 생성하고 몬스터를 넣었습니다.");
        Queue<GameObject> que = new Queue<GameObject>();
        que.Enqueue(monster);
        monsterDic.Add(monsterNum, que);
    }

    public GameObject OutPutMonster(int monsterNum)//몬스터 반출
    {
        //해당 몬스터가 dictionary에 존재하는지 확인
        if (monsterDic.ContainsKey(monsterNum)) {

            //해당 que에 존재하는지 확인하고 있으면 반출하고 없으면 에러메세지 후 생성함
            if (monsterDic[monsterNum].Count > 1)
            {
                return monsterDic[monsterNum].Dequeue();
            }
            else
            {

                return null; 
            }
        };

        //아무것도 없을 경우
        Debug.LogWarning("반출할 몬스터가 없어서 생성함");

        return null;
    }
    private void PrintQue()
    {
        foreach (int v in monsterDic.Keys)
        {
            Debug.Log($"키값은 {v}  개수는 {monsterDic[v].Count}");
        }
    }
}
