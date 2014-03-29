using System;
using Idecom.Host.Interfaces;
using Topshelf;
using Topshelf.HostConfigurators;

//using Castle.MicroKernel.Registration;
//using Castle.Windsor;
//using Castle.Windsor.Installer;
//using Idecom.Host.CastleWindsor;
//using log4net;
//using Topshelf.Logging;


namespace idecomrootns
{
    /// <summary>
    /// IWantToSpecifyContainer - use this interface if you'd like to specify your own IoC container
    /// IWantToStartAfterServiceStarts - use this interface on any class to support starting/stopping services with the host
    /// </summary>
    public class IdecomHost : HostedService
    {
//        public ILog Log { get; set; } //you can install log4net support Idecom.Host.Log4Net or with IoC resolution Idecom.Host.CastleWindsor

        public override bool Start(HostControl hostControl)
        {

            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));

            return true;
        }

        public override bool Stop(HostControl hostControl)
        {
            return true;
        }

        public override void OverrideDefaultConfiguration(HostConfigurator configurator)
        {
        }

        public override bool Pause(HostControl hostControl)
        {
            return true;
        }

        public override bool Continue(HostControl hostControl)
        {
            return true;
        }

//        /// <summary>
//        /// Integration of Castle Windsor, Log4Net and Idecom.Host can be installed with Idecom.Host.Log4Net.Windsor package
//        /// </summary>
//        public IContainerAdapter ConfigureContainer()
//        {
//            var container = new WindsorContainer();
//
//            //This runs all castle installers in the folder. Log4net integration is configured by the installer
//            //see https://github.com/evgenyk/Idecom.Host/blob/master/Idecom.Host.Log4Net.Windsor/WindsorInstaller.cs
//            container.Install(FromAssembly.InDirectory(new AssemblyFilter(""))); 
//
//            return new CastleWindsorContainerAdapter(container);
//        }
    }
}
