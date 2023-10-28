using Customer.Model.Notifications;

namespace NotificationApi.HubService
{
    public interface INotificationService
    {
        Task GetNotificaiton(List<PushNotificationModel> pushNotificationModel);
        Task GetNotificaitonForChat(List<PushNotificationChatModel> pushNotificationChatModel);
        Task SendNotificationToClient(List<PushNotificationModel> pushNotificationModel);
        Task SendAsync(string OnConnected);
        Task SaveUserConnection(string Message);
    }
}
