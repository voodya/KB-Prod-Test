using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class BootstrapEntryPoint : ABaseEntryPoint
{
    private IScenesService _scenesService;
    private IPlayerHolderService _playerHolderService;
    private IMainGameService _gameCycleService;
    private ISaveService _saveService;
    private IRuntimeUserData _runtimeUserData;
    private INotificationsService _notificationsService;
    private IMonoEventHandlerService _monoEvent;
    private IAppMetricaService _appMetricaService;
    private IParallaxService _parallaxService;
    private UserData _userData;
    private SettingsData _settingsData;
    private uint _playId = 0;

    public BootstrapEntryPoint(
        IEnumerable<IBootable> bootables,
        IScenesService scenesService,
        IPlayerHolderService playerHolderService,
        IMainGameService gameCycleService,
        ISaveService saveService,
        IRuntimeUserData runtimeUserData,
        INotificationsService notificationsService,
        IMonoEventHandlerService monoEvent,
        IAppMetricaService appMetricaService,
        IParallaxService parallaxService) : base(bootables)
    {
        _scenesService = scenesService;
        _playerHolderService = playerHolderService;
        _gameCycleService = gameCycleService;
        _saveService = saveService;
        _runtimeUserData = runtimeUserData;
        _notificationsService = notificationsService;
        _monoEvent = monoEvent;
        _appMetricaService = appMetricaService;
        _parallaxService = parallaxService;
    }

    public override async UniTask StartAsync(CancellationToken cancellation = default)
    {
        await base.StartAsync(cancellation);
        Application.targetFrameRate = 120;

        if (!_saveService.TryGetData(nameof(UserData), out _userData))
        {
            _userData = new();
        }
        if (!_saveService.TryGetData(nameof(SettingsData), out _settingsData))
        {
            _settingsData = new();
            _settingsData.IsMusic = true;
            _settingsData.IsSound = true;
        }
        await _scenesService.GetScene<BGScene>(false, _ => { }, scene => 
        {
            _parallaxService.RegisterLayer(scene.BackLayer, 0);
            _parallaxService.RegisterLayer(scene.MiddleLayer, 1);
            _parallaxService.RegisterLayer(scene.FrontLayer, 2);
        }  );
        _monoEvent.OnQuit.Subscribe(_ => _notificationsService.DelayedNotify(10));

        _gameCycleService.OnEndCycle.Subscribe(async _ =>
        {

            if (_runtimeUserData.CurrentDistance.Value > _userData.DistanceRecord)
            {
                _userData.DistanceRecord = (uint)_runtimeUserData.CurrentDistance.Value;
            }
            if (_runtimeUserData.CurrentEat.Value > _userData.EatRecord)
            {
                _userData.EatRecord = _runtimeUserData.CurrentEat.Value;
            }
            _saveService.SetData(nameof(UserData), _userData);
            _appMetricaService?.EndGame((uint)_runtimeUserData.CurrentDistance.Value, _runtimeUserData.CurrentEat.Value);

            if (_)
            {
                await _scenesService.GetScene<CharacterViewScene>(false, _ => { }, 
                    beforeShow => 
                    {
                        _playerHolderService.SetPlayerToRect(beforeShow.CharacterRect, Vector2.zero);
                    });
                
                await _scenesService.ReleaseScene<GamePlayScene>();
                _scenesService.ReleaseScene<GameUIScene>();
                PlayGame();
            }
            else
            {
                LoadMainMenu();
                _scenesService.ReleaseScene<GamePlayScene>();
                _scenesService.ReleaseScene<GameUIScene>();
            }
            
        }).AddTo(Disposable);
        _parallaxService.PlayParallax();
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        _scenesService.GetScene<MenuScene>(false,
        onLoadScene =>
        {
            onLoadScene.OnPlay.Subscribe(_ => PlayGame()).AddTo(onLoadScene.OnceSub);
            onLoadScene.OnSettings.Subscribe(_ => OpenSettings()).AddTo(onLoadScene.OnceSub);
            onLoadScene.OnTestNoitifi.Subscribe(_ => _notificationsService.DelayedNotify(1)).AddTo(onLoadScene.OnceSub);
        },
        beforeLoadScene =>
        {
            _playerHolderService.SetPlayerToRect(beforeLoadScene.PlayerHolder, Vector2.zero);
            beforeLoadScene.ShowUserData(_userData);
        });
    }

    private async void OpenSettings()
    {
        await _scenesService.GetScene<SettingsScene>(false, 
        scene =>
        {
            scene.OnMusic.Subscribe(_ =>
            {
                _settingsData.IsMusic = !_settingsData.IsMusic;
                _saveService.SetData(nameof(SettingsData), _settingsData);
                scene.SetData(_settingsData);
            }).AddTo(scene.OnceSub);
            scene.OnSound.Subscribe(_ =>
            {
                _settingsData.IsSound = !_settingsData.IsSound;
                _saveService.SetData(nameof(SettingsData), _settingsData);
                scene.SetData(_settingsData);
            }).AddTo(scene.OnceSub);
            scene.OnReturn.Subscribe(_ => scene.Hide()).AddTo(scene.OnceSub);
        },
        beforeScene => 
        {
            beforeScene.SetData(_settingsData);
        });

    }

    private async void PlayGame()
    {
        _playId++;
        _appMetricaService?.LaunchGame(_playId);
        var scene = await _scenesService.GetScene<GamePlayScene>(false,
            scene => _playerHolderService.SetPlayerToRect(scene.PlayerHolder, Vector2.zero));
        _gameCycleService.StartGameCycles();
        _scenesService.ReleaseScene<MenuScene>();
        _scenesService.GetScene<FGScene>(false, _ => { }, scene =>
        {
            _parallaxService.RegisterLayer(scene.ExtraFrontLayer, 3);
        });


    }
}
