using System;
using System.Collections.Generic;

namespace MenuGen.Ioc
{
    public interface IContainer
    {
        //T GetInstance<T>() where T : class;
        object GetInstance(Type type);
        IEnumerable<object> GetInstances(Type type) ;
        DependencyMap For<T>() where T : class;
    }
}
