using System;

namespace MenuGen.Ioc
{
    public class DependencyMap
    {
        public Type ConcreteType { get; set; }

        public Type AbstractType { get; set; }

        public string Name { get; set; }

        public object Instance { get; set; }

        public DependencyMap Use<T>() where T : class
        {
            ConcreteType = typeof(T);

            return this;
        }

        public DependencyMap Use<T>(T instance) where T : class 
        {
            ConcreteType = typeof(T);
            Instance = instance;

            return this;
        }

        public void Named(string name)
        {
            Name = name;
        }
    }
}
