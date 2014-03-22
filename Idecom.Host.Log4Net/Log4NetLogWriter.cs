namespace Idecom.Host.Log4Net
{
    using System;
    using System.Globalization;
    using Topshelf.Logging;

    public class Log4NetLogWriter :
        LogWriter
    {
        readonly log4net.ILog _log;


        public Log4NetLogWriter(log4net.ILog log)
        {
            _log = log;
        }


        public void Debug(object message)
        {
            _log.Debug(message);
        }


        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }


        public void Debug(LogWriterOutputProvider messageProvider)
        {
            if (!IsDebugEnabled)
                return;


            _log.Debug(messageProvider());
        }


        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }


        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }


        public void Info(object message)
        {
            _log.Info(message);
        }


        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }


        public void Info(LogWriterOutputProvider messageProvider)
        {
            if (!IsInfoEnabled)
                return;


            _log.Info(messageProvider());
        }


        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }


        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }


        public void Warn(object message)
        {
            _log.Warn(message);
        }


        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }


        public void Warn(LogWriterOutputProvider messageProvider)
        {
            if (!IsWarnEnabled)
                return;


            _log.Warn(messageProvider());
        }


        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }


        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }


        public void Error(object message)
        {
            _log.Error(message);
        }


        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }


        public void Error(LogWriterOutputProvider messageProvider)
        {
            if (!IsErrorEnabled)
                return;


            _log.Error(messageProvider());
        }


        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }


        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }


        public void Fatal(object message)
        {
            _log.Fatal(message);
        }


        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }


        public void Fatal(LogWriterOutputProvider messageProvider)
        {
            if (!IsFatalEnabled)
                return;


            _log.Fatal(messageProvider());
        }


        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }


        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }


        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }


        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }


        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }


        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }


        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }


        public void Log(LoggingLevel level, object obj)
        {
            if (level == LoggingLevel.Fatal)
                Fatal(obj);
            else if (level == LoggingLevel.Error)
                Error(obj);
            else if (level == LoggingLevel.Warn)
                Warn(obj);
            else if (level == LoggingLevel.Info)
                Info(obj);
            else if (level >= LoggingLevel.Debug)
                Debug(obj);
        }


        public void Log(LoggingLevel level, object obj, Exception exception)
        {
            if (level == LoggingLevel.Fatal)
                Fatal(obj, exception);
            else if (level == LoggingLevel.Error)
                Error(obj, exception);
            else if (level == LoggingLevel.Warn)
                Warn(obj, exception);
            else if (level == LoggingLevel.Info)
                Info(obj, exception);
            else if (level >= LoggingLevel.Debug)
                Debug(obj, exception);
        }


        public void Log(LoggingLevel level, LogWriterOutputProvider messageProvider)
        {
            if (level == LoggingLevel.Fatal)
                Fatal(messageProvider);
            else if (level == LoggingLevel.Error)
                Error(messageProvider);
            else if (level == LoggingLevel.Warn)
                Warn(messageProvider);
            else if (level == LoggingLevel.Info)
                Info(messageProvider);
            else if (level >= LoggingLevel.Debug)
                Debug(messageProvider);
        }


        public void LogFormat(LoggingLevel level, string format, params object[] args)
        {
            if (level == LoggingLevel.Fatal)
                FatalFormat(CultureInfo.InvariantCulture, format, args);
            else if (level == LoggingLevel.Error)
                ErrorFormat(CultureInfo.InvariantCulture, format, args);
            else if (level == LoggingLevel.Warn)
                WarnFormat(CultureInfo.InvariantCulture, format, args);
            else if (level == LoggingLevel.Info)
                InfoFormat(CultureInfo.InvariantCulture, format, args);
            else if (level >= LoggingLevel.Debug)
                DebugFormat(CultureInfo.InvariantCulture, format, args);
        }


        public void LogFormat(LoggingLevel level, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (level == LoggingLevel.Fatal)
                FatalFormat(formatProvider, format, args);
            else if (level == LoggingLevel.Error)
                ErrorFormat(formatProvider, format, args);
            else if (level == LoggingLevel.Warn)
                WarnFormat(formatProvider, format, args);
            else if (level == LoggingLevel.Info)
                InfoFormat(formatProvider, format, args);
            else if (level >= LoggingLevel.Debug)
                DebugFormat(formatProvider, format, args);
        }
    }
}