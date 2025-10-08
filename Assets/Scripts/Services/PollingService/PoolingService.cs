using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolingService
{
    void Clear<T>(T pfb) where T : MonoBehaviour;
    T GetObject<T>(T pfb) where T : MonoBehaviour;
    void Register<T>(T pfb) where T : MonoBehaviour;
    void ReleaseObject<T>(T obj, string name) where T : MonoBehaviour;
}


public class PoolingService : IPoolingService
{
    private Dictionary<Type, Queue<MonoBehaviour>> _poolingObjects;
    private Dictionary<string, Queue<MonoBehaviour>> _poolingObjectsStr;
    private Dictionary<Type, MonoBehaviour> _pfbs;
    private Dictionary<string, MonoBehaviour> _pfbsStr;
    private int _initialPoolSize;
    
    private IMonoEventHandlerService _monoProvider;

    public PoolingService(IMonoEventHandlerService monoEventHandlerService, int initialPoolSize) 
    {
        _monoProvider = monoEventHandlerService;
        _initialPoolSize = initialPoolSize;
        _pfbs = new Dictionary<Type, MonoBehaviour>();
        _pfbsStr = new();
        _poolingObjects = new();
        _poolingObjectsStr = new();
    }

    public void Register<T>(T pfb) where T : MonoBehaviour
    {
        string t = pfb.name;
        
        if (_pfbsStr.ContainsKey(t)) return;
        _pfbsStr[t] = pfb;
        _poolingObjectsStr[t] = new();
        for (int i = 0; i < _initialPoolSize; i++)
        {
            _poolingObjectsStr[t].Enqueue(_monoProvider.GetInstance(pfb));
        }
    }

    public T GetObject<T>(T pfb) where T : MonoBehaviour
    {
        string type = pfb.name;
        if (_poolingObjectsStr[type].Count > 0)
            return _poolingObjectsStr[type].Dequeue() as T;
        else
        {
            return _monoProvider.GetInstance(_pfbsStr[type]) as T;
        }
    }

    public void ReleaseObject<T>(T obj, string name) where T : MonoBehaviour
    {
        Type type = typeof(T);
        _poolingObjectsStr[name].Enqueue(obj);
    }

    public void Clear<T>(T pfb) where T : MonoBehaviour
    {
        string name = pfb.name;
        if (_poolingObjectsStr.ContainsKey(name)) return;
        _poolingObjectsStr[name] = new();
    }
}
