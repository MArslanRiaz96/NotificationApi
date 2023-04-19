using Customer.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class Notification : FullyAuditableEntity
    {
        [Key]
        public string Id { get; set; }
        public virtual string Heading { get; set; }
        public virtual string Message { get; set; }
        public virtual string UserEmail { get; set; }
        public virtual Boolean IsRead { get; set; }
        public virtual string RedirectUrl { get; set; }


    }
}
