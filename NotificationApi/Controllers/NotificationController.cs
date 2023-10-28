using Customer.Data.Models;
using Customer.Manager.Notifications;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NotificationApi.HubService;

namespace NotificationApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private IHubContext<NotificationHub, INotificationService> _hubContext;
        public INotificationManager _notificationManager;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public NotificationController(IHubContext<NotificationHub> notificationHub, IHubContext<NotificationHub, INotificationService> hubContext, INotificationManager notificationManager)
        {
            _hubContext = hubContext;
            _notificationManager = notificationManager;
            _notificationHub = notificationHub;

        }
        [HttpPost]
        public async Task<string> PushNotification([FromBody] NotificationsModel model)
        {
            string retMessage = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(model.Heading) && !string.IsNullOrEmpty(model.Message) && !string.IsNullOrEmpty(model.ProductId))
                {
                    if (!model.IsSpecific)
                    {
                        string notificationId = await _notificationManager.InsertNotification(model);

                        var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, Bodysize = model.Bodysize, CreatedOn = DateTime.UtcNow.ToString("MM/dd/yyyy h:mm tt"), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId, IsSpecific = model.IsSpecific, TenantId = model.TenantId, EnvironmentId = model.EnvironmentId, CompanyId = model.CompanyId } };
                        await _hubContext.Clients.Group(model.EnvironmentId).GetNotificaiton(notificationPush);

                        retMessage = "Success";
                    }
                    else
                    {
                        var hubConnections = await _notificationManager.GetUserConnections(model.UserEmail, model.ProductId, model.TenantId, model.EnvironmentId, model.CompanyId);
                        if (hubConnections?.Count() >= 1)
                        {
                            string notificationId = await _notificationManager.InsertNotification(model);
                            var notificationPush = new List<PushNotificationModel>() { new PushNotificationModel { Heading = model.Heading, Message = model.Message, Body = model.Body, UserEmail = model.UserEmail, RedirectUrl = model.RedirectUrl, Bodysize = model.Bodysize, CreatedOn = DateTime.UtcNow.ToString("MM/dd/yyyy h:mm tt"), Id = notificationId, IsRead = false, ProductId = model.ProductId, GroupId = model.GroupId, IsSpecific = model.IsSpecific, TenantId = model.TenantId, EnvironmentId = model.EnvironmentId, CompanyId = model.CompanyId } };
                            await _hubContext.Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).GetNotificaiton(notificationPush);
                        }
                        retMessage = "Success";
                    }
                }
                else
                {
                    retMessage = "Can't enter null or Empty values in Heading, Message, UserEmail and ProducdId";
                }
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }

        [HttpPost("PublishBulkNotification")]
        public async Task<string> PublishBulkNotification([FromBody] BulkNotificationModel model)
        {
            string retMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    foreach (string email in model.UserEmails)
                    {
                        //string notificationId = await _notificationManager.InsertNotification(model);
                        //await _hubContext.Clients.All.GetNotificaiton(model.Heading, model.Message, email, model.RedirectUrl, DateTime.UtcNow.ToString());

                    }
                    retMessage = "Success";
                }
                else
                {
                    foreach (string email in model.UserEmails)
                    {
                        //var hubConnections = await _notificationManager.GetUserConnections(model.UserName);
                        //if (hubConnections?.Count() >= 1)
                        //{
                        //    //string notificationId = await _notificationManager.InsertNotification(model);
                        //    //await _hubContext.Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).SendNotificationToClient(model.Heading, model.Message, email, model.RedirectUrl, DateTime.UtcNow.ToString(), model.UserName);
                        //}
                    }
                    retMessage = "Success";
                }

            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }
        [HttpGet("GetUnreadNotifications")]
        public async Task<IActionResult> GetUnreadNotifications([FromQuery] string userEmail, string productId, string notificationId = "")
        {
            var response = await _notificationManager.GetUnreadNotifications(userEmail, productId, notificationId);
            return Ok(response);
        }

        [HttpPost("MarkNotificationRead")]
        public async Task<IActionResult> MarkNotificationRead([FromBody] string userEmail, string productId, string notificationId = "")
        {
            await _notificationManager.MarkNotificationRead(userEmail, productId, notificationId);
            return Ok();
        }

        [HttpPost("PublishGlobalEnviromentNotification")]
        public async Task<string> PublishGlobalEnviromentNotification([FromBody] NotificationsModel model)
        {
            try
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
                                    await _hubContext.Clients.Group(subEnv.Environment.Id + company.CompanyId).GetNotificaiton(notificationPush);
                                }
                            }
                        }
                        return "Success";
                    }
                    return "No Environment found against Product";
                }
                else
                {
                    return "Can't enter null or Empty values in Heading, Message, UserEmail, ProducdId and Url";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        [HttpPost("ChatNotification")]
        public async Task<string> ChatNotification([FromBody] NotificationChatModel model)
        {
            string retMessage = string.Empty;
            if (!model.IsSpecific)
            {
                string notificationChatId = await _notificationManager.InsertNotificationChat(model);
                var notificationPush = new List<PushNotificationChatModel>() { new PushNotificationChatModel { Message=model.Message } };
                await _hubContext.Clients.Group(model.EnvironmentId).GetNotificaitonForChat(notificationPush);
                retMessage = "Success";
            }
            else
            {
                var hubConnections = await _notificationManager.GetUserConnectionForChat(model.ReceiverUserId);
                if (hubConnections?.Count() >= 1)
                {
                    string notificationChatId = await _notificationManager.InsertNotificationChat(model);
                    //var notificationPush = new List<PushNotificationChatModel>() { new PushNotificationChatModel { Message = model.Message } };
                    var notificationPush = new List<PushNotificationChatModel>() { new PushNotificationChatModel { Message = model.Message } };
                    await _hubContext.Clients.Clients(hubConnections.Select(x => x.ConnectionId).ToList()).GetNotificaitonForChat(notificationPush);
                    retMessage = "Success";
                }
            }                    

            return retMessage;
        }
    }
}