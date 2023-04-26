using Customer.Data.Context;
using Customer.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace NotificationApi.HubService
{
    public class NotificationHub : Hub<INotificationService>
    {
        private readonly ApplicationDbContext _dbContext;
        public NotificationHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveUserConnection(string username)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection
            {
                ConnectionId = connectionId,
                Username = username
            };

            _dbContext.HubConnections.Add(hubConnection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SendNotificationToClient(string Heading, string Message, string UserEmail, string RedirectUrl, string CreatedDate, string username)
        {
            var hubConnections = _dbContext.HubConnections.Where(con => con.Username == username).ToList();
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(Heading, Message, UserEmail, RedirectUrl, CreatedDate, username);
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
