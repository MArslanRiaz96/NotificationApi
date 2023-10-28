using Customer.Data.Models;
using Customer.Model.Common;
using Customer.Model.Environment;
using Customer.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Manager.Notifications
{
    public interface INotificationManager
    {
        public Task<string> InsertNotification(NotificationsModel notification);
        public Task<string> InsertNotificationChat(NotificationChatModel notification);
        public Task<List<HubConnection>> GetUserConnections(string UserName, string productId, string tenantId = "", string environmentId = "", string companyId = "");
        public Task<List<HubConnection>> GetUserConnectionForChat(string UserName);
        Task<PagedResult<PushNotificationModel>> GetUnreadNotifications(string userEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetUnreadGroupNotifications(string userEmail, string productId, string groupId = "", string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetReadNotifications(string UserEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetAllNotifications(string userEmail, string productId, int page, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetAllGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "");
        Task<PagedResult<PushNotificationModel>> GetAllTypeNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "");
        public Task MarkNotificationRead(string userEmail, string productId, string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "");
        public Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "");
        public Task MarkNotificationUnRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "");
        public Task<IList<EnvironmentCompanyDto>> GetEnvironmentsByProduct(string productId, string appUrl);
    }
}
