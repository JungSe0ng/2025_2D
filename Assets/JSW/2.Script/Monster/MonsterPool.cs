using System;
using System.Collections.Generic;
using UnityEngine;
public class MonsterPool : Singleton<MonsterPool>
{
    // 최대 6종류
    public Dictionary<int, Queue<BaseMonster>> monsterDic = new Dictionary<int, Queue<BaseMonster>>();

    // 풀에 생성하는 몬스터 개수
    private int maxMonsterNum = 50;

    // 부족할 경우 추가 생성하는 몬스터 개수
    private int plusMonsterNum = 50;

    // 여기서 미리 생성해 0번부터 몬스터를 생성함
    void Awake()
    {
        
    }

    public void InputQue(int monsterNum, BaseMonster monster)// 몬스터를 que에 넣기 위해 먼저 풀 비활성화로 넣어두기
    {
        // 해당 키값이 있을 경우
        if (monsterDic.TryGetValue(monsterNum, out Queue<BaseMonster> value))
        {
            //Debug.Log("해당 몬스터가 있어 que에 추가했습니다.");
            value.Enqueue(monster);
        }

        // 해당 몬스터가 없을 경우 해당 que를 생성함
        if (!monsterDic.ContainsKey(monsterNum)) { MonsterInput(monsterNum, monster); };

        ObjectReset(monster);
    }

    private void ObjectReset(BaseMonster obj)// 오브젝트 위치 등을 초기화 후 비활성화
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.parent = transform;
        obj.name = obj.MonsterDB.MonsterName;
        obj.gameObject.SetActive(false);
    }

    // 임시 que를 생성하고 해당 몬스터를 큐에 추가
    public void MonsterInput(int monsterNum, BaseMonster monster)
    {
        Debug.Log("해당 큐를 생성하고 몬스터를 넣어두었습니다.");
        Queue<BaseMonster> que = new Queue<BaseMonster>();
        que.Enqueue(monster);
        monsterDic.Add(monsterNum, que);
    }

    public BaseMonster OutPutMonster(int monsterNum, Transform parent)// 몬스터 출력
    {
        // 해당 몬스터가 dictionary에 존재하는지 확인
        if (monsterDic.ContainsKey(monsterNum))
        {
            BaseMonster obj = null;
            // 해당 que가 존재하는지 확인하고 있으면 꺼내고 없으면 생성메세지 후 꺼내기
            if (monsterDic[monsterNum].Count > 1)
            {
                obj = monsterDic[monsterNum].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                // 몬스터를 추가로 10개씩 생성 후 마지막 1개만을 반환
                obj = monsterDic[monsterNum].Dequeue();
                BaseMonster instanceObj = null;

                //추가 생산
                for (int i = 0; i < plusMonsterNum; i++)
                {
                    instanceObj =  MonsterFactory.Instance.TGetProduct(monsterNum);
                    InputQue(monsterNum, instanceObj);
                }
                obj.gameObject.SetActive(true);
                obj.transform.parent = parent;
                return obj;
            }
        };

        // 아무것도 없을 경우
        Debug.LogWarning("요청한 몬스터가 전부 사용중");

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
