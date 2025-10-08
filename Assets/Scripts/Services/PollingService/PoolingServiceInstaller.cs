using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/PoolingInstaller", fileName = "PoolingService Installer")]
public class PoolingServiceInstaller : ScriptableInstaller
{
    [SerializeField] private int _initialSize;
    public override void Install(IContainerBuilder container)
    {
        container.Register<PoolingService>(Lifetime.Singleton)
            .WithParameter("initialPoolSize", _initialSize)
            .AsImplementedInterfaces();
    }
}
