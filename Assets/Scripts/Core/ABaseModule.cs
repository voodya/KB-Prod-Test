using System;
using UniRx;

public interface IGameModule
{
    void OnStart(IMainGameService gameCycleService);
    void OnEnd();
    void OnUpdate();

    IObservable<Unit> OnEndCycle { get; }
}


public abstract class ABaseModule : IGameModule
{
    protected IMainGameService _gameCycleService;
    protected IRuntimeUserData _runtimeData;
    protected CompositeDisposable _compositeDisposable;

    private Subject<Unit> _onEnd = new();
    public IObservable<Unit> OnEndCycle => _onEnd;

    public ABaseModule(IRuntimeUserData runtimeUserData)
    {
        _runtimeData = runtimeUserData;
    }


    public virtual void OnStart(IMainGameService gameCycleService)
    {
        _gameCycleService = gameCycleService;
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();

    }
    public virtual void OnEnd()
    {
        _compositeDisposable?.Dispose();
    }
    public virtual void OnUpdate()
    {

    }
}
