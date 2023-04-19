using Microsoft.AspNetCore.SignalR;

namespace NotificationApi.Hub
{
    public class NotificationHub : Hub<INotificationService>
    {
    }
}
