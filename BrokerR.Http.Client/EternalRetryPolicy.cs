using System;
using Microsoft.AspNetCore.SignalR.Client;
using BrokerR.Http.Client.Logging;

namespace BrokerR.Http.Client
{
    internal sealed class EternalRetryPolicy : IRetryPolicy
    {
        private static readonly Random _random = new Random();
        
        private readonly Logger? _logger;

        public EternalRetryPolicy(Logger? logger)
        {
            _logger = logger;
        }
        
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            var nextRetryDelay = TimeSpan.FromSeconds(5) + (TimeSpan.FromSeconds(5) * _random.NextDouble());
            LogDebug($"Reconnecting in {nextRetryDelay:g}");
            return nextRetryDelay;
        }

        private void LogDebug(string message)
        {
            _logger?.Invoke(this, new LogArgs(BrokerRLogLevel.Debug, message));
        }
    }
}