using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MenuGen.Core.Attributes;

namespace MenuGen.Core
{
    public class MenuEngine : IMenuEngine
    {
        private readonly ICollection<MenuModel> _menus = new List<MenuModel>();

        public IEnumerable<MenuModel> CreateMenus(IEnumerable<Type> menus, IEnumerable<Type> controllers)
        {
            if (menus == null)
                throw new ArgumentException("Menus cannot be null.");

            if (controllers == null)
                throw new ArgumentException("Controllers cannot be null.");

            var menuNodes = GenerateMenuModelsFromControllerActions(controllers).ToList();

            var lookup = new Dictionary<string, MenuNodeModel>();
            menuNodes.ForEach(x => lookup.Add(x.Key, x));

            var menuNodeTrees = BuildMenuNodeTrees(lookup);

            foreach (var menuNodeModel in menuNodeTrees.Where(x => x.MenuNames != null))
            {
                foreach (var menuName in menuNodeModel.MenuNames)
                {
                    var menu = _menus.FirstOrDefault(x => x.Name == menuName);
                    if (menu == null)
                    {
                        menu = new MenuModel
                        {
                            MenuNodes = new List<MenuNodeModel>(),
                            Name = menuName
                        };
                        _menus.Add(menu);
                    }
                    menu.MenuNodes.Add(menuNodeModel);
                }
            }

            //TODO: need to order menu nodes

            return _menus;
        }

        /// <summary>
        /// Iterates through MenuNodes in a dictionary and creates a hierarchy using the menu name in the dictionary and the MenuNode's ParentKey.
        /// </summary>
        /// <param name="lookup">A dictionary of the menu name, and a menu node model that belongs to that menu.</param>
        /// <returns></returns>
        private static IEnumerable<MenuNodeModel> BuildMenuNodeTrees(Dictionary<string, MenuNodeModel> lookup)
        {
            //http://stackoverflow.com/a/444303/1647062

            var finalList = new List<MenuNodeModel>();

            foreach (var menuNodeModel in lookup.Values)
            {
                MenuNodeModel parentNode;
                if (string.IsNullOrEmpty(menuNodeModel.ParentKey) ||
                    !lookup.TryGetValue(menuNodeModel.ParentKey, out parentNode))
                {
                    finalList.Add(menuNodeModel);
                    continue;
                }

                if (parentNode.ChildMenu == null)
                {
                    parentNode.ChildMenu = new MenuModel
                       {
                           MenuNodes = new List<MenuNodeModel>()
                       };
                }

                parentNode.ChildMenu.MenuNodes.Add(menuNodeModel);
            }
            return finalList;
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
                AreaName = attribute.AreaName,
                ControllerName = controllerName.ToLower().Replace("controller", ""),
                ActionName = action.Name,
                Order = attribute.Order,
                Text = attribute.Text,
                Clickable = attribute.Clickable,
                ChildMenu = new MenuModel
                {
                    MenuNodes = new List<MenuNodeModel>()
                },
                Key = attribute.Key,
                ParentKey = attribute.ParentKey,
                MenuNames = attribute.Menus != null ? new List<string>(attribute.Menus.Select(x => x.Name)) : null
            };

            return menuNodeModel;
        }

        /// <summary>
        /// Returns a MethodInfo for each method that has a return type of ActionResult and is marked with the MenuNodeAttribute.
        /// </summary>
        private IEnumerable<MethodInfo> GetActionsForController(Type controller)
        {
            if (controller == null)
                return null;

            return controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(action => action.ReturnType == typeof(ActionResult) && action.GetCustomAttributes(typeof(MenuNodeAttribute), false).Length > 0);
        }

        private IEnumerable<MenuNodeModel> GenerateMenuModelsFromControllerActions(IEnumerable<Type> controllers)
        {
            var menuNodes = new List<MenuNodeModel>();
            foreach (var controller in controllers)
            {
                var actions = GetActionsForController(controller).ToList();
                actions.ForEach(x =>
                {
                    var menuNode = CreateMenuNode(controller.Name, x);
                    menuNodes.Add(menuNode);
                });
            }
            return menuNodes;
        }
    }
}
