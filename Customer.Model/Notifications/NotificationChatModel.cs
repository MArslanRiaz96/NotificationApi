using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Notifications
{
    public class NotificationChatModel
    {
        public string Id { get; set; }
        public string ReceiverUserId { get; set; }
        public string Message { get; set; }
        public bool IsSpecific { get; set; }
        public string EnvironmentId { get; set; }
    }
    public class NotificationBody
    {
        public string Id { get; set; } = null;
        public string ReceiverUserId { get; set; } = null;
        public string Message { get; set; } = null;
        public bool IsSpecific { get; set; }
        public string EnvironmentId { get; set; } = null;
    }
}
