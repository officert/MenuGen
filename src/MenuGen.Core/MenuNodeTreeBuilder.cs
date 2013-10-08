using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen
{
    public class MenuNodeTreeBuilder : IMenuNodeTreeBuilder
    {
        public IEnumerable<MenuNodeModel> BuildMenuNodeTrees(Dictionary<string, MenuNodeModel> lookup)
        {
            //http://stackoverflow.com/a/444303/1647062

            var finalList = new List<MenuNodeModel>();

            foreach (var menuNodeModel in lookup.Values)
            {
                MenuNodeModel parentNodeModel;
                if (string.IsNullOrEmpty(menuNodeModel.ParentKey) ||
                    !lookup.TryGetValue(menuNodeModel.ParentKey, out parentNodeModel))
                {
                    finalList.Add(menuNodeModel);
                    continue;
                }

                if (parentNodeModel.ChildNodes == null)
                {
                    parentNodeModel.ChildNodes = new List<MenuNodeModel>();
                }

                parentNodeModel.ChildNodes.Add(menuNodeModel);
            }
            return finalList;
        }
    }
}
