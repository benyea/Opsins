using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Mvc;

namespace Opsins.CMS.Web
{
    using Infra;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            OnStart();

            //DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(RegisterDependencies()));
        }

        protected void Application_End()
        {
            OnEnd();
        }

        public static void OnStart()
        {
            Bootstrapper.Run();
        }

        public static void OnEnd()
        {
            Ioc.Reset();
        }

        protected IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CMS.Services.Imple.UserService>().As<Services.IUserService>().InstancePerLifetimeScope();

            var container = builder.Build();
            return container;
        }
    }
}