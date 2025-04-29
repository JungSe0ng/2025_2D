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

    public override void InstancePrefabs()//���͵��� �̸� �ִ� ������ŭ �����ϰ�  pool�� �ִ´�.
    {
        MonsterBase monster = null;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == null) continue;

            Debug.Log($"{prefabs[i].codeName}�� ���͸� 50���� ��ȯ�ϰԾ��ϴ�.");

            for (int j = 0; j < monsterInstanceNum; j++)
            {
                monster = TGetProduct(i, Vector3.zero);
                MonsterPool.Instance.InputQue(monster.MonsterDB.MonsterCodeName, monster.gameObject);
            }
        }
    }
}
