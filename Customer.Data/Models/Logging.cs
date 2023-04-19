using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class Logging
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Tenant_Id { get; set; }
        public string Status { get; set; }
        public string ComapnyId { get; set; }
        public string Action { get; set; }
        public string Email { get; set; }
        public string Application_Id { get; set; }
        public int App_Id { get; set; }
        public string Request_Type { get; set; }
        public string ActionDetail { get; set; }
        public DateTime Request_Date { get; set; }
        public string Request_By { get; set; }

    }
}
