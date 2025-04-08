using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatePattern<T, K> : MonoBehaviour
{
    protected Dictionary<T, IState<K>> dicState = new Dictionary<T, IState<K>>();
    protected StateMachine<K> machine = null;

    //State초반 설정 세팅
    protected abstract void IStateStartSetting();

    //각자 패턴을 정의해서 start시 실행한다.
    protected abstract IEnumerator CorutinePattern();

    //state에 맞는 패턴으로 변경한다.
    public abstract void StatePatttern(T state);

    //stateUpdate문
    protected abstract void UpdateSetting();
}