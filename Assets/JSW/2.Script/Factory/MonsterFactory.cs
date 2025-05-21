using UnityEngine;
using System.Collections.Generic;

public class MonsterFactory : FactoryBase<BaseMonster>
{
    //몬스터 객체들 저장
    [SerializeField] private BaseMonster[] monsters = null;

    // codeName을 key로 하는 몬스터 딕셔너리
    public Dictionary<int, BaseMonster> monsterDic = new Dictionary<int, BaseMonster>();

    private int maxMonsterNum = 50;
    //생산한 몬스터
    private BaseMonster baseMonster = null;

    private void Awake()
    {
        DictionarySetting();

       StartMonsterSetting();
    }
    private void DictionarySetting()
    {
        monsterDic.Clear();
        if (monsters != null)
        {
            foreach (BaseMonster monster in monsters)
            {
                if (monster != null)
                {
                    int codeName = monster.MonsterDB.MonsterCodeName;
                    if (!monsterDic.ContainsKey(codeName))
                    {
                        monsterDic.Add(codeName, monster);
                    }
                }
            }
        }
    }

    //몬스터를 생산을 담당한다. 자동 POOLIng에 넣어지지 않는다.
    public override BaseMonster TGetProduct(int codeName)
    {
        if (monsterDic.TryGetValue(codeName, out BaseMonster prefab))
        {
            //미리 생산한 몬스터들을 Pooling에서 가져와서 사용한다.
            baseMonster = Instantiate(prefab.gameObject).GetComponent<BaseMonster>();
            baseMonster.Initialize();
            return baseMonster;
        }
        return null;
    }
     public void StartMonsterSetting()// start시 50마리씩 미리 몬스터들을 생산한다.
    {
        foreach (BaseMonster monster in monsters)
        {
            if (monster == null) continue;
            // 모든 몬스터를 50개씩만 미리 생성해서 비활성화
            for (int i = 0; i < maxMonsterNum; i++)
            {
                baseMonster=TGetProduct(monster.MonsterDB.MonsterCodeName);
                MonsterPool.Instance.InputQue(monster.MonsterDB.MonsterCodeName, baseMonster);
            }
        }
        //PrintQue();
    }
}
