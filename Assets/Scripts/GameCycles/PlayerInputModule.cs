using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerInputModule : ABaseModule
{
    private IScenesService _scenesService;

    public PlayerInputModule(IRuntimeUserData runtimeUserData, IScenesService scenesService) : base(runtimeUserData)
    {
        _scenesService = scenesService;
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        _gameCycleService = gameCycleService;
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
        _scenesService.GetScene<GameUIScene>(false, OnShowed =>
        {
            OnShowed.OnDown.Subscribe(OnDown).AddTo(_compositeDisposable);
            OnShowed.OnUp.Subscribe(OnUp).AddTo(_compositeDisposable);
        });
        OnUp(Unit.Default);
        OnDown(Unit.Default);

    }

    private void OnUp(Unit unit)
    {
        if (_runtimeData.PlayerPosition.Value < _runtimeData.RoadPositions.Count - 1)
            _runtimeData.PlayerPosition.Value += 1;
    }
    private void OnDown(Unit unit)
    {
        if (_runtimeData.PlayerPosition.Value > 0)
            _runtimeData.PlayerPosition.Value -= 1;
    }
}
