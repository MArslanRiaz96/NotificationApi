using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class NotificationChat
    {
        [Key]
        public string Id { get; set; }
        public string ReceiverUserId { get; set; }
        public string Message { get; set; }
        public bool IsSpecific { get; set; }
        public string EnvironmentId { get; set; }
    }
}
