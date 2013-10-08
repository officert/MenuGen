﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.Models;

namespace MenuGen.MenuNodeTreeGenerators
{
    public class ReflectionMenuGenerator : MenuNodeTreeGeneratorBase
    {
        private readonly IMenuNodeTreeBuilder _menuNodeTreeBuilder;

        public ReflectionMenuGenerator(IMenuNodeTreeBuilder menuNodeTreeBuilder)
            : base(menuNodeTreeBuilder)
        {
            _menuNodeTreeBuilder = menuNodeTreeBuilder;
        }

        #region MenuNodeTreeGeneratorBase Impl

        public override IEnumerable<MenuNodeModel> GenerateMenuTrees()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var controllers = GetControllers(callingAssembly);

            //TODO: what about duplicate keys??

            //TODO: what about dynamically generated menu links?

            //TODO: can keys contain spaces? is foobar the same as foo bar?

            var lookup = GenerateMenuModelsFromControllerActions(controllers).ToDictionary(x => x.Key);

            //var menuNames = _menuNames.Distinct();

            var menuNodeTrees = _menuNodeTreeBuilder.BuildMenuNodeTrees(lookup);

            //foreach (var menuNodeModel in menuNodeTrees.Where(x => x.MenuNames != null))
            //{
            //    foreach (var menuName in menuNodeModel.MenuNames)
            //    {
            //        var menu = _menus.FirstOrDefault(x => x.Name == menuName);
            //        if (menu == null)
            //        {
            //            menu = new MenuModel
            //            {
            //                MenuNodes = new List<MenuNode>(),
            //                Name = menuName
            //            };
            //            _menus.Add(menu);
            //        }
            //        menu.MenuNodes.Add(menuNodeModel);
            //    }
            //}

            //TODO: need to order menu nodes

            return menuNodeTrees;
        }

        #endregion

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
                AreaName = attribute.AreaName,
                ControllerName = controllerName.ToLower().Replace("controller", ""),
                ActionName = action.Name,
                Order = attribute.Order,
                Text = attribute.Text,
                Clickable = attribute.Clickable,
                ChildNodes = new List<MenuNodeModel>(),
                Key = attribute.Key,
                ParentKey = attribute.ParentKey
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