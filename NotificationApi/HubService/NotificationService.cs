using Customer.Data.Context;
using Customer.Data.Models;
using Customer.Manager.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace NotificationApi.HubService
{
    public class NotificationHub : Hub<INotificationService>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INotificationManager _notificationManager;
        public NotificationHub(ApplicationDbContext dbContext, INotificationManager notificationManager)
        {
            _dbContext = dbContext;
            _notificationManager = notificationManager;
        }
        public async Task SaveUserConnection(string userName, string productId)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var connectionid = Context.ConnectionId;
                HubConnection hubconnection = new HubConnection
                {
                    ConnectionId = connectionid,
                    Username = userName,
                    ProductId = productId,
                    CreatedOn = DateTime.UtcNow
                };

                hubconnection.Id = Guid.NewGuid().ToString();
                _dbContext.HubConnections.Add(hubconnection);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<Notification>> GetNotifications(string userEmail)
        {
            var response = await _notificationManager.GetNotifications(userEmail);
            return response;
        }

        public async Task GetUnreadNotifications(string userEmail, string notificationId = "")
        {
            var response = await _notificationManager.GetUnreadNotifications(userEmail, notificationId);
            //Clients.All.GetNotificaiton(model.Heading, model.Message, model.UserEmail, model.RedirectUrl, DateTime.UtcNow.ToString(), notificationId, false)
        }
        public async Task MarkNotificationRead(string userEmail, string notificationId = "")
        {
             await _notificationManager.MarkNotificationRead(userEmail, notificationId);
        }

        //public async Task SendNotificationToClient(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate, string username)
        //{
        //    var hubConnections = _dbContext.HubConnections.Where(con => con.Username == username).ToList();
        //    if (hubConnections?.Count() >= 1)
        //    {
        //        await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(Heading, Message, UserEmail, RedirectUrl, CreatedDate, username);
        //    }

        //}
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = _dbContext.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if (hubConnection != null)
            {
                _dbContext.HubConnections.Remove(hubConnection);
                _dbContext.SaveChangesAsync();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
