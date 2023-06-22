using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Notifications
{
    public class NotificationsModel
    {
        public string Id { get; set; }
        public string Heading { get; set; }  = null!;
        public string Message { get; set; } 
        public string Body { get; set; }
        public string UserEmail { get; set; }
        public string RedirectUrl { get; set; }
        public string Bodysize { get; set; }
        public bool IsSpecific { get; set; } = false;
        public string ProductId { get; set; } = null!;
        public string GroupId { get; set; }
        public string TenantId { get; set; }
        public string EnvironmentId { get; set; }
        public string CompanyId { get; set; }

        public string AppUrl { get; set; }


    }
}
