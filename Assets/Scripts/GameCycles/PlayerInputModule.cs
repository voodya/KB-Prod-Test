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
            //_runtimeData.CurrentDistance.Subscribe(value => OnShowed.SetDistance(value, 0)).AddTo(OnShowed.OnceSub);
            //_runtimeData.CurrentEat.Subscribe(value => OnShowed.SetEat(value)).AddTo(OnShowed.OnceSub);
            //_runtimeData.CurrentHealth.Subscribe(value => OnShowed.SetHearts(value)).AddTo(OnShowed.OnceSub);
            //scene.OnPause.Subscribe(ShowPause).AddTo(scene.OnceSub);
        });

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
