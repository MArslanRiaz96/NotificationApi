using Customer.Manager.Notifications;
using Customer.Model.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationApi.Hub;

namespace NotificationApi.Controllers
{
   // [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private IHubContext<NotificationHub, INotificationService> _hubContext;
        public INotificationManager _notificationManager;

        public NotificationController(IHubContext<NotificationHub, INotificationService> hubContext, INotificationManager notificationManager)
        {
            _hubContext = hubContext;
            _notificationManager = notificationManager;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] NotificationsModel notification)
        {
            string retMessage = string.Empty;

            try
            {
               await _hubContext.Clients.All.GetNotificaiton(notification.Heading, notification.Message, notification.UserEmail, notification.RedirectUrl, DateTime.UtcNow.ToString());

               retMessage = "Success";
               await _notificationManager.InsertNotification(notification);
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }
        [HttpPost("PushNotification")]
        public async Task<string> PushNotification([FromBody] NotificationsModel model)
        {
            string retMessage = string.Empty;

            try
            {
                await _hubContext.Clients.All.GetNotificaiton(model.Heading, model.Message, model.UserEmail, model.RedirectUrl, DateTime.UtcNow.ToString());
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }

        [HttpPost("PublishBulkNotification")]
        public async Task<string> PublishBulkNotification([FromBody] BulkNotificationModel notification)
        {
            string retMessage = string.Empty;

            try
            {
                foreach (string email in notification.UserEmails)
                {
                    await _hubContext.Clients.All.GetNotificaiton(notification.Heading, notification.Message, email, notification.RedirectUrl, DateTime.UtcNow.ToString());

                }
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }

        // [HttpGet("GetUnreadNotifications/{userEmail}")]
        [HttpGet]
        public async Task<IActionResult> GetUnreadNotifications(string userEmail)
        {
            var response = await _notificationManager.GetUnreadNotifications(userEmail);
            return Ok(response);
        }

        [HttpPost("MarkNotificationRead")]
        public async Task<IActionResult> MarkNotificationRead([FromBody] string userEmail)
        {
            await _notificationManager.MarkNotificationRead(userEmail);
            return Ok();
        }
    }
}