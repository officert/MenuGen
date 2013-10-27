using System;
using MenuGen.MenuNodeGenerators;

namespace MenuGen
{
    public abstract class MenuBase<TNodeGenerator> where TNodeGenerator : IMenuNodeGenerator
    {
        public Type MenuGeneratorType
        {
            get { return typeof(IMenuNodeGenerator); }
        }
    }
}
