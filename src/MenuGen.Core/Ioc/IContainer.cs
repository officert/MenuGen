using System;
using System.Collections.Generic;

namespace MenuGen.Ioc
{
    public interface IContainer
    {
        T GetInstance<T>(Type type) where T : class;
        IEnumerable<T> GetInstances<T>(Type type) where T : class;
        ContainerMapping For<T>() where T : class;
    }

    public class ContainerMapping
    {
        public ContainerMappingOptions Use<T>() where T : class
        {
            return new ContainerMappingOptions();
        }
    }

    public class ContainerMappingOptions
    {
        public string Named(string name)
        {
            return "";
        }
    }
}
