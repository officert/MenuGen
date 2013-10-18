using System.Collections.Generic;
using System.Linq;
using MenuGen.MenuNodeGenerators;
using MenuGen.Models;
using MenuGen.SampleApp.Data;
using MenuGen.SampleApp.Models;

namespace MenuGen.SampleApp.MenuGenerators
{
    public class ItemsMenuGenerator : MenuNodeGeneratorBase
    {
        private readonly IDbContext _dbContext;

        public ItemsMenuGenerator(IMenuNodeTreeBuilder menuNodeTreeBuilder, IDbContext dbContext) : base(menuNodeTreeBuilder)
        {
            _dbContext = dbContext;
        }

        public override IEnumerable<MenuNodeModel> GenerateMenuNodes()
        {
            var items = _dbContext.GetDbSet<Item>().ToList();

            var menuNodes = new List<MenuNodeModel>();

            foreach (var item in items)
            {
                menuNodes.Add(new MenuNodeModel
                {
                    ControllerName = "store",
                    ActionName = "item",
                    Clickable = true,
                    HtmlAttributes = new { Id = item.Id },
                    Key = string.Format("Item{0}", item.Id),
                    Text = item.Name
                });
            }

            return menuNodes;
        }
    }
}