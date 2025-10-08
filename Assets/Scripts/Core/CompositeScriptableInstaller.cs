using System.Collections.Generic;
using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Composite Installer", fileName = "Composite Installer")]
public class CompositeScriptableInstaller : ScriptableInstaller
{
    [SerializeField] private List<ScriptableInstaller> _installers;
    public override void Install(IContainerBuilder container)
    {
        foreach (var installer in _installers)
        {
            installer.Install(container);
        }
    }
}
