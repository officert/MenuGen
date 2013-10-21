using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MenuGen.Ioc;
using MenuGen.MenuNodeGenerators;
using MenuGen.SampleApp.App_Start;
using MenuGen.SampleApp.Data;
using MenuGen.SampleApp.Ioc;
using MenuGen.SampleApp.MenuGenerators;

namespace MenuGen.SampleApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private MenuGen _menuGen;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _menuGen = new MenuGen();
            _menuGen.Init(x =>
            {
                x.Container.For<IMenuNodeGenerator>().Use<ItemsMenuGenerator>();
                x.Container.For<IDbContext>().Use<SampleAppDbContext>();

                ControllerBuilder.Current.SetControllerFactory(new IocControllerFactory(x.Container)); 
            });
        }
    }

    public class NinjectContainerAdapter : IContainerAdapter
    {
        public object GetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetInstances(Type type)
        {
            throw new NotImplementedException();
        }
    }
}