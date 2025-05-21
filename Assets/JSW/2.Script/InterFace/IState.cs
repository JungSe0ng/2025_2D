using UnityEngine;


public interface IState<T>
{
    void OperateEnter();
    void OperateUpdate();
    void OperateExit();
}

