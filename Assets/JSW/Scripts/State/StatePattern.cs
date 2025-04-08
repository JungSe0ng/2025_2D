using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatePattern<T, K> : MonoBehaviour
{
    protected Dictionary<T, IState<K>> dicState = new Dictionary<T, IState<K>>();
    protected StateMachine<K> machine = null;

    //State�ʹ� ���� ����
    protected abstract void IStateStartSetting();

    //���� ������ �����ؼ� start�� �����Ѵ�.
    protected abstract IEnumerator CorutinePattern();

    //state�� �´� �������� �����Ѵ�.
    public abstract void StatePatttern(T state);

    //stateUpdate��
    protected abstract void UpdateSetting();
}