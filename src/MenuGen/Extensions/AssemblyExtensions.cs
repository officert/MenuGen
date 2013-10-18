using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MenuGen.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetSubTypesOf<T>(this Assembly assembly) where T : class
        {
            return assembly.GetTypes().Where(x => IsSubclassOfRawGeneric(typeof(T), x));
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
