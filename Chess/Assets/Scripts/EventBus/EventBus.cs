using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{

    public static EventBus instance {  get; private set; }

    public Dictionary<Type, IEvent> broadCasters = new Dictionary<Type, IEvent>();
    public Dictionary<Type, IEvent> lastBroadCastersData = new Dictionary<Type, IEvent>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }


    public void AddBroadCaster<T>(Event<T> broadCaster)
    {
        Type type = typeof(T);
        if (broadCasters.ContainsKey(type))
        {
            return;
        }
        broadCasters[type] = broadCaster;
        lastBroadCastersData[type] = null;
    }
    

    public void Subscribe<T>(Action<T> subscriber) where T : IEvent
    {
        Type type = typeof(T);
        if (!broadCasters.ContainsKey(type)) 
        { 
            AddBroadCaster(new Event<T>());    
        }
        Event<T> _event = (Event<T>)broadCasters[type];
        _event.AddListener(subscriber);
        if(lastBroadCastersData[type] != null)
        {
            subscriber.Invoke(lastBroadCastersData[type] as T);
        }
    }

    public void Invoke<T>(T data) where T : IEvent
    {
        Type type = typeof(T);
        Event<T> _event = (Event<T>)broadCasters[type];
        _event.Invoke(data);
        lastBroadCastersData[type] = data;
        Debug.Log(lastBroadCastersData[type]);
    }
}
