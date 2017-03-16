namespace temalab.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<temalab.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Roles.FirstOrDefault(r => r.Name == "Admin") == null)
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }



            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (userManager.FindByName("admin@admin.com") == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

                userManager.Create(user, "Temp_123");
                userManager.AddToRole(user.Id, "Admin");
            }

      /*      if(context.Pictures.FirstOrDefault(p => p.Path == "~/Images/noimage.jpg") == null)
            {
                Picture defaultPic = new Picture();
                defaultPic.Path = "~/Images/noimage.jpg";

                context.Pictures.Add(defaultPic);
                context.SaveChanges();
            }*/

        }
    }
}
