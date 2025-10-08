using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/RuntimeData", fileName = "RuntimeData installer")]
public class RuntimeUserDataInstaller : ScriptableInstaller
{
    public override void Install(IContainerBuilder container)
    {
        container.Register<RuntimeUserDataService>(Lifetime.Singleton)
            .AsImplementedInterfaces();
    }
}
