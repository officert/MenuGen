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
            var menuNodes = GenerateMenuNodes();

            if (menuNodes == null || !menuNodes.Any()) return null;

            var lookup = menuNodes.ToDictionary(x => x.Key);

            return _menuNodeTreeBuilder.BuildMenuNodeTrees(lookup);
        }

        public virtual IEnumerable<MenuNodeModel> GenerateMenuNodes()
        {
            throw new NotImplementedException();
        }
    }
}
