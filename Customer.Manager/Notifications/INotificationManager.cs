using Customer.Data.Models;
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
        public Task InsertNotification(NotificationsModel notification);
        Task<List<Notification>> GetUnreadNotifications(string UserEmail);
        public Task MarkNotificationRead(string UserEmail);
    }
}
