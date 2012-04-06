using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.Infra
{
    public class DependencyResolverFactory : IDependencyResolverFactory
    {
        private readonly Type _resolveType;

        public DependencyResolverFactory(string resolverTypeName)
        {
            Checker.IsNotEmpty(resolverTypeName, "resolverTypeName");

            _resolveType = Type.GetType(resolverTypeName, true, true);
        }

        public DependencyResolverFactory()
            : this(AppsetHelper.GetString("DependencyResolveTypeName"))
        { }

        public IDependencyResolver CreateInstance()
        {
            return Activator.CreateInstance(_resolveType) as IDependencyResolver;
        }
    }
}
