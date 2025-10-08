using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/App Metrica", fileName = "App Metrica Installer")]
public class AppMetricaInstaller : ScriptableInstaller
{
    [SerializeField] private string _apiKey;
    public override void Install(IContainerBuilder container)
    {
        container.Register<AppMetricaService>(Lifetime.Singleton)
            .WithParameter("apiKey", _apiKey)
            .AsImplementedInterfaces();
    }
}
