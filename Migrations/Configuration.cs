namespace Resume_Portal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Resume_Portal.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    
    internal sealed class Configuration : DbMigrationsConfiguration<Resume_Portal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Resume_Portal.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            //if (!RoleManager.RoleExists("Admin"))
            //{
            //    RoleManager.Create(new IdentityRole("Admin"));
            //    RoleManager.Create(new IdentityRole("Instructor"));
            //    RoleManager.Create(new IdentityRole("Student"));
            //    RoleManager.Create(new IdentityRole("Employer"));
            //}
            //if (!context.Users.Any(u => u.UserName == "Admin@test.com"))
            //{
            //    ApplicationUser Admin = new ApplicationUser { UserName = "Admin@test.com", Email = "Admin@test.com", LastLogedIn = DateTime.Now };
            //    UserManager.Create(Admin, "EntityFr@mew0rk");
            //    UserManager.AddToRole(Admin.Id, "Admin");
            //}
        }
    }
}
