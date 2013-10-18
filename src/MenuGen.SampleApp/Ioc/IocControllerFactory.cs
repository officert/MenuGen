using System;
using System.Web.Mvc;
using System.Web.Routing;
using MenuGen.Ioc;

namespace MenuGen.SampleApp.Ioc
{
    public class IocControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;
        public IocControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? null
                       : (IController)_container.GetInstance(controllerType);
        }
    }
}