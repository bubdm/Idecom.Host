using Castle.Windsor;
using Idecom.Host.CastleWindsor;
using Idecom.Host.Interfaces;

namespace SampleService
{
    public class Container : IWantToSpecifyContainer
    {
        public IContainerAdapter ConfigureContainer()
        {
            return new CastleWindsorContainerAdapter(new WindsorContainer(), true);
        }
    }
}