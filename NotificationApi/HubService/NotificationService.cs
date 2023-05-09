using Customer.Data.Context;
using Customer.Data.Models;
using Customer.Manager.Notifications;
using Customer.Model.Common;
using Customer.Model.Notifications;
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
        public async Task<PagedResult<PushNotificationModel>> GetReadNotifications(string userEmail, string productId, int page, int pageSize = 10)
        {
            var response = await _notificationManager.GetReadNotifications(userEmail, productId, page, pageSize);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "")
        {
            var response = await _notificationManager.GetReadGroupNotifications(userEmail, productId, page, pageSize);
            await Clients.Group(groupId).GetNotificaiton(response.Results.ToList());
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task GetUnreadNotifications(string userEmail, string productId, string notificationId = "")
        {
            var response = await _notificationManager.GetUnreadNotifications(userEmail, productId, notificationId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response);
            }
        }

        public async Task GetUnreadGroupNotifications(string userEmail, string productId, string groupId = "", string notificationId = "")
        {
            var response = await _notificationManager.GetUnreadGroupNotifications(userEmail, productId, groupId, notificationId);
            await Clients.Group(groupId).GetNotificaiton(response);
        }
        public async Task MarkNotificationRead(string userEmail, string productId, string notificationId = "")
        {
            await _notificationManager.MarkNotificationRead(userEmail, productId, notificationId);
        }
        public async Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "")
        {
            await _notificationManager.MarkGroupNotificationRead(userEmail, productId, groupId, notificationId);
        }

        public async Task PushNotification(NotificationsModel model)
        {
            if (!string.IsNullOrEmpty(model.Heading) && !string.IsNullOrEmpty(model.Message) && !string.IsNullOrEmpty(model.UserEmail) && !string.IsNullOrEmpty(model.ProductId))
            {
                if (!model.IsSpecific)
                {
                    string notificationId = await _notificationManager.InsertNotification(model);

                    var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, CreatedOn = DateTime.UtcNow.ToString(), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId } };
                    await Clients.Group(model.ProductId).GetNotificaiton(notificationPush);
                }
                else
                {
                    var hubConnections = await _notificationManager.GetUserConnections(model.UserEmail, model.ProductId);
                    if (hubConnections?.Count() >= 1)
                    {
                        string notificationId = await _notificationManager.InsertNotification(model);
                        var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, CreatedOn = DateTime.UtcNow.ToString(), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId } };
                        await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(notificationPush);
                    }
                }
            }
        }
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = _dbContext.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if (hubConnection != null)
            {
                _dbContext.HubConnections.Remove(hubConnection);
                _dbContext.SaveChangesAsync();

                await RemoveFromGroup(hubConnection?.ProductId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
