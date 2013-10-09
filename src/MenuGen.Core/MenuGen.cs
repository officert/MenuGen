using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MenuGen.Extensions;
using MenuGen.MenuNodeGenerators;
using MenuGen.Models;

namespace MenuGen
{
    public static class MenuGen
    {
        private static readonly Dictionary<string, IMenuNodeGenerator> MenuNodeGenerators = new Dictionary<string, IMenuNodeGenerator>();
        private static readonly IMenuNodeTreeBuilder MenuNodeTreeBuilder = new MenuNodeTreeBuilder();

        private static readonly ICollection<MenuModel> Menus = new List<MenuModel>();

        public static MenuModel GetMenu(string menuName)
        {
            return Menus.FirstOrDefault(x => x.Name == menuName);
        }

        public static void Init()
        {
            var assembly = Assembly.GetCallingAssembly();

            var menuImpls = GetSubClassesOfGenericType(assembly.GetTypes(), typeof(MenuBase<IMenuNodeGenerator>));

            foreach (var menuImpl in menuImpls)
            {
                var menuGeneratorType = GetMenuGeneratorType(menuImpl);
                IMenuNodeGenerator menuNodeTreeGeneratorInstance;

                if (MenuNodeGenerators.ContainsKey(menuGeneratorType.Name))
                {
                    menuNodeTreeGeneratorInstance = MenuNodeGenerators[menuGeneratorType.Name];
                }
                else
                {
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
                        menuNodeTreeGeneratorInstance = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder, assembly).Cast<IMenuNodeGenerator>();
                    }
                    else
                    {
                        menuNodeTreeGeneratorInstance = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder).Cast<IMenuNodeGenerator>();
                    }

                    MenuNodeGenerators.Add(menuGeneratorType.Name, menuNodeTreeGeneratorInstance);
                }

                var menu = new MenuModel
                {
                    Name = menuImpl.Name,
                    MenuNodes = menuNodeTreeGeneratorInstance.BuildMenuNodeTrees().ToList()
                };

                Menus.Add(menu);
            }
        }

        #region Private Helpers

        private static IEnumerable<Type> GetSubClassesOfGenericType(IEnumerable<Type> types, Type typeToCheckAgainst)
        {
            return types.Where(x => IsSubTypeOf(x, typeToCheckAgainst));
        }

        private static bool IsSubTypeOf(Type typeToCheckFrom, Type typeToCheckAgainst)
        {
            if (typeToCheckFrom.IsGenericType
                && typeToCheckFrom.GetGenericTypeDefinition() == typeToCheckAgainst.GetGenericTypeDefinition())
            {
                return true;
            }
            if (typeToCheckFrom.BaseType != null)
            {
                return IsSubTypeOf(typeToCheckFrom.BaseType, typeToCheckAgainst);
            }

            return false;
        }

        private static Type GetMenuGeneratorType(Type menuImpl)
        {
            return menuImpl.BaseType.GetGenericArguments().FirstOrDefault();
        }

        #endregion
    }
}
