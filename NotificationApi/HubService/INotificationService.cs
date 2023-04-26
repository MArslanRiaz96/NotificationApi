namespace NotificationApi.HubService
{
    public interface INotificationService
    {
        Task GetNotificaiton(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate);
        Task SendNotificationToClient(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate, string username);
    }
}
