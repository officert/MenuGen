using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuGen.Ioc
{
    public class BasicContainer : IContainer
    {
        private readonly Dictionary<string, Type> _typeDictionary;

        public BasicContainer()
        {
            _typeDictionary = new Dictionary<string, Type>();
        }

        public T Resolve<T>(T type) where T : Type, new()
        {
            if (_typeDictionary.ContainsValue(type))
            {
                return (T)Activator.CreateInstance(_typeDictionary.First(x => x.Value == type).Value);
            }
            //return (T)Activator.CreateInstance(type);
            return Activator.CreateInstance<T>();
        }

        public void Register<T>(T type) where T : Type, new()
        {
            _typeDictionary.Add(type.Name, type);
        }
    }
}
