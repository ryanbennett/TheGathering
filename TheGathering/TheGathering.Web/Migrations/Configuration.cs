namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TheGathering.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TheGathering.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override async void Seed(TheGathering.Web.Models.ApplicationDbContext context)
        {
            string[] roles = new string[] { "admin","volunteer","groupleader" };
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            foreach (string role in roles)
            {

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleManager.Create(new IdentityRole(role));
                }
            }


            var user = new ApplicationUser
            {
               
                Email = "admin@thegatheringwis.org",
                UserName = "admin@thegatheringwis.org",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                userManager.Create(user, "F3edTheHungry1235#");
                userManager.AddToRole(user.Id, "admin");
             
            }

        }

  
    
    }
}
