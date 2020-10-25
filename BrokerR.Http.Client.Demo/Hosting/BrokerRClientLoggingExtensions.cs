using System;
using Microsoft.Extensions.Logging;
using BrokerR.Http.Client.Logging;

namespace BrokerR.Http.Client.Demo.Hosting
{
    public static class BrokerRClientLoggingExtensions
    {
        public static BrokerRConnectionBuilder WithLogging(
            this BrokerRConnectionBuilder source,
            BrokerRLogLevel level,
            Func<ILogger> logger
        )
        {
            return source.WithLogging((sender, e) => Log(e.LogLevel, e));

            void Log(BrokerRLogLevel logLevel, LogArgs e)
            {
                if (logLevel < level)
                {
                    return;
                }

                switch (logLevel)
                {
                    case BrokerRLogLevel.Trace:
                        logger().LogTrace(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.Debug:
                        logger().LogDebug(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.Information:
                        logger().LogInformation(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.Warning:
                        logger().LogWarning(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.Error:
                        logger().LogError(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.Critical:
                        logger().LogCritical(e.Exception, e.Message);
                        break;
                    case BrokerRLogLevel.None:
                        // Don't log
                        break;
                    default:
                        logger()
                            .LogWarning($"Unknown log level `{logLevel:G}`. Please update `BrokerR.Client.Logging`.");
                        break;
                }
            }
        }
    }
}