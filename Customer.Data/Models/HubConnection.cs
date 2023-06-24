using Customer.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class HubConnection
    {
        [Key]
        public string Id { get; set; }
        public string ConnectionId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string ProductId { get; set; }
        public string TenantId { get; set; }
        public string EnvironmentId { get; set; }
        public string CompanyId { get; set; }
        public virtual Product Products { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
