namespace Idecom.Host.Log4Net
{
    using System;
    using log4net.Repository.Hierarchy;
    using Topshelf.HostConfigurators;

    public static class Log4NetConfigurationExtensions
    {
        /// <summary>
        ///   Specify that you want to use the Log4net logging engine.
        /// </summary>
        /// <param name="configurator"> </param>
        public static void UseLog4Net(this HostConfigurator configurator)
        {
            Log4NetLogWriterFactory.Use();
        }


        /// <summary>
        ///   Specify that you want to use the Log4net logging engine.
        /// </summary>
        /// <param name="configurator"> </param>
        /// <param name="configureAction"> The name of the log4net xml configuration file </param>
        public static void UseLog4Net(this HostConfigurator configurator, Action<Hierarchy> configureAction)
        {
            Log4NetLogWriterFactory.Use(configureAction);
        }
    }
}