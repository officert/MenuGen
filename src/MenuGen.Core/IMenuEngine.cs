using System;
using System.Collections.Generic;

namespace MenuGen.Core
{
    public interface IMenuEngine
    {
        IEnumerable<MenuModel> CreateMenus(IEnumerable<Type> menus, IEnumerable<Type> controllers);
    }
}
