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

        public async Task<List<Notification>> GetUnreadNotifications(string UserEmail)
        {
            try
            {
                return await _context.Notifications.Where(x=>x.UserEmail == UserEmail).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task InsertNotification(NotificationsModel notification)
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
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task MarkNotificationRead(string UserEmail)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                List<Notification> notifications = await GetUnreadNotifications(UserEmail);
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
