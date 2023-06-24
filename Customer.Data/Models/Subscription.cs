using Customer.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Customer.Data.Models
{
    public class Subscription: FullyAuditableEntity
    {
        [Key]
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? CustomerId { get; set; }
        public string? ProductId { get;set; }
        public DateTime StartDate { get; set; }
        public string? TeamChannelId { get; set; }
        public string? TeamDriveItemId { get; set; }
        public string? TeamDriveParentRefId { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
      
    }
}
