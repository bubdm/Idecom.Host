namespace Idecom.Host.Interfaces
{
    using System;

    public interface IContainerAdapter
    {
        T Resolve<T>(Type service);
        void Release(object instance);
        IContainerAdapter CreateChildContainer();
        void Configure(Type component, Lifecycle lifecycle);
        void ConfigureInstance(object instance);

    }

    class ActivatorContainerAdapter : IContainerAdapter
    {
        public T Resolve<T>(Type service)
        {
            return (T)Activator.CreateInstance(service);
        }

        public void Release(object instance)
        {
        }

        public IContainerAdapter CreateChildContainer()
        {
            return this;
        }

        public void Configure(Type component, Lifecycle lifecycle)
        {
        }

        public void ConfigureInstance(object instance)
        {
        }
    }

    public enum Lifecycle
    {
        Singleton,
        PerWorkUnit,
        Transient
    }

}