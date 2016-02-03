namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WMATC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WMATC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WMATC.Models.ApplicationDbContext";
        }

        protected override void Seed(WMATC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            AddUserAndRole(context);
            context.Events.AddOrUpdate(p => p.Title,
                                               new Event
                                               {
                                                   Title = "Test ATC",
                                                  // EventId = 1,
                                                   EventDate = DateTime.Now
                                               });

        }

        bool AddUserAndRole(WMATC.Models.ApplicationDbContext context)
        {
            //IdentityResult ir;
            //var rm = new RoleManager<IdentityRole>
            //    (new RoleStore<IdentityRole>(context));
            //ir = rm.Create(new IdentityRole("canEdit"));
            //var um = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(context));
            //var user = new ApplicationUser()
            //{
            //    UserName = "user1@contoso.com",
            //};
            //ir = um.Create(user, "P_assw0rd1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            //ir = um.AddToRole(user.Id, "canEdit");
            //return ir.Succeeded;
            return false; 
        }
    }
}
