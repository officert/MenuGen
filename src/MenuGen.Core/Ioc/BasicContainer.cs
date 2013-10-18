using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using MenuGen.Extensions;

namespace MenuGen.Ioc
{
    public class BasicContainer : IContainer
    {
        private readonly ICollection<DependencyMap> _dependencyMaps;

        public BasicContainer()
        {
            _dependencyMaps = new Collection<DependencyMap>();
        }

        //public void Register<T>(T type) where T : Type
        //{
        //    _typeDictionary.Add(type.Name, type);
        //}

        //public T GetInstance<T>() where T : class
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// If a dependency map has already been registered for type, this will return an instance of the passed type with all of its dependencies.
        /// </summary>
        public object GetInstance(Type type)
        {
            var mapForType = FindDependencyMaps(type).FirstOrDefault();

            if (mapForType == null)
            {
                var newMap = new DependencyMap();

                if (type.IsInterface)
                    throw new InvalidOperationException(string.Format("No map for interface type '{0}' exists. You must register a map with a concrete implementation to inject this interface.", type));

                newMap.ConcreteType = type;

                //_dependencyMaps.Add(newMap); //TODO: for now don't hold onto to new maps that weren't registered on startup - need to figure out when/how to release objects
                mapForType = newMap;
            }

            if (mapForType.Instance != null) return mapForType.Instance;

            var constructors = type.GetConstructors();
            var ctor = constructors.FirstOrDefault();

            if (mapForType.ConcreteType.HasADefaultConstructor() || ctor == null) //TODO: should no constructor just create the instance, since there are no dependencies to resolve??
            {
                mapForType.Instance = Activator.CreateInstance(mapForType.ConcreteType);

                return mapForType.Instance;
            }

            var constructorArgs = ctor.GetParameters().ToList();
            var argObjs = new List<object>();

            foreach (var constructorArg in constructorArgs)
            {
                argObjs.Add(GetInstance(constructorArg.ParameterType));
            }
            mapForType.Instance = Activator.CreateInstance(mapForType.ConcreteType, argObjs.ToArray());

            return mapForType.Instance;
        }

        public IEnumerable<object> GetInstances(Type type)
        {
            var maps = FindDependencyMaps(type).ToList();

            if (maps == null || !maps.Any()) throw new InvalidOperationException(string.Format("no mapping found for type '{0}'", type));

            return maps;
        }

        public DependencyMap For<T>() where T : class
        {
            var map = new DependencyMap
            {
                AbstractType = typeof(T)
            };

            _dependencyMaps.Add(map);

            return map;
        }

        private IEnumerable<DependencyMap> FindDependencyMaps(Type type)
        {
            if (type == null) return null;

            if (type.IsInterface)
            {
                return _dependencyMaps.Where(x => x.AbstractType == type);
            }

            return _dependencyMaps.Where(x => x.ConcreteType == type);
        }
    }

    public static class TypeExtensions
    {
        public static bool HasADefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}
