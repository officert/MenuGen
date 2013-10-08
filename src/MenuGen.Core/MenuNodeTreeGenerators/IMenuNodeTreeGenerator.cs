using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen.MenuNodeTreeGenerators
{
    public interface IMenuNodeTreeGenerator
    {
        IEnumerable<MenuNodeModel> GenerateMenuTrees();
    }
}
