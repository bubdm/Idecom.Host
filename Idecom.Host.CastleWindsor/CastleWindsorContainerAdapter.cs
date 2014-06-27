using Castle.Core.Internal;
using Castle.Windsor.Installer;
using Idecom.Host.Utility;

namespace Idecom.Host.CastleWindsor
{
    using System;
    using System.Linq;
    using Castle.Core;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Interfaces;

    public class CastleWindsorContainerAdapter: IContainerAdapter
    {
        readonly IWindsorContainer _container;

        public CastleWindsorContainerAdapter(IWindsorContainer container = null, bool runInstallers = false)
        {
            _container = container ?? new WindsorContainer();

            if (runInstallers)
                RunInstallers();
        }

        private void RunInstallers()
        {
            AssemblyScanner.GetScannableAssemblies().ForEach(x =>
            {
                try
                {
                    _container.Install(FromAssembly.Instance(x));
                }
                catch
                {
                }
            });
        }

        public T Resolve<T>(Type service)
        {
            try
            {
                return (T) _container.Resolve(service);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public void Release(object instance)
        {
            _container.Release(instance);
        }

        public IContainerAdapter CreateChildContainer()
        {
            var childContainer = new WindsorContainer();
            _container.AddChildContainer(childContainer);
            return new CastleWindsorContainerAdapter(childContainer);
        }

        public void Configure(Type component, Lifecycle lifecycle)
        {
            var lifestyleTypeFrom = GetLifestyleTypeFrom(lifecycle);
            var services = component.GetInterfaces().Where(x => !x.FullName.StartsWith("System.")).Concat(new[] { component });
            _container.Register(Component.For(services).ImplementedBy(component).LifeStyle.Is(lifestyleTypeFrom));
        }

        public void ConfigureInstance(object instance)
        {
            var component = instance.GetType();

            var registration = _container.Kernel.GetAssignableHandlers(component).Select(x => x.ComponentModel).SingleOrDefault();
            if (registration != null)
                throw new Exception("Can not configure a component that has already been registered.");

            var services = component.GetInterfaces().Where(x => !x.FullName.StartsWith("System.")).Concat(new[] { component });
            _container.Register(Component.For(services).Instance(instance).LifeStyle.Is(LifestyleType.Singleton));            
        }

        LifestyleType GetLifestyleTypeFrom(Lifecycle lifecycle)
        {
            switch (lifecycle)
            {
                case Lifecycle.Singleton:
                    return LifestyleType.Singleton;
                case Lifecycle.PerWorkUnit:
                    return LifestyleType.Scoped;
                case Lifecycle.Transient:
                    return LifestyleType.Transient;
                default:
                    throw new ArgumentOutOfRangeException("lifecycle", string.Format("Unknown lifestype {0}, please check what's going on", lifecycle));
            }
        }

    }
}