using UnityEngine;
using VContainer;
[CreateAssetMenu(menuName = "SO/Installers/Notifications", fileName = "Notifications Installer")]
public class NotificationsInstaller : ScriptableInstaller
{
    [SerializeField] private string _title;
    [SerializeField] private string _message;
    [SerializeField] private int _delayInMitutes;
    public override void Install(IContainerBuilder container)
    {
        container.Register<NotificationsService>(Lifetime.Singleton)
            .WithParameter("title", _title)
            .WithParameter("message", _message)
            .WithParameter("delay", _delayInMitutes)
            .AsImplementedInterfaces();
    }

}
