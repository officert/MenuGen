using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MenuGen.Extensions;
using MenuGen.MenuNodeTreeGenerators;
using MenuGen.Models;

namespace MenuGen
{
    public static class MenuGen
    {
        private static readonly IList<IMenuNodeTreeGenerator> MenuNodeGenerators = new List<IMenuNodeTreeGenerator>();
        private static readonly IMenuNodeTreeBuilder MenuNodeTreeBuilder = new MenuNodeTreeBuilder();

        private static readonly ICollection<MenuModel> Menus = new List<MenuModel>();

        public static MenuModel GetMenu(string menuName)
        {
            return Menus.FirstOrDefault(x => x.Name == menuName);
        }

        public static void Init()
        {
            var assembly = Assembly.GetCallingAssembly();

            var menuImpls = GetSubClassesOfGenericType(assembly.GetTypes(), typeof(MenuBase<IMenuNodeTreeGenerator>));

            foreach (var menuImpl in menuImpls)
            {
                var menuGeneratorType = GetMenuGeneratorType(menuImpl);

                if (!MenuNodeGenerators.Any(x => x.GetType().Name == menuGeneratorType.Name))
                {
                    var menuNodeTreeGeneratorInstance = Activator.CreateInstance(menuGeneratorType, MenuNodeTreeBuilder).Cast<IMenuNodeTreeGenerator>();

                    var menuNodeTreeMethod = menuGeneratorType.GetMethod("GenerateMenuTrees");  //TODO: figure out a way not to have to hard code the method name

                    var menu = new MenuModel
                    {
                        Name = menuImpl.Name,
                        MenuNodes = (List<MenuNodeModel>)menuNodeTreeMethod.Invoke(menuNodeTreeGeneratorInstance, null)
                    };

                    Menus.Add(menu);

                    //MenuNodeGenerators.Add(menuNodeTreeGeneratorInstance);
                }
            }
        }

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
    }
}
