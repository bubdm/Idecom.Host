namespace Idecom.Host.Log4Net.Windsor
{
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using log4net;

    public class WindsorInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Log4NetLogWriterFactory.Use(); 
            container.Register(Component.For<ILog>().UsingFactoryMethod((k, c) => LogManager.GetLogger(c.RequestedType)).LifestyleTransient());
            container.Kernel.Resolver.AddSubResolver(new LoggerResolver());

        }
    }

    public class LoggerResolver : ISubDependencyResolver
    {
        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return LogManager.GetLogger(model.Implementation.FullName);
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType == typeof(ILog);
        }
    }

}