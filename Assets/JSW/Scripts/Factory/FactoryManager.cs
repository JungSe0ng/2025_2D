using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    [SerializeField] private MonsterBase[] monsters = new MonsterBase[10];

    [SerializeField] private int monsterInstanceNum = 50;

    private MonsterBase monster = null;

    public void FactoryMonsterBasicInstance()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null) continue;

            Debug.Log($"{monsters[i].codeName}의 몬스터를 50마리 소환하게씁니다.");
            
            for (int j = 0; j < monsterInstanceNum; j++)
            {
                
                monster = Instantiate(monsters[i].gameObject).GetComponent<MonsterBase>();
                MonsterPool.Instance.InputQue(monster.MonsterDB.MonsterCodeName, monster.gameObject);
            }
        }
    }
}


