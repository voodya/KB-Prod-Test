using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/MonoEvent", fileName = "MonoEvent Installer")]
public class MonoEventHandlerInstaller : ScriptableInstaller
{
    public override void Install(IContainerBuilder container)
    {
        container.Register<MonoEventHandlerService>(Lifetime.Singleton)
            .AsImplementedInterfaces();
    }
}
