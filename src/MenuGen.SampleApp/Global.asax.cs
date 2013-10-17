using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MenuGen.Ioc;
using MenuGen.MenuNodeGenerators;
using MenuGen.SampleApp.App_Start;
using MenuGen.SampleApp.MenuGenerators;

namespace MenuGen.SampleApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

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
                //x.ContainerAdapter = new NinjectContainerAdapter();
                x.Container.For<IMenuNodeGenerator>().Use<EmployeesMenuGenerator>().Named("EmployeeGenerator");
            });
        }
    }

    public class NinjectContainerAdapter : IContainerAdapter
    {
        public object GetInstance(System.Type type)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<object> GetInstances(System.Type type)
        {
            throw new System.NotImplementedException();
        }
    }
}