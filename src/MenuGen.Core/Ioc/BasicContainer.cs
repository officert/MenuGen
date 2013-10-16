using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MenuGen.Extensions;

namespace MenuGen.Ioc
{
    public class BasicContainer : IContainer
    {
        private readonly Dictionary<string, Type> _typeDictionary;

        public BasicContainer()
        {
            _typeDictionary = new Dictionary<string, Type>();
        }

        public void Register<T>(T type) where T : Type
        {
            _typeDictionary.Add(type.Name, type);
        }

        public T GetInstance<T>(Type type) where T : class
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor != null) return Activator.CreateInstance(type).Cast<T>();

            var constructors = type.GetConstructors(BindingFlags.Public);
            foreach (var constructorInfo in constructors)
            {
                var constructorArgs = constructorInfo.GetParameters().ToList();
            }
            return null;
        }

        public IEnumerable<T> GetInstances<T>(Type type) where T : class
        {
            throw new NotImplementedException();
        }

        public ContainerMapping For<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
