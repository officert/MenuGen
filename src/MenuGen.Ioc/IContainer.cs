using System;
using System.Collections.Generic;

namespace MenuGen.Ioc
{
    public interface IContainer
    {
        DependencyMap<TAbstractType> For<TAbstractType>();

        object Resolve(Type type);
        IEnumerable<object> ResolveAll(Type type);

        void Release(Type type);

        void AddBinding(IBinding binding);
    }
}
