using UnityEngine;

public class StateMachine<T>
{
    private T tData;

    //현재 상태를 담는 프로퍼티
    public IState<T> CurState { get; set; }

    //기본 상태를 생성시에 설정하게 생성자 선언
    public StateMachine(T sender, IState<T> state)
    {
        tData = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        // null에러출력
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

        //상태 교체.
        CurState = state;

        //새 상태의 Enter를 호출한다.
        if (CurState != null)
            CurState.OperateEnter(tData);

        Debug.Log("SetNextState : " + state.GetType());

    }

    //State용 Update 함수.
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

public enum MonsterState { Idle = 0, Walk = 1, Attack = 2, Dead = 3 } //기본 상태, 걷기, 공격, 죽음