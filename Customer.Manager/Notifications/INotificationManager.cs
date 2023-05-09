﻿using Customer.Data.Models;
using Customer.Model.Common;
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
        public Task<List<HubConnection>> GetUserConnections(string UserName, string productId);
        Task<List<PushNotificationModel>> GetUnreadNotifications(string userEmail, string productId, string notificationId = "");
        Task<List<PushNotificationModel>> GetUnreadGroupNotifications(string userEmail, string productId, string groupId = "", string notificationId = "");
        Task<PagedResult<PushNotificationModel>> GetReadNotifications(string userEmail, string productId, int page, int pageSize = 10);
        Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize = 10, string groupId = "");
        public Task MarkNotificationRead(string userEmail, string productId, string notificationId = "");
        public Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "");
    }
}
