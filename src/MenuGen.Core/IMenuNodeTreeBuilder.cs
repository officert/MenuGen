using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen
{
    public interface IMenuNodeTreeBuilder
    {
        IEnumerable<MenuNodeModel> BuildMenuNodeTrees(Dictionary<string, MenuNodeModel> lookup);
    }
}
