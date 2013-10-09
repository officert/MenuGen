using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen.MenuNodeGenerators
{
    public interface IMenuNodeGenerator
    {
        IEnumerable<MenuNodeModel> BuildMenuNodeTrees();
        IEnumerable<MenuNodeModel> GenerateMenuNodes();
    }
}
