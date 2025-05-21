using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatePattern<T, K> : MonoBehaviour
{
    protected Dictionary<T, IState<K>> dicState = new Dictionary<T, IState<K>>();
    protected StateMachine<K> machine = null;

    //State패턴 초기 설정
    protected abstract void IStateStartSetting();

    //상태 패턴을 실행하여 start에 넣는다.
    protected abstract IEnumerator CorutinePattern();

    //state에 맞는 상태로 전환한다.
    public abstract void StatePatttern(T state);

    //stateUpdate함
    protected abstract void UpdateSetting();
}