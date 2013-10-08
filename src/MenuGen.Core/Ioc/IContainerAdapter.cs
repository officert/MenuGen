using System;

namespace MenuGen.Ioc
{
    public interface IContainerAdapter
    {
        T Resolve<T>(T type) where T : Type, new();
        void Register<T>(T type) where T : Type, new();
    }
}
