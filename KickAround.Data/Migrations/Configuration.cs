using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KickAround.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<KickAroundContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(KickAroundContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any(role => role.Name == "Admin"))
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new KickAroundContext()));

                var roleCreateResult = roleManager.Create(new IdentityRole("Admin"));
                if (!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleCreateResult.Errors));
                }
            }

            //if (!context.Roles.Any(role => role.Name == "User"))
            //{
            //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new KickAroundContext()));

            //    var roleCreateResult = roleManager.Create(new IdentityRole("User"));
            //    if (!roleCreateResult.Succeeded)
            //    {
            //        throw new Exception(string.Join("; ", roleCreateResult.Errors));
            //    }
            //}
        }
    }
}
