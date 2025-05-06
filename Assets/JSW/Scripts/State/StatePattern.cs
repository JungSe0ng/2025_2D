using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatePattern<T, K> : MonoBehaviour
{
    protected Dictionary<T, IState<K>> dicState = new Dictionary<T, IState<K>>();
    protected StateMachine<K> machine = null;

    //State패턴 기본 구조
    protected abstract void IStateStartSetting();

    //상태를 전환하면서 start를 호출한다.
    protected abstract IEnumerator CorutinePattern();

    //state에 맞는 동작을 실행한다.
    public abstract void StatePatttern(T state);

    //stateUpdate
    protected abstract void UpdateSetting();
}