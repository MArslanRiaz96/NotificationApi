using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Notifications
{
    public class PushNotificationModel
    {
        public string Id { get; set; }
        public string Heading { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string RedirectUrl { get; set; }
        public bool IsSpecific { get; set; } = false;
        public string ProductId { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public string CreatedDate { get; set; }
    }
}
