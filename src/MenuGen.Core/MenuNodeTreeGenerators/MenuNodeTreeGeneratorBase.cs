using System;
using System.Collections.Generic;
using MenuGen.Models;

namespace MenuGen.MenuNodeTreeGenerators
{
    public abstract class MenuNodeTreeGeneratorBase : IMenuNodeTreeGenerator
    {
        protected MenuNodeTreeGeneratorBase(IMenuNodeTreeBuilder menuNodeTreeBuilder)
        {

        }

        public virtual IEnumerable<MenuNodeModel> GenerateMenuTrees()
        {
            throw new NotImplementedException();
        }
    }
}
