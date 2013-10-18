using System;
using System.Collections.Generic;
using System.Linq;
using MenuGen.Models;

namespace MenuGen.MenuNodeGenerators
{
    public abstract class MenuNodeGeneratorBase : IMenuNodeGenerator
    {
        protected IEnumerable<MenuNodeModel> MenuNodes { get; set; }
        private readonly IMenuNodeTreeBuilder _menuNodeTreeBuilder;

        protected MenuNodeGeneratorBase(IMenuNodeTreeBuilder menuNodeTreeBuilder)
        {
            _menuNodeTreeBuilder = menuNodeTreeBuilder;
        }

        public IEnumerable<MenuNodeModel> BuildMenuNodeTrees()
        {
            return _menuNodeTreeBuilder.BuildMenuNodeTrees(GenerateMenuNodes());
        }

        public virtual IEnumerable<MenuNodeModel> GenerateMenuNodes()
        {
            throw new NotImplementedException("Subclasses of MenuNodeGeneratorBase must provide their own implementation of GenerateMenuNodes");
        }
    }
}
