using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    [SerializeField] private MonsterBase[] monsters = new MonsterBase[10];

    //기본 prefab들을 미리 생성해서 오브젝트 풀로 보냄
    public void FactoryBasicInstance()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null) continue;
            monsters[i].GetComponent<IProduct>().Initialize();
            MonsterPool.Instance.MonsterInput(monsters[i].monsterDB.MonsterCodeName, monsters[i].gameObject);
        }
    }

    //codeName부르면 해당 몬스터가 소환되도록 만들가?
}


