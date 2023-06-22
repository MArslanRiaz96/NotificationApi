using Customer.Data.Context;
using Customer.Data.Models;
using Customer.Manager.Notifications;
using Customer.Model.Common;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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
        public async Task SaveUserConnection(string userName, string productId, string tenantId = "", string environmentId = "", string companyId = "")
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
                    TenantId = tenantId,
                    EnvironmentId = environmentId,
                    CompanyId = companyId == null ? "" : companyId,
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
        public async Task<PagedResult<PushNotificationModel>> GetReadNotifications(string UserEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetReadNotifications(UserEmail, productId, notificationId, page, pageSize, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(UserEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetAllNotifications(string userEmail, string productId, int page, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetAllNotifications(userEmail, productId, page, pageSize, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetReadGroupNotifications(userEmail, productId, page, pageSize, groupId, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }
        public async Task<PagedResult<PushNotificationModel>> GetAllTypeNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetAllTypeNotifications(userEmail, productId, page, pageSize, groupId, tenantId, environmentId, companyId);
            response.Results = response.Results.OrderByDescending(x => x.CreatedOn).ToList();
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetAllGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetAllGroupNotifications(userEmail, productId, page, pageSize, groupId, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetUnreadNotifications(string userEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetUnreadNotifications(userEmail, productId, notificationId, page, pageSize, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }

        public async Task<PagedResult<PushNotificationModel>> GetUnreadGroupNotifications(string userEmail, string productId, string groupId = "", string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            var response = await _notificationManager.GetUnreadGroupNotifications(userEmail, productId, groupId, notificationId, page, pageSize, tenantId, environmentId, companyId);
            var hubConnections = await _notificationManager.GetUserConnections(userEmail, productId, tenantId, environmentId, companyId);
            if (hubConnections?.Count() >= 1)
            {
                await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(response.Results.ToList());
            }
            response.Results = new List<PushNotificationModel>();
            return response;
        }
        public async Task MarkNotificationRead(string userEmail, string productId, string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            await _notificationManager.MarkNotificationRead(userEmail, productId, notificationId, tenantId, environmentId, companyId);
        }
        public async Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            await _notificationManager.MarkGroupNotificationRead(userEmail, productId, groupId, notificationId, tenantId, environmentId, companyId);
        }

        public async Task MarkNotificationUnRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            await _notificationManager.MarkNotificationUnRead(userEmail, productId, groupId, notificationId, tenantId, environmentId, companyId);
        }

        public async Task PushNotification(NotificationsModel model)
        {
            if (!string.IsNullOrEmpty(model.Heading) && !string.IsNullOrEmpty(model.Message) && !string.IsNullOrEmpty(model.ProductId))
            {
                if (!model.IsSpecific)
                {
                    string notificationId = await _notificationManager.InsertNotification(model);

                    var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, Bodysize = model.Bodysize, CreatedOn = DateTime.UtcNow.ToString("MM/dd/yyyy h:mm tt"), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId,IsSpecific = model.IsSpecific, TenantId = model.TenantId, EnvironmentId = model.EnvironmentId, CompanyId = model.CompanyId } };
                    await Clients.Group(model.EnvironmentId).GetNotificaiton(notificationPush);
                }
                else
                {
                    var hubConnections = await _notificationManager.GetUserConnections(model.UserEmail, model.ProductId, model.TenantId, model.EnvironmentId, model.CompanyId);
                    if (hubConnections?.Count() >= 1)
                    {
                        string notificationId = await _notificationManager.InsertNotification(model);
                        var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, Bodysize = model.Bodysize, CreatedOn = DateTime.UtcNow.ToString("MM/dd/yyyy h:mm tt"), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId, IsSpecific = model.IsSpecific, TenantId = model.TenantId, EnvironmentId = model.EnvironmentId , CompanyId = model.CompanyId } };
                        await Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).GetNotificaiton(notificationPush);
                    }
                }
            }
        }

        public async Task PublishGlobalEnviromentNotification(NotificationsModel model)
        {
            if (!string.IsNullOrEmpty(model.Heading) && !string.IsNullOrEmpty(model.Message) && !string.IsNullOrEmpty(model.ProductId) && !string.IsNullOrEmpty(model.AppUrl) && !model.IsSpecific)
            {
                var subEnviroments = await _notificationManager.GetEnvironmentsByProduct(model.ProductId, model.AppUrl);

                if (subEnviroments.Count > 0)
                {
                    foreach (var subEnv in subEnviroments)
                    {
                        foreach (var company in subEnv.Companies)
                        {
                            model.GroupId = subEnv.Environment.Id;
                            model.CompanyId = company.CompanyId.ToString();
                            model.EnvironmentId = subEnv.Environment.Id;
                            model.TenantId = subEnv.Environment.TenantId;

                            string notificationId = await _notificationManager.InsertNotification(model);
                            if (!string.IsNullOrEmpty(notificationId))
                            {
                                var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel
                                    {
                                        Heading = model.Heading,
                                        Message = model.Message,
                                        Body = model.Body,
                                        UserEmail = model.UserEmail,
                                        RedirectUrl = model.RedirectUrl,
                                        CreatedOn = DateTime.UtcNow.ToString("MM/dd/yyyy h:mm tt"),
                                        Id = notificationId,
                                        IsRead = false,
                                        ProductId = model.ProductId,
                                        GroupId = model.GroupId,
                                        IsSpecific = model.IsSpecific,
                                        TenantId = model.TenantId,
                                        EnvironmentId = model.EnvironmentId,
                                        Bodysize = model.Bodysize,
                                        CompanyId = model.CompanyId
                                    }

                                };
                                await Clients.Group(subEnv.Environment.Id + company.CompanyId).GetNotificaiton(notificationPush);
                            }
                        }
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
                await RemoveFromGroup(hubConnection?.EnvironmentId);
                await RemoveFromGroup(hubConnection?.EnvironmentId + hubConnection?.CompanyId);
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
