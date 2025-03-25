using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    [SerializeField] private MonsterBase[] monsters = new MonsterBase[10];

    //�⺻ prefab���� �̸� �����ؼ� ������Ʈ Ǯ�� ����
    public void FactoryBasicInstance()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null) continue;
            monsters[i].GetComponent<IProduct>().Initialize();
            MonsterPool.Instance.MonsterInput(monsters[i].monsterDB.MonsterCodeName, monsters[i].gameObject);
        }
    }

    //codeName�θ��� �ش� ���Ͱ� ��ȯ�ǵ��� ���鰡?
}


