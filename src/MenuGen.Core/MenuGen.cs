using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MenuGen.Extensions;
using MenuGen.Ioc;
using MenuGen.MenuNodeGenerators;
using MenuGen.Models;

namespace MenuGen
{
    public class MenuGen
    {
        private BasicContainer _container;
        private IContainerAdapter _containerAdapter;
        private static readonly IMenuNodeTreeBuilder MenuNodeTreeBuilder = new MenuNodeTreeBuilder();

        private static readonly ICollection<MenuModel> Menus = new List<MenuModel>();

        public static MenuModel GetMenu(string menuName)
        {
            return Menus.FirstOrDefault(x => x.Name == menuName);
        }

        public void Init(Action<MenuGenSetup> setup)
        {
            _container = new BasicContainer();
            _containerAdapter = new InternalContainerAdapter(_container);

            var menuGenOptions = new MenuGenSetup
            {
                Container = _container,
                ContainerAdapter = _containerAdapter
            };

            //setup(menuGenOptions);

            //_container.For<IMenuNodeGenerator>().Use<ReflectionMenuNodeGenerator>();
            //TODO: register an XML node generator

            var assembly = Assembly.GetCallingAssembly();

            var menuImpls = GetSubClassesOfGenericType<MenuBase<IMenuNodeGenerator>>(assembly.GetTypes());

            foreach (var menuImpl in menuImpls)
            {
                var menuGeneratorType = GetMenuGeneratorType(menuImpl);

                //var menuNodeGenerator = _containerAdapter.GetInstance<IMenuNodeGenerator>(menuGeneratorType);

                IMenuNodeGenerator menuNodeGenerator;

                var constructor = menuGeneratorType.GetConstructors().FirstOrDefault();
                List<ParameterInfo> constructorArgs = null;

                if (constructor != null)
                {
                    constructorArgs = constructor.GetParameters().ToList();
                }

                //TODO: creation of MenuGenerator instances should be delegated to an IOC container - a lightweight internal one
                //TODO: that can be swapped out for whatever IOC the user wants to use. That way a user can have their IOC inject any
                //TODO: other instances that their MenuGenerator needs to generate menu nodes - (ie. some data access service, etc...)

                if (constructorArgs != null && constructorArgs.Count > 1)
                {
                    menuNodeGenerator = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder, assembly).Cast<IMenuNodeGenerator>();
                }
                else
                {
                    menuNodeGenerator = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder).Cast<IMenuNodeGenerator>();
                }

                var menu = new MenuModel
                {
                    Name = menuImpl.Name,
                    MenuNodes = menuNodeGenerator.BuildMenuNodeTrees().ToList()
                };

                Menus.Add(menu);
            }
        }

        #region Private Helpers

        private static IEnumerable<Type> GetSubClassesOfGenericType<T>(IEnumerable<Type> types) where T : class 
        {
            return types.Where(IsSubTypeOf<T>);
        }

        private static bool IsSubTypeOf<T>(Type typeToCheckFrom) where T : class
        {
            while (true)
            {
                var typeToCheckAgainst = typeof (T);

                if (typeToCheckFrom.IsGenericType && typeToCheckFrom.GetGenericTypeDefinition() == typeToCheckAgainst.GetGenericTypeDefinition())
                {
                    return true;
                }
                if (typeToCheckFrom.BaseType != null)
                {
                    typeToCheckFrom = typeToCheckFrom.BaseType;
                    continue;
                }

                return false;
                break;
            }
        }

        private static Type GetMenuGeneratorType(Type menuImpl)
        {
            return menuImpl.BaseType.GetGenericArguments().FirstOrDefault();
        }

        #endregion
    }

    public class InternalContainerAdapter : IContainerAdapter
    {
        private readonly IContainer _container;

        public InternalContainerAdapter(IContainer container)
        {
            _container = container;
        }

        public T GetInstance<T>(Type type) where T : class
        {
            return _container.GetInstance<T>(type);
        }

        public IEnumerable<T> GetInstances<T>(Type type) where T : class
        {
            return _container.GetInstances<T>(type);
        }

        public void Register<T>(Type type) where T : class
        {
            throw new NotImplementedException();
        }

        public ContainerMapping For<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }

    public class MenuGenSetup
    {
        public IContainer Container { get; set; }
        public IContainerAdapter ContainerAdapter { get; set; }
    }
}
