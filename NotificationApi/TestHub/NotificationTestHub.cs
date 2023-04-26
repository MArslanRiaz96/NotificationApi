using Customer.Data.Context;
using Customer.Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace NotificationApi.TestHub
{
    public class NotificationTestHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        public NotificationTestHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task GetNotificaiton(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate)
        {
            await Clients.All.SendAsync("GetNotificaiton", Heading, Message, UserEmail, RedirectUrl, CreatedDate);
        }
        public async Task SaveUserConnection(string username)
        {
            var connectionid = Context.ConnectionId;
            HubConnection hubconnection = new HubConnection
            {
                ConnectionId = connectionid,
                Username = username
            };

            _dbContext.HubConnections.Add(hubconnection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SendNotificationToClient(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate, string username)
        {
            var hubConnections = _dbContext.HubConnections.Where(con => con.Username == username).ToList();
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendAsync("ReceivedPersonalNotification",Heading, Message, UserEmail, RedirectUrl, CreatedDate, username);
            }

        }
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
