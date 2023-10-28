using Customer.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Customer.Data.Extentions;
using Customer.Data.Models;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Customer.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       // public DbSet<Logging> Loggings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<HubConnection> HubConnections { get; set; }
        public DbSet<Product> NotificationProducts { get; set; }
        public DbSet<GroupNotification> GroupNotifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<TenantEnvironment> Environments { get; set; }
        public DbSet<NotificationChat> NotificationChats { get; set; }
        public override int SaveChanges()
        {
            UpdateAuditableProperties();
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<SpExternalUsersResult>()
            //.HasNoKey().ToTable("SpExternalUsersResult", t => t.ExcludeFromMigrations());

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

            // builder.Entity<Lookup>()
            //     .HasIndex(u => u.Value)
            //     .IsUnique();

            // builder.Entity<Media>()
            // .Property(e => e.PrincipalName)
            //.HasConversion(
            //    v => v.ToString(),
            //    v => (PrincipalTables)Enum.Parse(typeof(PrincipalTables), v));

            // builder.Entity<Models.Customer>()
            //     .HasIndex(c => c.Code)
            //     .IsUnique(true);
            // builder.Entity<Process>()
            //     .HasIndex(p => new { p.Code, p.Direction, p.EdiVersion, p.PartnerId })
            //     .IsUnique(true);
            // builder.Entity<Partner>()
            //     .HasIndex(p => new { p.CompanyId, p.Code })
            //     .IsUnique(true);
            // builder.Entity<Company>()
            // .HasIndex(p => new { p.SubscriptionId, p.Code })
            // .IsUnique(true);
            // builder.Entity<Subscription>()
            // .HasIndex(p => new { p.CustomerId, p.ProductId })
            // .IsUnique(true);


            builder.Seed();

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditableProperties()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        //entry.Entity.CreatedBy = "USER_ID";
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedOn = DateTime.UtcNow;
                        //entry.Entity.ModifiedBy = "USER_ID";
                        break;
                }
            }
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.Entity.IsActive = false;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }

    }
}
