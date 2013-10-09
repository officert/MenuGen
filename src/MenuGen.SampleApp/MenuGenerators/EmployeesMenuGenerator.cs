using System.Collections.Generic;
using MenuGen.MenuNodeGenerators;
using MenuGen.Models;

namespace MenuGen.SampleApp.MenuGenerators
{
    public class EmployeesMenuGenerator : MenuNodeGeneratorBase
    {
        public EmployeesMenuGenerator(IMenuNodeTreeBuilder menuNodeTreeBuilder) : base(menuNodeTreeBuilder)
        {
        }

        public override IEnumerable<MenuNodeModel> GenerateMenuNodes()
        {
            var menuNodes = new List<MenuNodeModel>
            {
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Employees",
                    IsActive = true,
                    Clickable = true,
                    Key = "TomCruise",
                    Text = "Tom Cruise"
                },
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Employees",
                    IsActive = true,
                    Clickable = true,
                    Key = "SandraBullock",
                    Text = "Sandra Bullock"
                },
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Employees",
                    IsActive = true,
                    Clickable = true,
                    Key = "MileyCyrus",
                    Text = "Miley Cyrus"
                },
                new MenuNodeModel
                {
                    ActionName = "Index",
                    ControllerName = "Employees",
                    IsActive = true,
                    Clickable = true,
                    Key = "MorganFreeman",
                    Text = "Morgan Freeman"
                }
            };
            return menuNodes;
        }
    }
}