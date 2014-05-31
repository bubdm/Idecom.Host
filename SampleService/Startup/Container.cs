using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Idecom.Host.CastleWindsor;
using Idecom.Host.Interfaces;

namespace SampleService.Startup
{
    public class Container : IWantToSpecifyContainer
    {
        public IContainerAdapter ConfigureContainer()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(new AssemblyFilter("")));
            return new CastleWindsorContainerAdapter(container);
        }
    }
}