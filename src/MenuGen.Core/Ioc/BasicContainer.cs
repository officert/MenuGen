using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuGen.Ioc
{
    public class BasicContainer : IContainer
    {
        private readonly Dictionary<string, Type> _typeDictionary;
        private IContainerAdapter _containerAdapter;

        public BasicContainer()
        {
            _typeDictionary = new Dictionary<string, Type>();
        }

        public T Resolve<T>(T type) where T : Type
        {
            if (_containerAdapter != null)
            {
                return _containerAdapter.Resolve(type);
            }
            else
            {
                if (_typeDictionary.ContainsValue(type))
                {
                    return (T)Activator.CreateInstance(_typeDictionary.First(x => x.Value == type).Value);
                }
                //return (T)Activator.CreateInstance(type);
                throw new ArgumentException("");    //throw a different exception - TODO : create a registry not found exception or something
            }
        }

        public void Register<T>(T type) where T : Type
        {
            if (_containerAdapter != null)
            {
                _containerAdapter.Register(type);
                return;
            }

            _typeDictionary.Add(type.Name, type);
        }

        public IContainerAdapter ContainerAdapter
        {
            get
            {
                return _containerAdapter;
            }
            set
            {
                _containerAdapter = value;
            }
        }
    }
}
