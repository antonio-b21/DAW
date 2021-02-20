using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAW_Lab_4.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DBConnectionString") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, DAW_Lab_4.Migrations.Configuration>("DBConnectionString"));
        }
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}