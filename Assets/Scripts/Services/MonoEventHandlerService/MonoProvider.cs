using System;
using UniRx;
using UnityEngine;

public class MonoProvider : MonoBehaviour
{
    private Subject<Unit> _onQuit = new();
    private Subject<Unit> _onUpdate = new();
    public IObservable<Unit> OnQuit => _onQuit;
    public IObservable<Unit> OnUpdate => _onUpdate;

    private void OnApplicationQuit()
    {
        _onQuit?.OnNext(Unit.Default);
    }

    private void Update()
    {
        _onUpdate?.OnNext(Unit.Default);
    }

    public T GetInstance<T>(T pfb) where T : MonoBehaviour
    {
        return Instantiate(pfb);
    }
}
