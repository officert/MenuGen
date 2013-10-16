using System;
using System.Collections.Generic;

namespace MenuGen.Ioc
{
    public interface IContainerAdapter
    {
        T GetInstance<T>(Type type) where T : class;
        IEnumerable<T> GetInstances<T>(Type type) where T : class;
    }
}
