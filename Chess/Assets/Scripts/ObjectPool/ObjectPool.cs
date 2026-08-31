using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    Stack<T> currentPool = new Stack<T>();
    Stack<T> inUsePool = new Stack<T>();
    T poolObject;
    Func<T, T> duplicationFunc;


    public ObjectPool(T poolObjcet, Func<T, T> duplicationFunc) 
    {
        this.poolObject = poolObjcet;
        this.duplicationFunc = duplicationFunc;
    }

    public void SetPool(int quantity = 10)
    {
        for (int i = 0; i < quantity; i++)
        {
            currentPool.Push(duplicationFunc.Invoke(poolObject));
        }
    }

    public T GetOjbectFromPool(Action<T> action = null)
    {
        if (currentPool.Count == 0)
        {
            SetPool(1);
        }
        T _item = currentPool.Pop();
        inUsePool.Push(_item);
        action?.Invoke(_item);
        return _item;
    }

    public void ReturnAllToPool(Action<T> action = null)
    {
        while (inUsePool.Count > 0)
        {
            T _item = inUsePool.Pop();
            currentPool.Push(_item);
            action?.Invoke(_item);
        }
    }

}
