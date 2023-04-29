﻿using Customer.Data.Models;
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
        public Task<List<HubConnection>> GetUserConnections(string UserName);
        Task<List<Notification>> GetUnreadNotifications(string UserEmail, string notificationId = "");
        Task<List<Notification>> GetNotifications(string UserEmail, string notificationId = "");
        public Task MarkNotificationRead(string UserEmail, string notificationId = "");
    }
}
