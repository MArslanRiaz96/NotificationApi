using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Notifications
{
    public class NotificationsModel
    {
        public string Heading { get; set; }
        public string Message { get; set; }
        public string UserEmail { get; set; }
        public string RedirectUrl { get; set; }
    }
}
