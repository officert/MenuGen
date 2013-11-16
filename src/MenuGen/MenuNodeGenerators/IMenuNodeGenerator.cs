using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen.MenuNodeGenerators
{
    public interface IMenuNodeGenerator
    {
        IEnumerable<MenuNodeModel> BuildMenuNodeTrees(string menuName = null);
        IEnumerable<MenuNodeModel> GenerateMenuNodes();
        IEnumerable<MenuNodeModel> GenerateMenuNodes(string menuName);
    }
}
