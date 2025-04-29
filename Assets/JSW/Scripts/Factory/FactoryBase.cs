using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class FactoryBase<T> : Singleton<FactoryBase<T>>
{
    [SerializeField] protected T[] prefabs = null;

    [SerializeField] protected int prefabsCount = 0;

    protected Dictionary<Int32, T> dic =new Dictionary<Int32, T>();

    protected virtual void Start()
    {
        DicSetting();
      
    }
    protected abstract void DicSetting(); //Ditionary 초기 값 세팅
    public abstract T TGetProduct(int codeName, Vector3 pos); //제품을 생성 후 가져옴

    public abstract void InstancePrefabs();

}

