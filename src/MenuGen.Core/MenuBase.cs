using MenuGen.MenuNodeGenerators;

namespace MenuGen
{
    public abstract class MenuBase<TNodeGenerator> where TNodeGenerator : IMenuNodeGenerator
    {
    }
}
