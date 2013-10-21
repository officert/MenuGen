using System;
using System.Collections.Generic;

namespace MenuGen.Ioc
{
    public interface IContainerAdapter
    {
        object GetInstance(Type type);
        IEnumerable<object> GetInstances(Type type);
    }
}
