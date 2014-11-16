namespace VirtualGuide.Models.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VirtualGuide.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(VirtualGuide.Models.ApplicationDbContext context)
        {

            var UserManager = new BaseVGUserManager();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roles = new List<string>()
            {
                "Administrator", "Creator", "Approver", "User"
            };

            var users = new List<User>()
            {
                new User() {
                    UserName = "Admin@example.com",
                    Firstname = "Damian",
                    Lastname = "Adminowski"
                },
                new User() {
                    UserName = "Creator@example.com",
                    Firstname = "Anna",
                    Lastname = "Pisarka"
                },
                new User() {
                    UserName = "Approver@example.com",
                    Firstname = "Beata",
                    Lastname = "Aprobuj¹ca"
                },
                new User() {
                    UserName = "User@example.com",
                    Firstname = "Waldek",
                    Lastname = "Podró¿nik"
                }
            };

            foreach (var role in roles)
            {
                if (!RoleManager.RoleExists(role))
                {
                    RoleManager.Create(new IdentityRole(role));
                }
            }

            var i = 0;
            foreach (var user in users)
            {
                if (UserManager.FindByName(user.UserName) == null)
                {
                    user.Email = user.UserName;
                    UserManager.Create(user, user.UserName);
                    UserManager.AddToRole(user.Id, roles[i++]);
                }
                else
                {
                    i++;
                }
            }


            base.Seed(context);
        }
    }
}
