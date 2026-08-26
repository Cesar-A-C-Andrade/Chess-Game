using System;
using UnityEngine;

public class Event<T> : IEvent
{
    private event Action<T> listeners;

    public void AddListener(Action<T> listener)
    {
        listeners += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        listeners -= listener;
    }

    public void Invoke(T data)
    {
        listeners?.Invoke(data);
    }
}
