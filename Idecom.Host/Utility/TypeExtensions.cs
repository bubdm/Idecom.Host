namespace Idecom.Host.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class TypeExtensions
    {
        public static IEnumerable<Type> BasedOn<T>(this IEnumerable<Type> types) where T: class
        {
            var baseType = typeof (T);
            return types.Where(type => type.IsSubclassOf(baseType));
        }
        public static IEnumerable<Type> Implementing<T>(this IEnumerable<Type> types)
        {
            var interfaceType = typeof (T);
            return types.Where(type => type.GetInterfaces().Contains(interfaceType));
        }

        public static T FirstImplementingInstance<T>(this IEnumerable<Type> types)
        {
            var interfaceType = typeof (T);
            Type firstImplementingInstance = types.FirstOrDefault(type => type.GetInterfaces().Contains(interfaceType));
            if (firstImplementingInstance != null) return (T)Activator.CreateInstance(firstImplementingInstance);
            return default(T);
        }
    }
}