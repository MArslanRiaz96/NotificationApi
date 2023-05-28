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
        public string Body { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string RedirectUrl { get; set; }
        public bool IsSpecific { get; set; } = false;
        public string ProductId { get; set; } = null!;
        public string GroupId { get; set; } 
        public bool IsRead { get; set; } = false;
        public string CreatedOn { get; set; }
        public string TenantId { get; set; }
        public string EnvironmentId { get; set; }
        public string CompanyId { get; set; }
    }
}
