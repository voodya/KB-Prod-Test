using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/GameCycles", fileName = "GameCycles")]
public class GameCycleServiceInstaller : ScriptableInstaller
{
    [SerializeField] private Obstacle _obstaclePfb;
    [SerializeField] private Obstacle _largeObstaclePfb;
    [SerializeField] private Obstacle _eatPfb;
    [SerializeField] private RectTransform _roadRect;
    [SerializeField] private GameConfig _gameConfig;
    public override void Install(IContainerBuilder container)
    {
        container.Register<PlayerInputModule>(Lifetime.Singleton).AsImplementedInterfaces();
        container.Register<PlayerEventModule>(Lifetime.Singleton).AsImplementedInterfaces();
        container.Register<CharacterMoveModule>(Lifetime.Singleton).AsImplementedInterfaces();
        container.Register<CalculateDistanceModule>(Lifetime.Singleton).AsImplementedInterfaces();

        container.Register<RoadsModule>(Lifetime.Singleton)
            .WithParameter("roadRect", _roadRect)
            .AsImplementedInterfaces();

        container.Register<GridGenModule>(Lifetime.Singleton)
            .WithParameter("obstacle", _obstaclePfb)
            .AsImplementedInterfaces();

        container.Register<ObjectsSpawnerModule>(Lifetime.Singleton)
            .WithParameter("obstacle", _obstaclePfb)
            .WithParameter("largeObstacle", _largeObstaclePfb)
            .WithParameter("eat", _eatPfb)
            .AsImplementedInterfaces();

        container.Register<MainGameService>(Lifetime.Singleton)
            .WithParameter("gameConfig", _gameConfig)
            .AsImplementedInterfaces();
    }
}
