using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Bootstrap : LifetimeScope
{
    [SerializeField] private CompositeScriptableInstaller _installer;
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        _installer.Install(builder);
        builder.RegisterEntryPoint<BootstrapEntryPoint>();
    }
}
