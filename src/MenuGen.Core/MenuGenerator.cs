using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MenuGen.Core.Attributes;

namespace MenuGen.Core
{
    /// <summary>
    /// Static wrapper for the Menu Engine. Main entry point for MenuGen.
    /// </summary>
    public static class MenuGenerator
    {
        private static IEnumerable<MenuModel> _menus = new List<MenuModel>();
        public static IEnumerable<MenuModel> Menus { get { return _menus; } }

        private static readonly IMenuEngine MenuEngine = new MenuEngine();

        public static void Scan(Assembly assembly)  //TODO: add support for multiple assemblies
        {
            var menus = GetMenus(assembly).ToList();
            if (!menus.Any())
                return;

            var controllers = GetControllers(assembly).ToList();
            if (!controllers.Any())
                return;

            _menus = MenuEngine.CreateMenus(menus, controllers);
        }

        /// <summary>
        /// Returns a Type for each Type in the assembly that subclassess MapperLite.Core.Menu
        /// </summary>
        private static IEnumerable<Type> GetMenus(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(MenuModel)));
        }

        /// <summary>
        /// Returns all types in the assembly that subclass System.Web.Mvc.Controller.
        /// </summary>
        private static IEnumerable<Type> GetControllers(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Controller))).ToList();
        }
    }
}
