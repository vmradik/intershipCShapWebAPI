using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StoreApp.WebApi.Simple.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

            

        public ApplicationDbContext() : base("name=DatabaseContext")
        {
    //      Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
        //      public DatabaseContext(string conectionString) : base(conectionString) { }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }




        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
