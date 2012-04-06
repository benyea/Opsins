using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.Infra
{
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}
