using Customer.Data.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Data.Models
{
    public class Company: FullyAuditableEntity
    {
        [Key]
        public string  Id { get; set; }
        public string?  Name { get; set; }
        public string? Code { get; set; }
        public string? Alias { get; set; }
        public string SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; }
        public int CompanyId { get; set; }
    }
}
