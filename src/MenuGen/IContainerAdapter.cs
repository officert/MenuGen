using System;

namespace MenuGen
{
    public interface IContainerAdapter
    {
        object TryResolve(Type type);
    }
}
