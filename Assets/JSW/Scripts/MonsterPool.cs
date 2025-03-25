using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class MonsterPool : Singleton<MonsterPool>
{
    //최대 6마리
    public Dictionary<int, Queue<GameObject>> monsterDic = new Dictionary<int, Queue<GameObject>>();

    //확인용 몬스터들
    [SerializeField]
    public MonsterBase[] monsters = new MonsterBase[3];

    //초반에 생성하는 몬스터 숫자
    private int maxMonsterNum = 50;

    //부족할 경우 추가 생성하는 몬스터 숫자
    private int plusMonsterNum = 10;

    //여기서 미리 생성함 0보다 작으면 생성함 
    private void Start()
    {
        StartMonsterSetting(monsters);
    }

    public void StartMonsterSetting(MonsterBase[] monsters)//start를 하면 몬스터들을 자동 생성하는 코드를 실행
    {
        GameObject obj = null;
        foreach (MonsterBase monster in monsters)
        {
            if (monster == null) continue;
            //각자 몬스터를 50마리씩 먼저 생성해서 비활성화
            for (int i = 0; i < maxMonsterNum; i++)
            {
                obj = Instantiate(monster).gameObject;
                InputQue(monster.monsterDB.MonsterCodeName, obj);
            }
        }
        //PrintQue();
    }

    public void InputQue(int monsterNum, GameObject monster)//몬스터를 que에 넣기 대신 넣을 때 비활성화를 시켜서 넣어야함
    {
        //해당 키값이 있을 경우
        if (monsterDic.TryGetValue(monsterNum, out Queue<GameObject> value))
        {
            //Debug.Log("해당 몬스터가 있어서 que에 삽입했습니다.");
            value.Enqueue(monster);
        }

        //해당 몬스터가 없을 경우 해당 que를 생성함
        if (!monsterDic.ContainsKey(monsterNum)) { MonsterInput(monsterNum, monster); };

        ObjectReset(monster);
    }

    private void ObjectReset(GameObject obj)//물체 값들 전부 초기화 후 비활성화
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.parent = transform;
        obj.name = obj.GetComponent<MonsterBase>().monsterDB.MonsterName;
        obj.SetActive(false);
    }

    //임시 que를 생성하고 해당 몬스터를 큐에 삽입
    public void MonsterInput(int monsterNum, GameObject monster)
    {
        Debug.Log("해당 큐를 생성하고 몬스터를 넣었습니다.");
        Queue<GameObject> que = new Queue<GameObject>();
        que.Enqueue(monster);
        monsterDic.Add(monsterNum, que);
    }

    public GameObject OutPutMonster(int monsterNum)//몬스터 반출
    {

        //해당 몬스터가 dictionary에 존재하는지 확인
        if (monsterDic.ContainsKey(monsterNum))
        {
            GameObject obj = null;
            //해당 que에 존재하는지 확인하고 있으면 반출하고 없으면 에러메세지 후 생성함
            if (monsterDic[monsterNum].Count > 1)
            {
                obj = monsterDic[monsterNum].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                //몬스터를 추가로 10마리 생성 후 생성한 1마리를 반환
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
