using System;
using System.Collections.Generic;
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

        public IEnumerable<MenuNodeModel> BuildMenuNodeTrees(string menuName = null)
        {
            return string.IsNullOrEmpty(menuName)
                ? _menuNodeTreeBuilder.BuildMenuNodeTrees(GenerateMenuNodes())
                : _menuNodeTreeBuilder.BuildMenuNodeTrees(GenerateMenuNodes(menuName));
        }

        public virtual IEnumerable<MenuNodeModel> GenerateMenuNodes()
        {
            throw new NotImplementedException("Subclasses of MenuNodeGeneratorBase must provide their own implementation of GenerateMenuNodes");
        }

        public virtual IEnumerable<MenuNodeModel> GenerateMenuNodes(string menuName)
        {
            throw new NotImplementedException("Subclasses of MenuNodeGeneratorBase must provide their own implementation of GenerateMenuNodes");
        }
    }
}
