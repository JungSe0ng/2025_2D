using UnityEngine;

public abstract class FactoryBase<T> : Singleton<FactoryBase<T>>
{
    public abstract T TGetProduct(int codeName);
}

