using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    //���� ���¸� ��� ������Ƽ
    public IState<T> CurState { get; set; }

    //�⺻ ���¸� �����ÿ� �����ϰ� ������ ����
    public StateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        // null�������
        if (m_sender == null)
        {
            Debug.LogError("m_sender ERROR");
            return;
        }

        if (CurState == state)
        {
            //Debug.LogWarningFormat("Same state : ", state);
            return;
        }

        if (CurState != null)
            CurState.OperateExit(m_sender);

        //���� ��ü.
        CurState = state;

        //�� ������ Enter�� ȣ���Ѵ�.
        if (CurState != null)
            CurState.OperateEnter(m_sender);

        Debug.Log("SetNextState : " + state.GetType());

    }

    //State�� Update �Լ�.
    public void DoOperateUpdate()
    {
        if (m_sender == null)
        {
            Debug.LogError("invalid m_sener");
            return;
        }
        CurState.OperateUpdate(m_sender);
    }
}
public enum MonsterState { Idle = 0, Walk = 1, Attack = 2, Dead = 3 } //�⺻ ����, �ȱ�, ����, ����