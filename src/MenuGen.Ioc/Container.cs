using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MenuGen.Ioc.Extensions;

namespace MenuGen.Ioc
{
    public class Container : IContainer
    {
        private readonly ICollection<IBinding> _bindings;

        public Container()
        {
            _bindings = new Collection<IBinding>();
        }

        public DependencyMap<TAbstractType> For<TAbstractType>() 
        {
            return new DependencyMap<TAbstractType>(this);
        }

        public object Resolve(Type type)
        {
            return CreateObjectGraph(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            var maps = FindBindings(type).ToList();

            if (maps == null || !maps.Any()) throw new InvalidOperationException(string.Format("no mapping found for type '{0}'", type));

            var instances = new List<object>();

            foreach (var map in maps)
            {
                instances.Add(CreateObjectGraph(map.ConcreteType));
            }

            return instances;
        }

        public void Release(Type type)
        {
            var binding = FindBindings(type).FirstOrDefault();

            if (binding == null) return;

            binding.Dispose();
        }

        public void AddBinding(IBinding binding)
        {
            if (binding == null) throw new ArgumentException("binding");

            _bindings.Add(binding);
        }

        private IEnumerable<IBinding> FindBindings(Type type)
        {
            if (type == null) return null;

            if (type.IsInterface)
            {
                return _bindings.Where(x => x.AbstractType == type);
            }

            return _bindings.Where(x => x.ConcreteType == type);
        }

        private object CreateObjectGraph(Type type)
        {
            var binding = FindBindings(type).FirstOrDefault();

            if (binding == null)
            {
                if (type.IsAbstract || type.IsInterface)
                    throw new InvalidOperationException(string.Format("No map for abstract type '{0}' exists. You must register a map with a concrete implementation to inject this interface.", type));

                var newBinding = new Binding(type, type);

                //_bindings.Add(newBinding); //TODO: for now don't hold onto to new maps that weren't registered on startup - need to figure out when/how to release objects
                binding = newBinding;
            }

            var constructors = type.GetConstructors();
            var ctor = constructors.FirstOrDefault();

            if (binding.Instance != null) return binding.Instance;

            if (binding.ConcreteType.HasADefaultConstructor() || ctor == null) //TODO: should no constructor just create the instance, since there are no dependencies to resolve??
            {
                return Activator.CreateInstance(binding.ConcreteType);
            }

            var constructorArgs = ctor.GetParameters().ToList();
            var argObjs = new List<object>();

            foreach (var constructorArg in constructorArgs)
            {
                argObjs.Add(Resolve(constructorArg.ParameterType));
            }
            return Activator.CreateInstance(binding.ConcreteType, argObjs.ToArray());
        }
    }
}
