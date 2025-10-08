using Cysharp.Threading.Tasks;
using Unity.Notifications.Android;

public interface INotificationsService : IBootable
{
    void DelayedNotify(int seconds);
    void SendTimeDelayNotification();
}


public class NotificationsService : INotificationsService
{
    private string _title;
    private string _message;
    private int _timeDelayInMinutes;
    private const string ChannelID = "Default";

    public int Priority => 15;

    public NotificationsService(string title, string message, int delay)
    {
        _title = title;
        _message = message;
        _timeDelayInMinutes = delay;
    }

    public async UniTask Boot()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Name = "Default",
            Description = "Default app notifications",
            Importance = Importance.High,
            Id = ChannelID
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendTimeDelayNotification()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = _title,
            Text = _message,
            FireTime = System.DateTime.Now.AddMinutes(_timeDelayInMinutes),
            SmallIcon = "icon_1",
            LargeIcon = "icon_0"

        };
        AndroidNotificationCenter.SendNotification(notification, ChannelID);
    }

    public void DelayedNotify(int seconds)
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = _title,
            Text = _message,
            FireTime = System.DateTime.Now.AddSeconds(seconds),
            SmallIcon = "icon_1",
            LargeIcon = "icon_0"

        };

        AndroidNotificationCenter.SendNotification(notification, ChannelID);
    }
}
