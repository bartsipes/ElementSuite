using ElementSuite.Common;
using ElementSuite.Common.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ElementSuite.Core.Service
{
    public sealed class LoggingService : ILoggingService
    {
        private ILog logger;

        public LoggingService()
        {
            logger = LogManager.GetLogger("ElementSuite");
        }

        public void Log(LogLevel level, string description)
        {
            switch (level)
            {
                case LogLevel.Fatal:
                    logger.Fatal(description);
                    break;
                case LogLevel.Warning:
                    logger.Warn(description);
                    break;
                case LogLevel.Error:
                    logger.Error(description);
                    break;
                case LogLevel.Info:
                    logger.Info(description);
                    break;
                case LogLevel.Debug:
                    logger.Debug(description);
                    break;
                default:
                    logger.Warn(String.Format("LogLevel {0} was unrecognized. Logging message as error.", level.ToString()));
                    logger.Error(description);
                    break;
            }
        }

        public void Log(LogLevel level, string description, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Fatal:
                    logger.Fatal(description, exception);
                    break;
                case LogLevel.Warning:
                    logger.Warn(description, exception);
                    break;
                case LogLevel.Error:
                    logger.Error(description, exception);
                    break;
                case LogLevel.Info:
                    logger.Info(description, exception);
                    break;
                case LogLevel.Debug:
                    logger.Debug(description, exception);
                    break;
                default:
                    logger.Warn(String.Format("LogLevel {0} was unrecognized. Logging message as error.", level.ToString()));
                    logger.Error(description, exception);
                    break;
            }
        }
    }
}
