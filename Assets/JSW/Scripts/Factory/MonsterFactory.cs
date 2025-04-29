using UnityEngine;

public class MonsterFactory :  FactoryBase<MonsterBase>
{
    [SerializeField] private int monsterInstanceNum = 50;

    protected override void Start()
    {
        base.Start();
    }

    public override MonsterBase TGetProduct(int codeName, Vector3 pos)
    {
        MonsterBase obj = Instantiate(dic[codeName]);
        obj.Initialize();
        return obj;
    }

    protected override void DicSetting()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            Debug.Log(prefabs[i].codeName);
            Debug.Log(prefabs[i]);

            dic.Add(prefabs[i].codeName, prefabs[i]);
        }
    }

    public override void InstancePrefabs()//몬스터들을 미리 최대 개수만큼 생성하고  pool에 넣는다.
    {
        MonsterBase monster = null;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == null) continue;

            Debug.Log($"{prefabs[i].codeName}의 몬스터를 50마리 소환하게씁니다.");

            for (int j = 0; j < monsterInstanceNum; j++)
            {
                monster = TGetProduct(i, Vector3.zero);
                MonsterPool.Instance.InputQue(monster.MonsterDB.MonsterCodeName, monster.gameObject);
            }
        }
    }
}
