using Customer.Data.Common;
using Customer.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class Notification : ISoftDelete
    {
        [Key]
        public string Id { get; set; }
        public string Heading { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }
        public string UserEmail { get; set; }
        public Boolean IsRead { get; set; }
        public string RedirectUrl { get; set; }
        public string Bodysize { get; set; }
        public string ProductId { get; set; }
        public string TenantId { get; set; }
        public string EnvironmentId { get; set; }
        public string CompanyId { get; set; }
        public virtual Product Products { get; set; }
        public virtual string GroupId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSpecific { get; set; } = true;
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<GroupNotification> GroupNotifications { get; set; }

    }
}
