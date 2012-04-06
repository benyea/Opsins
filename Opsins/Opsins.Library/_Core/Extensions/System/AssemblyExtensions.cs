using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Opsins
{
    /// <summary>
    /// Assembly扩展
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 获取程序集的公共类型
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetPublicTypes(this Assembly instance)
        {
            IEnumerable<Type> types = null;

            if (instance != null)
            {
                try
                {
                    types = instance.GetTypes().Where(type => (type != null) && type.IsPublic && type.IsVisible).ToList();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
            }

            return types ?? Enumerable.Empty<Type>();
        }

        /// <summary>
        /// 获取程序集的公共类型
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetPublicTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.GetPublicTypes());
        }

        /// <summary>
        /// 获取程序集的复合类型[类、接口、泛型等]
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetConcreteTypes(this Assembly instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.GetPublicTypes()
                           .Where(type => (type != null) && type.IsClass && !type.IsAbstract && !type.IsInterface && !type.IsGenericType).ToList();
        }

        /// <summary>
        /// 获取程序集的复合类型[类、接口、泛型等]
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetConcreteTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.GetConcreteTypes());
        }
    }
}
