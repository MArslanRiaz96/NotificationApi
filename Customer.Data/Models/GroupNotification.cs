using Customer.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class GroupNotification : ISoftDelete
    {
        [Key]
        public string Id { get; set; }
        public string NotificationId { get; set; }
        public virtual Notification Notifications { get; set; }
        public virtual string UserEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Boolean IsRead { get; set; }
    }
}
