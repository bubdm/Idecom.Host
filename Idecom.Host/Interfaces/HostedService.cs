namespace Idecom.Host.Interfaces
{
    using Topshelf;
    using Topshelf.HostConfigurators;

    public abstract class HostedService : ServiceControl
    {
        public abstract bool Start(HostControl hostControl);
        public abstract bool Stop(HostControl hostControl);
        public abstract bool Pause(HostControl hostControl);
        public abstract bool Continue(HostControl hostControl);

        public virtual void OverrideDefaultConfiguration(HostConfigurator configurator)
        {
        }

        public virtual bool Shutdown(HostControl hostControl)
        {
            return true;
        }


        public void SessionChanged(HostControl control, SessionChangedArguments sessionChangedArguments)
        {
        }

        /// <summary>
        ///     Used to configure service hosting configuration.
        ///     Such as Display name, crash behaviour erc...
        ///     You can add command line parameters and switches here as well.
        /// </summary>
        /// <param name="configurator"></param>
        internal void ConfigureHostDefault(HostConfigurator configurator)
        {
            configurator.EnableServiceRecovery(r =>
            {
                r.RestartService(0);
                r.RestartService(1);
                r.RestartService(5);
                r.OnCrashOnly();
                r.SetResetPeriod(1);
            });
            configurator.EnablePauseAndContinue();
            configurator.StartAutomaticallyDelayed();
            configurator.SetServiceName(GetType().Namespace + "." + GetType().Name);
            configurator.SetDescription("Used for hosting services in windows service manager as well as for debugging from command line.");
            OverrideDefaultConfiguration(configurator);
        }
    }
}