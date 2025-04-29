using UnityEngine;

public interface IState<T>
{
    void OperateEnter(T data);
    void OperateUpdate(T data);
    void OperateExit(T data);
}

