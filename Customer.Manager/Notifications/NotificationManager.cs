using AutoMapper;
using Customer.Data.Context;
using Customer.Data.Models;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Customer.Manager.Notifications
{
    public class NotificationManager : INotificationManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public NotificationManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PushNotificationModel>> GetUnreadNotifications(string UserEmail, string productId, string notificationId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(notificationId))
                {
                    var notification =  await _context.Notifications.Where(x => x.UserEmail == UserEmail && x.ProductId == productId && x.IsRead == false).ToListAsync();
                    return _mapper.Map<List<PushNotificationModel>>(notification);
                }
                else
                {
                    var notification = await _context.Notifications.Where(x => x.Id == notificationId && x.ProductId == productId && x.IsRead == false).ToListAsync();
                    return _mapper.Map<List<PushNotificationModel>>(notification);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Notification>> GetNotifications(string UserEmail, string notificationId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(notificationId))
                {
                    return await _context.Notifications.Where(x => x.UserEmail == UserEmail).Take(30).ToListAsync();
                }
                else
                {
                    return await _context.Notifications.Where(x => x.Id == notificationId).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<HubConnection>> GetUserConnections(string UserName,string productId)
        {
            return await _context.HubConnections.Where(con => con.Username == UserName && con.ProductId == productId).ToListAsync();
        }

        public async Task<string> InsertNotification(NotificationsModel notification)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var notifications = _mapper.Map<Notification>(notification);

                notifications.Id = Guid.NewGuid().ToString();
                _context.Notifications.Add(notifications);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return notifications.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task MarkNotificationRead(string userEmail, string productId, string notificationId = "")
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                List<PushNotificationModel> pushNotifications = await GetUnreadNotifications(userEmail, productId, notificationId);
                var notifications = _mapper.Map<List<Notification>>(pushNotifications);

                if (notifications != null && notifications.Count > 0)
                {
                    notifications.ForEach(x => x.IsRead = true);
                    // Detach the existing tracked entities with the same key values
                    foreach (var notification in notifications)
                    {
                        var existingNotification = _context.Notifications.Local.FirstOrDefault(n => n.Id == notification.Id);
                        if (existingNotification != null)
                        {
                            _context.Entry(existingNotification).State = EntityState.Detached;
                        }
                    }
                    _context.Notifications.UpdateRange(notifications);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
