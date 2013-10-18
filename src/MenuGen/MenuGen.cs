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

        public void Init()
        {
            var assembly = Assembly.GetCallingAssembly();

            _container = new BasicContainer();
            _containerAdapter = new InternalContainerAdapter(_container);

            _init(assembly);
        }

        public void Init(Action<MenuGenSetup> setup)
        {
            var assembly = Assembly.GetCallingAssembly();

            _container = new BasicContainer();
            _containerAdapter = new InternalContainerAdapter(_container);

            var menuGenOptions = new MenuGenSetup
            {
                Container = _container,
                ContainerAdapter = _containerAdapter
            };

            setup(menuGenOptions);

            if (menuGenOptions.ContainerAdapter != null && menuGenOptions.ContainerAdapter != _containerAdapter)
                _containerAdapter = menuGenOptions.ContainerAdapter;    //if they provide an adapter, use it

            _init(assembly);
        }

        private void _init(Assembly assembly)
        {
            RegisterDependencies(assembly);

            //TODO: register an XML node generator

            GenerateMenus(assembly);
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
                var typeToCheckAgainst = typeof(T);

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

        private void GenerateMenus(Assembly assembly)
        {
            var menuImpls = GetSubClassesOfGenericType<MenuBase<IMenuNodeGenerator>>(assembly.GetTypes());

            foreach (var menuImpl in menuImpls)
            {
                var menuGeneratorType = GetMenuGeneratorType(menuImpl);

                //TODO: for the reflection generator need to figure out how to pass it the MenuName,
                //TODO: that way it knows which Actions to create menu nodes for
                var menuNodeGenerator = _containerAdapter.GetInstance(menuGeneratorType).Cast<IMenuNodeGenerator>();

                //IMenuNodeGenerator menuNodeGenerator;

                //var constructor = menuGeneratorType.GetConstructors().FirstOrDefault();
                //List<ParameterInfo> constructorArgs = null;

                //if (constructor != null)
                //{
                //    constructorArgs = constructor.GetParameters().ToList();
                //}

                ////TODO: creation of MenuGenerator instances should be delegated to an IOC container - a lightweight internal one
                ////TODO: that can be swapped out for whatever IOC the user wants to use. That way a user can have their IOC inject any
                ////TODO: other instances that their MenuGenerator needs to generate menu nodes - (ie. some data access service, etc...)

                //if (constructorArgs != null && constructorArgs.Count > 1)
                //{
                //    menuNodeGenerator = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder, assembly).Cast<IMenuNodeGenerator>();
                //}
                //else
                //{
                //    menuNodeGenerator = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder).Cast<IMenuNodeGenerator>();
                //}

                var nodeTrees = menuNodeGenerator.BuildMenuNodeTrees();

                var menu = new MenuModel
                {
                    Name = menuImpl.Name,
                    MenuNodes = nodeTrees == null ? null : nodeTrees.ToList()
                };

                Menus.Add(menu);
            }
        }

        private void RegisterDependencies(Assembly assembly)
        {
            _container.For<IMenuNodeTreeBuilder>().Use<MenuNodeTreeBuilder>();

            _container.For<IMenuNodeGenerator>().Use<ReflectionMenuNodeGenerator>();

            _container.For<Assembly>().Use(assembly);
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

        public object GetInstance(Type type)
        {
            return _container.GetInstance(type);
        }

        public IEnumerable<object> GetInstances(Type type)
        {
            return _container.GetInstances(type);
        }

        public DependencyMap For<T>() where T : class
        {
            return _container.For<T>();
        }
    }

    public class MenuGenSetup
    {
        public IContainer Container { get; set; }
        public IContainerAdapter ContainerAdapter { get; set; }
    }
}
