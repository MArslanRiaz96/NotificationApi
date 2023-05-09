using AutoMapper;
using Customer.Data.Context;
using Customer.Data.Extentions;
using Customer.Data.Models;
using Customer.Model.Common;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                var notification = await _context.Notifications.Where(x => (string.IsNullOrEmpty(notificationId) || x.Id == notificationId) && x.UserEmail == UserEmail && x.ProductId == productId && x.IsSpecific == true && x.IsRead == false).OrderByDescending(x => x.CreatedOn).ToListAsync();
                return _mapper.Map<List<PushNotificationModel>>(notification);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PushNotificationModel>> GetUnreadGroupNotifications(string UserEmail, string productId, string groupId = "", string notificationId = "")
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(x => x.GroupNotifications)
                    .Where(x => (string.IsNullOrEmpty(notificationId) || x.Id == notificationId) && (string.IsNullOrEmpty(groupId) || x.GroupId == groupId) && x.UserEmail == UserEmail && x.ProductId == productId && x.IsSpecific == false && !x.GroupNotifications.Any(x => x.UserEmail == UserEmail && x.IsRead == true)).OrderByDescending(x => x.CreatedOn).ToListAsync();

                notifications.ForEach(x => x.IsRead = x.GroupNotifications.Any(z => z.IsRead));

                return _mapper.Map<List<PushNotificationModel>>(notifications);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PagedResult<PushNotificationModel>> GetReadNotifications(string userEmail, string productId, int page, int pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(productId))
                {
                    var result = await PaginationHelper.PaginateAsync(_context.Notifications.Where(x => x.UserEmail == userEmail && x.IsRead == true).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);
                    var test = result.TotalItemCount;
                    var test2 = result.HasPreviousPage;
                    var test3 = result.HasNextPage;
                    var test4 = result.IsLastPage;
                    var pushNotification = _mapper.Map<List<PushNotificationModel>>(result);
                    var pagedresult = new PagedResult<PushNotificationModel>
                    {
                        Results = pushNotification,
                        PageSize = result.PageSize,
                        RowCount = result.TotalItemCount,
                        CurrentPage = page,
                        PageCount = result.PageCount
                    };

                    return pagedresult;
                }
                return new PagedResult<PushNotificationModel>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize, string groupId = "")
        {
            try
            {

                if (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(productId))
                {
                    var result = await PaginationHelper.PaginateAsync(_context.Notifications
                    .Include(x => x.GroupNotifications)
                    .Where(x => (string.IsNullOrEmpty(groupId) || x.GroupId == groupId) && x.UserEmail == userEmail && x.ProductId == productId && x.IsSpecific == false && x.IsRead == false && x.GroupNotifications.Any(z => z.UserEmail == userEmail && z.IsRead == true)).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);

                    result.ForEach(x => x.IsRead = x.GroupNotifications.Any(z => z.IsRead));
                    var pushNotifications = _mapper.Map<List<PushNotificationModel>>(result);
                    var pagedresult = new PagedResult<PushNotificationModel>
                    {
                        Results = pushNotifications,
                        PageSize = result.PageSize,
                        RowCount = result.TotalItemCount,
                        CurrentPage = page,
                        PageCount = result.PageCount
                    };

                    return pagedresult;
                }
                return new PagedResult<PushNotificationModel>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<HubConnection>> GetUserConnections(string UserName, string productId)
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
                notifications.CreatedOn = DateTime.Now;
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

        public async Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "")
        {
            List<PushNotificationModel> pushNotifications = await GetUnreadGroupNotifications(userEmail, productId, groupId, notificationId);
            var notifications = _mapper.Map<List<Notification>>(pushNotifications);

            if (notifications.Any())
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var groupNotifications = notifications.Select(notification =>
                        new GroupNotification
                        {
                            Id = Guid.NewGuid().ToString(),
                            NotificationId = notification.Id,
                            IsRead = true,
                            UserEmail = userEmail
                        }).ToList();

                        await _context.GroupNotifications.AddRangeAsync(groupNotifications);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}
