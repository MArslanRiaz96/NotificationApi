namespace NotificationApi.HubService
{
    public interface INotificationService
    {
        Task GetNotificaiton(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate,string NotificationId = "", bool IsRead = false);
        Task SendNotificationToClient(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate, string NotificationId = "", bool IsRead = false, string UserName = "");
        Task SendAsync(string OnConnected);
        Task SaveUserConnection(string Message);
    }
}
