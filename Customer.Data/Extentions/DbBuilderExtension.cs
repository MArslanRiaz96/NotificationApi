using Customer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Data.Extentions
{
    public static class DbBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = "11db7c7d-2ddb-49b6-9c40-ff4dc23a7730",
                    Name = "PartnerLinq US",
                    CreatedBy = "arslan",
                    ModifiedBy = "arslan",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    IsActive = true
                }
            );

            modelBuilder.Entity<Product>().HasData(
               new Product
               {
                   Id = "56730618-A053-4605-BFA0-42DC6CBE0CF7",
                   Name = "Data Fabric",
                   CreatedBy = "arslan",
                   ModifiedBy = "arslan",
                   CreatedOn = DateTime.UtcNow,
                   ModifiedOn = DateTime.UtcNow,
                   IsActive = true
               }
           );
        }
    }
}
