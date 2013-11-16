using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IocLite;
using IocLite.Interfaces;
using MenuGen.MenuNodeGenerators;
using MenuGen.SampleApp.App_Start;
using MenuGen.SampleApp.Data;
using MenuGen.SampleApp.Ioc;
using MenuGen.SampleApp.MenuGenerators;

namespace MenuGen.SampleApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MenuGen.Init(x =>
            {
                x.Container.Register(new List<IRegistry>
                {
                    new SampleAppRegistry()
                });
                ControllerBuilder.Current.SetControllerFactory(new IocControllerFactory(x.Container));
            });
        }
    }

    public class SampleAppRegistry : Registry
    {
        public override void Load()
        {
            For<IDbContext>().Use<SampleAppDbContext>();
        }
    }
}