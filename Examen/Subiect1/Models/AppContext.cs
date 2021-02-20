using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BiganAntonioM41.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, BiganAntonioM41.Migrations.Configuration>("DBConnectionString"));
        }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}