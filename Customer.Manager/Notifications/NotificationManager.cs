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

        public async Task<List<Notification>> GetUnreadNotifications(string UserEmail, string notificationId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(notificationId))
                {
                    return await _context.Notifications.Where(x => x.UserEmail == UserEmail && x.IsRead == false).ToListAsync();
                }
                else
                {
                    return await _context.Notifications.Where(x => x.Id == notificationId && x.IsRead == false).ToListAsync();
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

        public async Task<List<HubConnection>> GetUserConnections(string UserName)
        {
            return await _context.HubConnections.Where(con => con.Username == UserName).ToListAsync();
        }

        public async Task<string> InsertNotification(NotificationsModel notification)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var notifications = _mapper.Map<Notification>(notification);

                notifications.Id = Guid.NewGuid().ToString();
                notifications.CreatedBy = "Api";
                notifications.ModifiedBy = "Api";
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

        public async Task MarkNotificationRead(string UserEmail, string notificationId = "")
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                List<Notification> notifications = await GetUnreadNotifications(UserEmail, notificationId);
                if (notifications != null && notifications.Count > 0)
                {
                    notifications.ForEach(x => x.IsRead = true);
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
