using System.Collections.Generic;
using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/ParallaxService", fileName = "ParallaxService Installer")]
public class ParallaxServiceInstaller : ScriptableInstaller
{
    [SerializeField] private List<float> _speeds;
    public override void Install(IContainerBuilder container)
    {
        container.Register<ParallaxService>(Lifetime.Singleton)
            .WithParameter("speeds", _speeds)
            .AsImplementedInterfaces();
    }
}
