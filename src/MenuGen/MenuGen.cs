using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IocLite;
using IocLite.Interfaces;
using MenuGen.Extensions;
using MenuGen.MenuNodeGenerators;
using MenuGen.Models;

namespace MenuGen
{
    public static class MenuGen
    {
        private static IContainer _container = new Container();
        private static IContainerAdapter _containerAdapter;
        private static Assembly _currentAssembly;

        private static readonly ICollection<MenuModel> Menus = new List<MenuModel>();

        public static MenuModel GetMenu(string menuName)
        {
            return Menus.FirstOrDefault(x => x.Name == menuName);
        }

        /// <summary>
        /// Clears any existing cached menus and regenerates them using their appropriate MenuGenerators
        /// </summary>
        public static void ClearMenuCache()
        {
            Menus.Clear();

            GenerateMenus(_currentAssembly);
        }

        public static void Init()
        {
            var assembly = Assembly.GetCallingAssembly();

            _init(assembly);
        }

        public static void Init(Action<MenuGenConfiguration> configuration)
        {
            Ensure.ArgumentIsNotNull(configuration, "configuration");

            _currentAssembly = Assembly.GetCallingAssembly();

            _container.Register(new List<IRegistry>
            {
                new MenuGenRegistry(_currentAssembly)
            });

            var menuGenOptions = new MenuGenConfiguration
            {
                Container = _container
            };

            configuration(menuGenOptions);

            _containerAdapter = menuGenOptions.ContainerAdapter;    //if they provide an adapter, use it

            _init(_currentAssembly);
        }

        private static void _init(Assembly assembly)
        {
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

        private static void GenerateMenus(Assembly assembly)
        {
            var menuImpls = GetSubClassesOfGenericType<MenuBase<IMenuNodeGenerator>>(assembly.GetTypes());

            foreach (var menuImpl in menuImpls)
            {
                var menuGeneratorType = GetMenuGeneratorType(menuImpl);

                //TODO: for the reflection generator need to figure out how to pass it the MenuName,
                //TODO: that way it knows which Actions to create menu nodes for

                var menuNodeGenerator = _container.TryResolve(menuGeneratorType).Cast<IMenuNodeGenerator>();

                if (menuNodeGenerator == null && _containerAdapter != null) menuNodeGenerator = _containerAdapter.TryResolve(menuGeneratorType).Cast<IMenuNodeGenerator>();

                if(menuNodeGenerator == null) throw new InvalidOperationException("asdfs fasdfasdf");

                var nodeTrees = menuNodeGenerator.BuildMenuNodeTrees();

                var menu = new MenuModel
                {
                    Name = menuImpl.Name,
                    MenuNodes = nodeTrees == null ? null : nodeTrees.ToList()
                };

                Menus.Add(menu);
            }
        }

        #endregion
    }

    public class MenuGenConfiguration
    {
        public IContainer Container { get; internal set; }
        public IContainerAdapter ContainerAdapter { get; set; }
    }

    public class MenuGenRegistry : Registry
    {
        private readonly Assembly _currentAssembly;

        public MenuGenRegistry(Assembly currentAssembly)
        {
            _currentAssembly = currentAssembly;
        }

        public override void Load()
        {
            For<IMenuNodeTreeBuilder>().Use<MenuNodeTreeBuilder>();

            For<IMenuNodeGenerator>().Use<ReflectionMenuNodeGenerator>();

            For<Assembly>().Use(_currentAssembly);
        }
    }
}
