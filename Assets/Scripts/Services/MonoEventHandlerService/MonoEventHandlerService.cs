using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

public interface IMonoEventHandlerService : IBootable
{
    IObservable<Unit> OnQuit { get; }
    IObservable<Unit> OnUpdate { get; }
    T GetInstance<T>(T pfb) where T : MonoBehaviour;
}


public class MonoEventHandlerService : IMonoEventHandlerService
{
    private MonoProvider _provider;
    private CompositeDisposable _compositeDisposable;
    private Subject<Unit> _onQuit = new();
    private Subject<Unit> _onUpdate = new();
    public int Priority => 20;

    public IObservable<Unit> OnQuit => _onQuit;
    public IObservable<Unit> OnUpdate => _onUpdate;

    public async UniTask Boot()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
        _provider = new GameObject("MonoProvider").AddComponent<MonoProvider>();
        _provider.OnQuit.Subscribe(_ => _onQuit?.OnNext(_)).AddTo(_compositeDisposable);
        _provider.OnUpdate.Subscribe(_ => _onUpdate?.OnNext(_)).AddTo(_compositeDisposable);
    }

    public T GetInstance<T>(T pfb) where T : MonoBehaviour
    {
        return _provider.GetInstance(pfb);
    }
}
