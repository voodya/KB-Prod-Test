using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/ScenesInstaller", fileName = "ScenesInstaller")]
public class ScenesServiceInstaller : ScriptableInstaller
{
    [SerializeField] private AssetReference _menu;
    [SerializeField] private AssetReference _settings;
    [SerializeField] private AssetReference _bg;
    [SerializeField] private AssetReference _gamePlay;
    [SerializeField] private AssetReference _gameUI;
    [SerializeField] private AssetReference _pause;
    [SerializeField] private AssetReference _delay;
    [SerializeField] private AssetReference _result;
    [SerializeField] private AssetReference _characterView;
    [SerializeField] private AssetReference _fg;
    public override void Install(IContainerBuilder container)
    {
        ScenesService service = new ScenesService();
        service.Register<MenuScene>(_menu);
        service.Register<SettingsScene>(_settings);
        service.Register<BGScene>(_bg);
        service.Register<GamePlayScene>(_gamePlay);
        service.Register<GameUIScene>(_gameUI);
        service.Register<PauseScene>(_pause);
        service.Register<DelayScene>(_delay);
        service.Register<ResultScene>(_result);
        service.Register<CharacterViewScene>(_characterView);
        service.Register<FGScene>(_fg);
        container.RegisterInstance(service).AsImplementedInterfaces();
    }
}
