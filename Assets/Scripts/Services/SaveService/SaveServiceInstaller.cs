using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/SaveService", fileName = "SaveService Installer")]
public class SaveServiceInstaller : ScriptableInstaller
{
    [SerializeField] private string _saveKey;
    public override void Install(IContainerBuilder container)
    {
        container.Register<SaveService>(Lifetime.Singleton)
            .WithParameter("saveKey", _saveKey)
            .AsImplementedInterfaces();
    }
}
