using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using KickAround.Models.BindingModels.Games;
using KickAround.Models.BindingModels.Groups;
using KickAround.Models.BindingModels.Users;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Games;
using KickAround.Models.ViewModels.Groups;
using KickAround.Models.ViewModels.Users;
using KickAround.Data.Migrations;

namespace KickAround.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var context = new KickAroundContext();
            //using (context)
            //{
            //    if (!context.Roles.Any(role => role.Name == "Admin"))
            //    {
            //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new KickAroundContext()));

            //        var roleCreateResult = roleManager.Create(new IdentityRole("Admin"));
            //        if (!roleCreateResult.Succeeded)
            //        {
            //            throw new Exception(string.Join("; ", roleCreateResult.Errors));
            //        }
            //    }

            //    if (!context.Roles.Any(role => role.Name == "User"))
            //    {
            //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new KickAroundContext()));

            //        var roleCreateResult = roleManager.Create(new IdentityRole("User"));
            //        if (!roleCreateResult.Succeeded)
            //        {
            //            throw new Exception(string.Join("; ", roleCreateResult.Errors));
            //        }
            //    }

            //}

            ConfigureMapper();
        }

        private void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Group, GroupViewModel>();
                cfg.CreateMap<CreateGroupBindingModel, Group>();
                cfg.CreateMap<CreateGroupBindingModel, Location>();
                cfg.CreateMap<Group, EditGroupBindingModel>();
                cfg.CreateMap<CreateGameBindingModel, Game>();
                cfg.CreateMap<Game, GameViewModel>();
                cfg.CreateMap<Game, GameDetailsViewModel>();
                cfg.CreateMap<Game, EditGameBindingModel>();
                cfg.CreateMap<FeedbackBindingModel, Feedback>();
                cfg.CreateMap<User, UserDetailsViewModel>();
            });
        }
    }
}
