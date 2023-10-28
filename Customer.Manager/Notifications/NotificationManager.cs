using AutoMapper;
using Customer.Data.Context;
using Customer.Data.Extentions;
using Customer.Data.Models;
using Customer.Model.Common;
using Customer.Model.Company;
using Customer.Model.Environment;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public async Task<PagedResult<PushNotificationModel>> GetUnreadNotifications(string UserEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                var result = await PaginationHelper.PaginateAsync(_context.Notifications.Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && (string.IsNullOrEmpty(notificationId) || x.Id == notificationId) && x.UserEmail == UserEmail && x.ProductId == productId && x.IsSpecific == true && x.IsRead == false).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);
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
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PagedResult<PushNotificationModel>> GetUnreadGroupNotifications(string UserEmail, string productId, string groupId = "", string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                var result = await PaginationHelper.PaginateAsync(_context.Notifications
                .Include(x => x.GroupNotifications.Where(x => x.UserEmail == UserEmail && x.IsRead == true))
                    .Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && (string.IsNullOrEmpty(notificationId) || x.Id == notificationId) && (string.IsNullOrEmpty(groupId) || x.GroupId == groupId) && x.ProductId == productId && x.IsSpecific == false && !x.GroupNotifications.Any(x => x.UserEmail == UserEmail && x.IsRead == true)).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);

                result.ForEach(x => { x.IsRead = x.GroupNotifications.Any(z => z.IsRead); x.UserEmail = UserEmail; });
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
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<GroupNotification> GetReadGroupNotification(string UserEmail, string notificationId = "")
        {
            try
            {
                return await _context.GroupNotifications
                    .FirstOrDefaultAsync(x => x.NotificationId == notificationId && x.UserEmail == UserEmail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PagedResult<PushNotificationModel>> GetReadNotifications(string UserEmail, string productId, string notificationId = "", int page = 1, int pageSize = 10, string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                var result = await PaginationHelper.PaginateAsync(_context.Notifications.Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && (string.IsNullOrEmpty(notificationId) || x.Id == notificationId) && x.UserEmail == UserEmail && x.ProductId == productId && x.IsSpecific == true && x.IsRead == true).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<PushNotificationModel>> GetAllNotifications(string userEmail, string productId, int page, int pageSize, string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(productId))
                {
                    var result = await PaginationHelper.PaginateAsync(_context.Notifications.Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && x.ProductId == productId && x.IsSpecific == true && x.UserEmail == userEmail).OrderByDescending(x => x.CreatedOn).AsQueryable(), page, pageSize);
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

        public async Task<PagedResult<PushNotificationModel>> GetReadGroupNotifications(string userEmail, string productId, int page, int pageSize, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(productId))
                {
                    return new PagedResult<PushNotificationModel>();
                }

                var query = _context.Notifications
                    .Include(x => x.GroupNotifications.Where(g => g.UserEmail == userEmail && g.IsRead))
                    .Where(x => x.ProductId == productId && !x.IsSpecific)
                    .Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && (string.IsNullOrEmpty(groupId) || x.GroupId == groupId))
                    .Where(x => x.GroupNotifications.Any(g => g.UserEmail == userEmail && g.IsRead))
                    .OrderByDescending(x => x.CreatedOn);

                var result = await PaginationHelper.PaginateAsync(query, page, pageSize);
                result.ForEach(x => { x.IsRead = x.GroupNotifications.Any(z => z.IsRead); x.UserEmail = userEmail; });
                var pushNotifications = _mapper.Map<List<PushNotificationModel>>(result);
                var pagedResult = new PagedResult<PushNotificationModel>
                {
                    Results = pushNotifications,
                    PageSize = result.PageSize,
                    RowCount = result.TotalItemCount,
                    CurrentPage = page,
                    PageCount = result.PageCount
                };

                return pagedResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<PushNotificationModel>> GetAllGroupNotifications(string userEmail, string productId, int page, int pageSize, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(productId))
                {
                    return new PagedResult<PushNotificationModel>();
                }

                var query = _context.Notifications
                    .Include(x => x.GroupNotifications.Where(g => g.UserEmail == userEmail && g.IsRead))
                    .Where(x => x.ProductId == productId && !x.IsSpecific)
                    .Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && (string.IsNullOrEmpty(groupId) || x.GroupId == groupId))
                    .OrderByDescending(x => x.CreatedOn);

                var result = await PaginationHelper.PaginateAsync(query, page, pageSize);
                result.ForEach(x => { x.IsRead = x.GroupNotifications.Any(z => z.IsRead); x.UserEmail = userEmail; });
                var pushNotifications = _mapper.Map<List<PushNotificationModel>>(result);
                var pagedResult = new PagedResult<PushNotificationModel>
                {
                    Results = pushNotifications,
                    PageSize = result.PageSize,
                    RowCount = result.TotalItemCount,
                    CurrentPage = page,
                    PageCount = result.PageCount
                };

                return pagedResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PagedResult<PushNotificationModel>> GetAllTypeNotifications(string userEmail, string productId, int page, int pageSize, string groupId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(productId))
                {
                    return new PagedResult<PushNotificationModel>();
                }

                var query = _context.Notifications
               .Where(x => (string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId) && (string.IsNullOrEmpty(environmentId) || x.EnvironmentId == environmentId) && (string.IsNullOrEmpty(companyId) || x.CompanyId == companyId) && x.ProductId == productId)
               .OrderByDescending(x => x.CreatedOn)
               .Where(x => (x.IsSpecific && x.UserEmail == userEmail) || (!x.IsSpecific && (string.IsNullOrEmpty(groupId) || x.GroupId == groupId)));

                var result = await PaginationHelper.PaginateAsync(query.AsQueryable(), page, pageSize);

                var notificationIds = result.Where(x => !x.IsSpecific).Select(x => x.Id).ToList();

                var groupNotifications = _context.GroupNotifications
                .Where(z => z.IsRead && notificationIds.Contains(z.NotificationId) && z.UserEmail == userEmail)
                .GroupBy(z => z.NotificationId)
                .Select(g => g.FirstOrDefault())
                .ToList();

                result.Where(x => !x.IsSpecific).ToList().ForEach(x =>
                {
                    x.IsRead = groupNotifications.Any(z => z.NotificationId == x.Id);
                    x.UserEmail = userEmail;
                });

                var pushNotifications = _mapper.Map<List<PushNotificationModel>>(result);
                var pagedResult = new PagedResult<PushNotificationModel>
                {
                    Results = pushNotifications,
                    PageSize = result.PageSize,
                    RowCount = result.TotalItemCount,
                    CurrentPage = page,
                    PageCount = result.PageCount
                };

                return pagedResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<HubConnection>> GetUserConnections(string UserName, string productId, string tenantId = "", string environmentId = "", string companyId = "")
        {
            return await _context.HubConnections.Where(con => con.Username == UserName && con.ProductId == productId && (string.IsNullOrEmpty(tenantId) || con.TenantId == tenantId) && (string.IsNullOrEmpty(companyId) || con.CompanyId == companyId)).ToListAsync();
        }
        public async Task<List<HubConnection>> GetUserConnectionForChat(string UserName)
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
                notifications.CreatedOn = DateTime.UtcNow;
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

        public async Task MarkNotificationRead(string userEmail, string productId, string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                PagedResult<PushNotificationModel> pushNotifications = await GetUnreadNotifications(userEmail, productId, notificationId, 1, 1000, tenantId, environmentId, companyId);
                var notifications = _mapper.Map<List<Notification>>(pushNotifications?.Results);

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

        public async Task MarkGroupNotificationRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            PagedResult<PushNotificationModel> pushNotifications = await GetUnreadGroupNotifications(userEmail, productId, groupId, notificationId, 1, 1000, tenantId, environmentId, companyId);
            var notifications = _mapper.Map<List<Notification>>(pushNotifications?.Results);

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

        public async Task MarkNotificationUnRead(string userEmail, string productId, string groupId = "", string notificationId = "", string tenantId = "", string environmentId = "", string companyId = "")
        {
            if (string.IsNullOrEmpty(groupId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        PagedResult<PushNotificationModel> pushNotifications = await GetReadNotifications(userEmail, productId, notificationId, 1, 1, tenantId, environmentId, companyId);
                        var notifications = _mapper.Map<List<Notification>>(pushNotifications?.Results);

                        if (notifications != null && notifications.Count > 0)
                        {
                            notifications.ForEach(x => x.IsRead = false);
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
            else if (!string.IsNullOrEmpty(groupId))
            {
                var groupNotification = await GetReadGroupNotification(userEmail, notificationId);

                if (groupNotification != null)
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            _context.GroupNotifications.Remove(groupNotification);
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

        public async Task<IList<EnvironmentCompanyDto>> GetEnvironmentsByProduct(string productId, string appUrl)
        {
            var enviromentComp = new List<EnvironmentCompanyDto>();
            try
            {
                if (!string.IsNullOrEmpty(productId) && !string.IsNullOrEmpty(appUrl))
                {
                    enviromentComp = await _context.Subscriptions
                                .Join(_context.Environments,
                                 s => s.Id,
                                 e => e.TenantId,
                                 (s, e) => new { s, e })
                                .Where(x => x.s.ProductId == productId && (x.e.Url.Contains(appUrl) || x.e.PlanningStudioUrl.Contains(appUrl)))
                                .Select(x => new EnvironmentCompanyDto
                                {
                                    Companies = _mapper.Map<List<CompanyDto>>(x.s.Companies),
                                    Environment = _mapper.Map<EnvironmentDto>(x.e)

                                }).ToListAsync();
                }
            }
            catch (Exception e)
            {
              
            }

            return enviromentComp;
        }

        public async Task<string> InsertNotificationChat(NotificationChatModel notification)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var notificationChat = _mapper.Map<NotificationChat>(notification);

                notificationChat.Id = Guid.NewGuid().ToString();
                _context.NotificationChats.Add(notificationChat);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return notificationChat.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
