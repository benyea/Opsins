using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.Infra
{
    public class Bootstrapper
    {
        static Bootstrapper()
        {
            try
            {
                Ioc.InitialzeWith(new DependencyResolverFactory());
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public static void Run()
        {
            Ioc.ResolveAll<IBootstrapperTask>().Each(t => t.Execute());
        }
    }
}
