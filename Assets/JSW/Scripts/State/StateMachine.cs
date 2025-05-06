using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    //상태 관리를 위한 클래스
    public IState<T> CurState { get; set; }

    //기본 상태를 설정하고 상태를 관리
    public StateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        // null체크
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

        //상태 객체.
        CurState = state;

        //이 상태의 Enter를 호출한다.
        if (CurState != null)
            CurState.OperateEnter(m_sender);

        Debug.Log("SetNextState : " + state.GetType());

    }

    //State의 Update 함수.
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