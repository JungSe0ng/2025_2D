using UnityEngine;
public class MonsterBase : MonoBehaviour
{
    //���� ���ݹ��� ����
    [SerializeField]
    private CircleCollider2D attackArea = null;

    //���� ��ũ���ͺ� ������Ʈ ����
    [SerializeField]
    public MonsterScriptableObjects monsterDB = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�浹��");
    }
    private void Awake()
    {
        MonsterAwakeSetting();
    }
    private void MonsterAwakeSetting()//���� �ʱ⼼��
    {
        //���� ���� ���� ����
        attackArea.radius = monsterDB.IsAttackArea;
    }

    public virtual void Attack()
    {

    }
    public virtual void Move()
    {

    }
    //���� �⺻ ����
    //���� �̵� ���
}
