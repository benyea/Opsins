using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Autofac;
using Autofac.Configuration;

namespace Opsins.Infra
{
    public class AutofacDependencyResolver : Disposable,IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver():this(new ContainerBuilder().Build())
        {
            UpdateContainer(cb=>cb.RegisterModule(new ConfigurationSettingsReader("autofac")));

            //UpdateContainer(cb=>cb.RegisterAssemblyTypes(typeof()));
        }

        public AutofacDependencyResolver(IContainer container)
        {
            Checker.IsNotNull(container,"container");

            _container = container;
        }

        public void Register<T>(T instance)
        {
            Checker.IsNotNull(instance,"instance");

            UpdateContainer(cb=>cb.RegisterInstance((object)instance).As<T>());
        }

        [DebuggerStepThrough]
        public void Inject<T>(T existing)
        {
            Checker.IsNotNull(existing, "existing");

            _container.InjectUnsetProperties(existing);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(Type type)
        {
            Checker.IsNotNull(type, "type");

            return (T)_container.Resolve(type);
        }

        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        [DebuggerStepThrough]
        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> namedInstances = _container.Resolve<IEnumerable<T>>();
            T unnamedInstance = default(T);

            try
            {
                unnamedInstance = _container.Resolve<T>();
            }
            catch (Exception)
            {
                //When default instance is missing
            }

            if (Equals(unnamedInstance, default(T)))
            {
                return namedInstances;
            }

            return new ReadOnlyCollection<T>(new List<T>(namedInstances) { unnamedInstance });
        }

        [DebuggerStepThrough]
        protected override void DisposeCore()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        /// <summary>
        /// 更新容器
        /// </summary>
        /// <param name="registrationBuilder"></param>
        private void UpdateContainer(Action<ContainerBuilder> registrationBuilder)
        {
            var containerBuilder = new ContainerBuilder();
            registrationBuilder(containerBuilder);
            containerBuilder.Update(_container);
        }
    }
}
