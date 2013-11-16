using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.Models;

namespace MenuGen.MenuNodeGenerators
{
    public class ReflectionMenuNodeGenerator : MenuNodeGeneratorBase
    {
        //TODO: need to support multiple menus using the attribute's Menu property
        //TODO: need to pass in the name of the Menu

        private readonly Assembly _callingAssembly;

        public ReflectionMenuNodeGenerator(IMenuNodeTreeBuilder menuNodeTreeBuilder, Assembly callingAssembly)
            : base(menuNodeTreeBuilder)
        {
            _callingAssembly = callingAssembly;
        }

        public override IEnumerable<MenuNodeModel> GenerateMenuNodes(string menuName)
        {
            var controllers = GetControllers(_callingAssembly);
            var menuNodes = GenerateMenuModelsFromControllerActions(controllers, menuName);

            return menuNodes;
        }

        #region Private Helpers

        private IEnumerable<Type> GetControllers(Assembly assembly)
        {
            return assembly == null ? null : assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Controller)));
        }

        private static MenuNodeModel CreateMenuNode(string controllerName, MethodInfo action)
        {
            if (string.IsNullOrEmpty(controllerName))
                throw new ArgumentException("ControllerName cannot be null or empty.");

            if (action == null)
                throw new ArgumentException("Action cannot be null.");

            var attribute = (MenuNodeAttribute)action.GetCustomAttributes(typeof(MenuNodeAttribute), false).FirstOrDefault();

            if (attribute == null)
                throw new ArgumentException("The Action '{0}' is not decorated with the MenuNodeAttribute.", action.Name);

            if (string.IsNullOrEmpty(attribute.Key))
                throw new ArgumentException(string.Format("All Actions with the MenuNodeAttribute must have a Key. The Action '{0}' does not have a Key.", action.Name));

            var menuNodeModel = new MenuNodeModel
            {
                ControllerName = controllerName.ToLower().Replace("controller", ""),
                ActionName = action.Name,
                Order = attribute.Order,
                Text = attribute.Text,
                Clickable = attribute.Clickable,
                ChildMenu = new MenuModel(),
                Key = attribute.Key,
                ParentKey = attribute.ParentKey,
                RouteValues = attribute.RouteValues,
                HtmlAttributes = attribute.HtmlAttributes
            };

            return menuNodeModel;
        }

        /// <summary>
        /// Returns a MethodInfo for each method that has a return type of ActionResult and is marked with the MenuNodeAttribute.
        /// </summary>
        private IEnumerable<MethodInfo> GetActionsForController(Type controller, string menuName = null)
        {
            if (controller == null)
                return null;

            var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(action => action.ReturnType == typeof(ActionResult)
                                 && action.GetCustomAttributes(typeof(MenuNodeAttribute), false).Length > 0).ToList();

            return string.IsNullOrEmpty(menuName)
                ? actions
                : from action in actions
                  let attribute = (MenuNodeAttribute)action.GetCustomAttributes(typeof(MenuNodeAttribute), false).FirstOrDefault()
                  where attribute != null
                   && attribute.Menus != null
                   && attribute.Menus.Any(x => x.Name == menuName)
                  select action;
        }

        private IEnumerable<MenuNodeModel> GenerateMenuModelsFromControllerActions(IEnumerable<Type> controllers, string menuName)
        {
            var menuNodes = new List<MenuNodeModel>();
            foreach (var controller in controllers)
            {
                var actions = GetActionsForController(controller, menuName).ToList();
                actions.ForEach(x =>
                {
                    var menuNode = CreateMenuNode(controller.Name, x);
                    menuNodes.Add(menuNode);
                });
            }
            return menuNodes;
        }

        #endregion
    }
}
