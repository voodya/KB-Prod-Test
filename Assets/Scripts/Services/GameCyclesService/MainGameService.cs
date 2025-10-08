using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public interface IMainGameService
{
    IObservable<bool> OnEndCycle { get; }
    GamePlayScene GamePlayScene { get; }
    GameConfig GameConfig { get; }

    void StartGameCycles();
}

public class MainGameService : IMainGameService
{
    //injected
    private IEnumerable<IGameModule> _modules;
    private IScenesService _scenesService;
    private IPlayerHolderService _playerHolderService;
    private IRuntimeUserData _runtimeUserData;
    private IMonoEventHandlerService _monoEventHandlerService;
    private ISaveService _saveService;
    private GameConfig _gameConfig;
    //local
    private GamePlayScene _gamePlayScene;
    private CompositeDisposable _compositeDisposable;
    private Subject<bool> _onEnd = new();


    
    
    public IObservable<bool> OnEndCycle  => _onEnd;
    public GamePlayScene GamePlayScene => _gamePlayScene;
    public GameConfig GameConfig => _gameConfig;

    public MainGameService(
        IEnumerable<IGameModule> gameCycles,
        IScenesService scenesService,
        IPlayerHolderService playerHolderService,
        IRuntimeUserData runtimeUserData,
        IMonoEventHandlerService monoEventHandlerService,
        GameConfig gameConfig,
        ISaveService saveService)
    {
        _modules = gameCycles;
        _scenesService = scenesService;
        _playerHolderService = playerHolderService;
        _runtimeUserData = runtimeUserData;
        _monoEventHandlerService = monoEventHandlerService;
        _gameConfig = gameConfig;
        _saveService = saveService;
    }

    public async void StartGameCycles()
    {
        foreach (var module in _modules)
        {
            module.OnEnd();
        }
        _runtimeUserData.Grid = new ReactiveProperty<bool[]>();
        _runtimeUserData.RoadRects = new();
        _runtimeUserData.RoadPositions = new();
        _runtimeUserData.CurrentSpeed = GameConfig.StratSpeed;
        _runtimeUserData.SetStartData(0, _gameConfig.StartLivesCount, 0); //todo
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
        
        Debug.Log($"ALERT_CREATE_DISPOSABLES_{_compositeDisposable.GetHashCode()}");
        _runtimeUserData.CurrentHealth.Subscribe(OnHit).AddTo(_compositeDisposable);

        await _scenesService.GetScene<GameUIScene>(false,scene =>
        {
            _saveService.TryGetData(nameof(UserData), out UserData data);
            _runtimeUserData.CurrentDistance.Subscribe(value => scene.SetDistance(value, data != null ? data.DistanceRecord : 0)).AddTo(scene.OnceSub);
            _runtimeUserData.CurrentEat.Subscribe(value => scene.SetEat(value)).AddTo(scene.OnceSub);
            _runtimeUserData.CurrentHealth.Subscribe(value => scene.SetHearts(value)).AddTo(scene.OnceSub);
            scene.OnPause.Subscribe(ShowPause).AddTo(scene.OnceSub);
        });

        await _scenesService.GetScene<GamePlayScene>(false, scene =>
        {
            _gamePlayScene = scene;

            foreach (var module in _modules)
            {
                module.OnStart(this);
            }
            _playerHolderService.SetPlayerToRect(scene.PlayerHolder, Vector2.zero);
        });

        

        _monoEventHandlerService.OnUpdate.Subscribe(_ =>
        {
            Debug.Log($"ALERT_CALL_UPDATE_{_compositeDisposable.GetHashCode()}");
            foreach (var module in _modules)
            {
                module.OnUpdate();
            }

        }).AddTo(_compositeDisposable);
    }

    private void ShowPause(Unit unit)
    {
        Time.timeScale = 0;
        _scenesService.GetScene<PauseScene>(false, scene => 
        {
            scene.OnReturn.Subscribe(_ => TimerBeforeContinue()).AddTo(scene.OnceSub);
            scene.OnMenu.Subscribe(_ => 
            {
                _scenesService.ReleaseScene<PauseScene>();
                Time.timeScale = 1;
                OnEnd(false);

            }).AddTo(scene.OnceSub);
        });
    }

    private void TimerBeforeContinue()
    {
        _scenesService.GetScene<DelayScene>(false, _ => { }, async beforeShow => 
        {
            beforeShow.DelayText.text = 3.ToString();
            await UniTask.Delay(1000, true);
            beforeShow.DelayText.text = 2.ToString();
            await UniTask.Delay(1000, true);
            beforeShow.DelayText.text = 1.ToString();
            await UniTask.Delay(1000, true);
            Time.timeScale = 1;
            _scenesService.ReleaseScene<DelayScene>();
        });
        _scenesService.ReleaseScene<PauseScene>();
    }

    private void OnHit(uint obj)
    {
        if (obj == 0)
        {
            Time.timeScale = 0;
            _scenesService.GetScene<ResultScene>(false,
            scene =>
            {
                scene.OnMenu.Subscribe(_ => 
                {
                    Time.timeScale = 1;
                    _scenesService.ReleaseScene<ResultScene>();
                    OnEnd(false);
                }).AddTo(scene.OnceSub);
                scene.OnReplay.Subscribe(_ =>
                {
                    Time.timeScale = 1;
                    _scenesService.ReleaseScene<ResultScene>();
                    OnEnd(true);
                }).AddTo(scene.OnceSub);
            },
            beforeShow =>
            {
                _saveService.TryGetData(nameof(UserData), out UserData data);
                beforeShow.SetData(
                    _runtimeUserData.CurrentDistance.Value,
                    data != null?data.DistanceRecord : 0,
                    _runtimeUserData.CurrentEat.Value,
                    data != null ? data.EatRecord : 0);
            });
            
        }
    }

    private void OnEnd(bool onReplay)
    {
        _onEnd?.OnNext(onReplay);
        _compositeDisposable?.Dispose();
        foreach (var item in _modules)
        {
            item.OnEnd();
        }

        
    }
}
