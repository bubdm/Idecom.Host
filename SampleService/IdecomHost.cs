namespace SampleService
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using log4net;
    using Topshelf;
    using Topshelf.HostConfigurators;
    using Idecom.Host.CastleWindsor;
    using Idecom.Host.Interfaces;

    public class IdecomHost : HostedService, IWantToSpecifyContainer
    {
        public ILog Log { get; set; }

        public IContainerAdapter ConfigureContainer()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(new AssemblyFilter("")));
            return new CastleWindsorContainerAdapter(container);
        }

        public override bool Start(HostControl hostControl)
        {
            Log.Info("IdecomHost Starting...");
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));

            Log.Info("IdecomHost Started");

            return true;
        }

        public override bool Stop(HostControl hostControl)
        {
            Log.Info("IdecomHost Stopped");
            return true;
        }

        public override void OverrideDefaultConfiguration(HostConfigurator configurator)
        {
            Log.Info("Overriding service configuration");
        }

        public override bool Pause(HostControl hostControl)
        {
            Log.Info("IdecomHost Paused");
            return true;
        }

        public override bool Continue(HostControl hostControl)
        {
            Log.Info("IdecomHost Continued");
            return true;
        }
    }
}