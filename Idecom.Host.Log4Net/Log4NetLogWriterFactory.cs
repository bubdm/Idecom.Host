namespace Idecom.Host.Log4Net
{
    using System;
    using System.Reflection;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;
    using Topshelf.Logging;

    public class Log4NetLogWriterFactory :
        LogWriterFactory
    {
        public LogWriter Get(string name)
        {
            return new Log4NetLogWriter(LogManager.GetLogger(name));
        }


        public void Shutdown()
        {
            LogManager.Shutdown();
        }


        public static void Use()
        {
            HostLogger.UseLogger(new Log4NetLoggerConfigurator(null));
        }


        public static void Use(Action<Hierarchy> configurator)
        {
            HostLogger.UseLogger(new Log4NetLoggerConfigurator(configurator));
        }


        [Serializable]
        public class Log4NetLoggerConfigurator :
            HostLoggerConfigurator
        {
            readonly Action<Hierarchy> _configurator;


            public Log4NetLoggerConfigurator(Action<Hierarchy> configurator)
            {
                _configurator = configurator;
            }


            public LogWriterFactory CreateLogWriterFactory()
            {
                var hierarchy = (Hierarchy) LogManager.GetRepository();
                hierarchy.Root.RemoveAllAppenders();

                var patternLayout = new PatternLayout
                {
                    ConversionPattern = "%-5level %date [%thread]  %logger  %newline%message%newline---%newline"
                };

                patternLayout.ActivateOptions();

                var asm = Assembly.GetEntryAssembly();
                var asmPath = new Uri(asm.CodeBase).LocalPath.Replace(":", string.Empty).Replace("\\", "_").Replace(".", "_").ToLowerInvariant();

                string moduleName = asmPath;

                // RollingAppender
                var rollerAppender = new RollingFileAppender
                {
                    AppendToFile = true,
                    File = string.Format("c:\\logs\\{0}.txt", moduleName),
                    Layout = patternLayout,
                    MaxSizeRollBackups = 1,
                    MaximumFileSize = "10MB",
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    StaticLogFileName = true
                };

                rollerAppender.ActivateOptions();
                hierarchy.Root.AddAppender(rollerAppender);

                var consoleAppender = SetupConsoleAppender();

                consoleAppender.Layout = patternLayout;
                consoleAppender.ActivateOptions();
                hierarchy.Root.AddAppender(consoleAppender);

                hierarchy.Root.Level = Level.All;
                hierarchy.Configured = true;

                if (_configurator != null)
                    _configurator(hierarchy);

                return new Log4NetLogWriterFactory();
            }

            public ColoredConsoleAppender SetupConsoleAppender()
            {
                var console = new ColoredConsoleAppender
                {
                    Layout = new SimpleLayout()
                };

                console.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Fatal,
                    BackColor = ColoredConsoleAppender.Colors.Red
                });

                console.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Error,
                    ForeColor = ColoredConsoleAppender.Colors.Red
                });

                console.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Warn,
                    ForeColor = ColoredConsoleAppender.Colors.Yellow
                });

                console.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Info,
                    ForeColor = ColoredConsoleAppender.Colors.Green
                });

                console.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Debug,
                    ForeColor = ColoredConsoleAppender.Colors.White
                });

                return console;
            }
        }
    }
}