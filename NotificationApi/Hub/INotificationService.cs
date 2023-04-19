namespace NotificationApi.Hub
{
    public interface INotificationService
    {
        Task GetNotificaiton(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate);
    }
}
