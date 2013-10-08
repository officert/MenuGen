using System.Collections.Generic;
using System.Linq;
using MenuGen.MenuNodeTreeGenerators;
using MenuGen.Models;

namespace MenuGen.SampleApp.Menus
{
    public class SidebarNav : MenuBase<SqlMenuGenerator>
    {
    }

    public class SqlMenuGenerator : MenuNodeTreeGeneratorBase
    {
        private readonly IMenuNodeTreeBuilder _menuNodeTreeBuilder;

        public SqlMenuGenerator(IMenuNodeTreeBuilder menuNodeTreeBuilder) : base(menuNodeTreeBuilder)
        {
            _menuNodeTreeBuilder = menuNodeTreeBuilder;
        }

        public override IEnumerable<MenuNodeModel> GenerateMenuTrees()
        {
            var nodes = new List<MenuNodeModel>
            {
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Home",
                    IsActive = true,
                    Clickable = true,
                    Key = "Home",
                    Text = "Home"
                },
                new MenuNodeModel
                {
                    ActionName = "About",
                    ControllerName = "Home",
                    IsActive = true,
                    Clickable = true,
                    Key = "About",
                    ParentKey = "Home",
                    Text = "About"
                },
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Employees",
                    IsActive = true,
                    Clickable = true,
                    Key = "Employees",
                    ParentKey = "Home",
                    Text = "Employees"
                }
            };
            return _menuNodeTreeBuilder.BuildMenuNodeTrees(nodes.ToDictionary(x => x.Key));
        }
    }
}