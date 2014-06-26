namespace Idecom.Host
{
    using System;
    using System.Linq;
    using Interfaces;
    using Topshelf;
    using Utility;

    class Program
    {
        static HostedService _hostCache;
        static readonly object LockRoot = new object();

        static int Main()
        {
            var containerAdapter = DiscoverContainerAdapter();
            var iWantToInitializeThreadManager = new WantToInitializeThreadManager(containerAdapter);
            var iWantToStartThreadManager = new WantToStartThreadManager(containerAdapter);
            var service = DiscoverService(containerAdapter);

            var host = HostFactory.New(configurator =>
            {
                
                var hostConfigurator = configurator.Service<HostedService>(hostSettings =>
                {
                    hostSettings.ConstructUsing(() => DiscoverService(containerAdapter));
                    hostSettings.WhenStarted((a, b) => a.Start(b));
                    hostSettings.WhenStopped((a, b) => a.Stop(b));
                    hostSettings.WhenPaused((a, b) => a.Pause(b));
                    hostSettings.WhenContinued((a, b) => a.Continue(b));
                    hostSettings.WhenShutdown((a, b) => a.Continue(b));
                    hostSettings.WhenSessionChanged(((hostedService, control, changedArguments) => hostedService.SessionChanged(control, changedArguments)));
                    hostSettings.AfterStartingService(hostStartedContext => iWantToInitializeThreadManager.BeforeStart(hostStartedContext).ContinueWith(task => iWantToStartThreadManager.BeforeStart(hostStartedContext)).Wait());
                    hostSettings.BeforeStoppingService(hostStartedContext => iWantToStartThreadManager.BeforeStop(hostStartedContext).ContinueWith(task => iWantToInitializeThreadManager.BeforeStop(hostStartedContext)).Wait());
                });

                service.ConfigureHostDefault(hostConfigurator);
            });
            return (int) host.Run();
        }

        static IContainerAdapter DiscoverContainerAdapter()
        {
            var containerSpecifier = AssemblyScanner.GetScannableAssemblies().SelectMany(x => x.GetTypes()).FirstImplementingInstance<IWantToSpecifyContainer>();
            IContainerAdapter discoveredContainerAdapter = new ActivatorContainerAdapter();
            if (containerSpecifier == null) return discoveredContainerAdapter;
            discoveredContainerAdapter = containerSpecifier.ConfigureContainer().CreateChildContainer();
            return discoveredContainerAdapter;
        }

        static HostedService DiscoverService(IContainerAdapter containerAdapter)
        {
            if (_hostCache != null)
                return _hostCache;

            lock (LockRoot)
            {
                var services = AssemblyScanner.GetScannableAssemblies().SelectMany(x => x.GetTypes()).BasedOn<HostedService>().ToList();

                if (services.Count > 1)
                    throw new Exception("More than one service found, don't know which one to start: " + services.Select(x => x.FullName).Aggregate((s, s1) => string.Format("{0}, {1}", s, s1)));
                Type firstOrDefault = services.FirstOrDefault();

                if (firstOrDefault == null)
                    throw new Exception("A class implementing ServiceControl could not be found.");
                _hostCache = containerAdapter.Resolve<HostedService>(firstOrDefault);

                if (_hostCache != null) return _hostCache;

                containerAdapter.Configure(firstOrDefault, Lifecycle.Singleton);
                _hostCache = containerAdapter.Resolve<HostedService>(firstOrDefault);
            }
            return _hostCache;
        }
    }
}