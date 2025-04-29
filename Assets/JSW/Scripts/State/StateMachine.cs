using UnityEngine;

public class StateMachine<T>
{
    private T tData;

    //���� ���¸� ��� ������Ƽ
    public IState<T> CurState { get; set; }

    //�⺻ ���¸� �����ÿ� �����ϰ� ������ ����
    public StateMachine(T sender, IState<T> state)
    {
        tData = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        // null�������
        if (tData == null)
        {
            Debug.LogError("tData ERROR");
            return;
        }

        if (CurState == state)
        {
            //Debug.LogWarningFormat("Same state : ", state);
            return;
        }

        if (CurState != null)
            CurState.OperateExit(tData);

        //���� ��ü.
        CurState = state;

        //�� ������ Enter�� ȣ���Ѵ�.
        if (CurState != null)
            CurState.OperateEnter(tData);

        Debug.Log("SetNextState : " + state.GetType());

    }

    //State�� Update �Լ�.
    public void DoOperateUpdate()
    {
        if (tData == null)
        {
            Debug.LogError("invalid m_sener");
            return;
        }
        CurState.OperateUpdate(tData);
    }
}

public enum MonsterState { Idle = 0, Walk = 1, Attack = 2, Dead = 3 } //�⺻ ����, �ȱ�, ����, ����