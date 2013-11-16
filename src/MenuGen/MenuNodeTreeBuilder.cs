using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MenuGen.Models;

namespace MenuGen
{
    public class MenuNodeTreeBuilder : IMenuNodeTreeBuilder
    {
        public IEnumerable<MenuNodeModel> BuildMenuNodeTrees(IEnumerable<MenuNodeModel> menuNodes)
        {
            if (menuNodes == null || !menuNodes.Any()) return null;

            //TODO: need to validate menu nodes for keys, duplicate keys, etc...

            //TODO: what about duplicate keys??

            //TODO: can keys contain spaces? is foobar the same as foo bar?

            //http://stackoverflow.com/a/444303/1647062

            var finalList = new List<MenuNodeModel>();

            if (menuNodes.Any(x => string.IsNullOrEmpty(x.Key))) throw new InvalidOperationException("All Menu Nodes must have a key.");

            var lookup = menuNodes.ToDictionary(x => x.Key);

            foreach (var menuNodeModel in lookup.Values)    
            {
                MenuNodeModel parentNodeModel;
                if (string.IsNullOrEmpty(menuNodeModel.ParentKey) ||
                    !lookup.TryGetValue(menuNodeModel.ParentKey, out parentNodeModel))
                {
                    finalList.Add(menuNodeModel);
                    continue;
                }

                if (parentNodeModel.ChildMenu == null)
                {
                    parentNodeModel.ChildMenu = new MenuModel
                    {
                        MenuNodes = new Collection<MenuNodeModel>()
                    };
                }
                else if (parentNodeModel.ChildMenu.MenuNodes == null)
                {
                    parentNodeModel.ChildMenu.MenuNodes = new Collection<MenuNodeModel>();
                }

                parentNodeModel.ChildMenu.MenuNodes.Add(menuNodeModel);
            }
            return finalList;
        }
    }
}
