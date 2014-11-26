using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Common.Helpers
{
    public class Logger
    {
        #region SingletonImplementation
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }
        #endregion SingletonImplementation

        private NLog.Logger _logger;

        private Logger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogMessage(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.debug:
                    _logger.Log(NLog.LogLevel.Debug, message);
                    break;
                case LogLevel.error:
                    _logger.Log(NLog.LogLevel.Error, message);
                    break;
                case LogLevel.trace:
                    _logger.Log(NLog.LogLevel.Trace, message);
                    break;
                case LogLevel.info:
                    _logger.Log(NLog.LogLevel.Info, message);
                    break;
                case LogLevel.warning:
                    _logger.Log(NLog.LogLevel.Warn, message);
                    break;
                case LogLevel.fatal:
                    _logger.Log(NLog.LogLevel.Fatal, message);
                    break;
            }
        }

        public void LogException(Exception exc, LogLevel level, string message = "exception occured")
        {
            switch (level)
            {
                case LogLevel.debug:
                    _logger.Debug(message, exc);
                    break;
                case LogLevel.error:
                    _logger.Error(message, exc);
                    break;
                case LogLevel.trace:
                    _logger.Trace(message, exc);
                    break;
                case LogLevel.info:
                    _logger.Info(message, exc);
                    break;
                case LogLevel.warning:
                    _logger.Warn(message, exc);
                    break;
                case LogLevel.fatal:
                    _logger.Fatal(message, exc);
                    break;
            }
        }
    }

    public enum LogLevel
    {
        debug,
        error,
        trace,
        info,
        warning,
        fatal
    }
}
