using Customer.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class HubConnection : ISoftDelete
    {
        [Key]
        public string Id { get; set; }
        public string ConnectionId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
