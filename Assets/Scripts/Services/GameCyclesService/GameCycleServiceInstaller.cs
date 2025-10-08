using System.Collections.Generic;
using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/GameCycles", fileName = "GameCycles")]
public class GameCycleServiceInstaller : ScriptableInstaller
{
    [SerializeField] private Obstacle _obstaclePfb;
    [SerializeField] private RectTransform _roadRect;
    [SerializeField] private GameConfig _gameConfig;

    [SerializeField] private List<ObstacleConfig> _eat;
    [SerializeField] private List<ObstacleConfig> _fatal;
    [SerializeField] private List<ObstacleConfig> _default;

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
            .WithParameter("configs", new Dictionary<EObstacleType, List<ObstacleConfig>>() 
            {
                { EObstacleType.Eat, _eat },
                { EObstacleType.FatalObstacle, _fatal },
                { EObstacleType.DefaultObstacle, _default }
            })
            .AsImplementedInterfaces();

        container.Register<MainGameService>(Lifetime.Singleton)
            .WithParameter("gameConfig", _gameConfig)
            .AsImplementedInterfaces();
    }
}
