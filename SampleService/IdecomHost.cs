using System;
using Idecom.Host.Interfaces;
using log4net;
using Topshelf;
using Topshelf.HostConfigurators;

namespace SampleService
{
    public class IdecomHost : HostedService
    {
        public ILog Log { get; set; }

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