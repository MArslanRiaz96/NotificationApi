using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Notifications
{
    public class PushNotificationModel : NotificationsModel
    {

        public bool IsRead { get; set; } = false;

        public string CreatedDate { get; set; }
    }
}
