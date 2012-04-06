using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Opsins.Infra
{
    /// <summary>
    /// IoC
    /// </summary>
    public static class Ioc
    {
        private static IDependencyResolver _resolver;

        public static void InitialzeWith(IDependencyResolverFactory factory)
        {
            Checker.IsNotNull(factory,"factory");

            _resolver = factory.CreateInstance();
        }

        public static void Register<T>(T instance)
        {
            Checker.IsNotNull(instance,"instance");

            _resolver.Register(instance);
        }

        [DebuggerStepThrough]
        public static void Inject<T>(T existing)
        {
            Checker.IsNotNull(existing, "existing");

            _resolver.Inject(existing);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type)
        {
            Checker.IsNotNull(type, "type");

            return _resolver.Resolve<T>(type);
        }

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ResolveAll<T>()
        {
            return _resolver.ResolveAll<T>();
        }

        [DebuggerStepThrough]
        public static void Reset()
        {
            if (_resolver != null)
            {
                _resolver.Dispose();
            }
        }
    }
}
