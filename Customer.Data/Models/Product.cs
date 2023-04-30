using Customer.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Models
{
    public class Product : FullyAuditableEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
