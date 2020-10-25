using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using BrokerR.Http.Client.Logging;

namespace BrokerR.Http.Client
{
    /// <summary>
    /// A connection used to subscribe 
    /// </summary>
    public sealed class BrokerRConnection : IAsyncDisposable
    {
        private static readonly Random Random = new Random();

        private readonly HubConnection _connection;
        private readonly Dictionary<string, RequestHandler> _handlers;
        private readonly Logger? _logger;

        private readonly CancellationTokenSource _stoppingTokenSource = new CancellationTokenSource();

        public BrokerRConnection(string url, Dictionary<string, RequestHandler> handlers, Logger? logger)
        {
            var retryPolicy = new EternalRetryPolicy(logger);
            
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect(retryPolicy)
                .Build();
            
            _handlers = handlers;
            _logger = logger;
        }

        public ManagerState State { get; private set; } = ManagerState.Stopped;

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (State == ManagerState.Started)
            {
                return;
            }
        
            await ConnectAsync(cancellationToken);

            var stoppingToken = _stoppingTokenSource.Token;

            await using (cancellationToken.Register(() => _stoppingTokenSource.Cancel()))
            {
                await SubscribeAsync(stoppingToken);

                _connection.Reconnected += async connectionId =>
                {
                    LogDebug("Reconnected");
                    await SubscribeAsync(stoppingToken);
                };
            }

            _connection.On<WebhookRequest>(nameof(IHooksClient.SendRequest), InvokeHandlerAsync);

            State = ManagerState.Started;
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            while (_connection.State != HubConnectionState.Connected)
            {
                try
                {
                    LogDebug("Connecting...");
                    await _connection.StartAsync(cancellationToken);
                    LogDebug("Connected");
                }
                catch when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogWarn($"Failed to connect: {ex}");

                    var delay = TimeSpan.FromSeconds(5) * Random.NextDouble();
                    LogDebug($"Retrying in {delay:g}");
                    
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }

        private async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            foreach (var path in _handlers.Keys)
            {
                LogDebug($"Subscribing to {path}...");
                await _connection.SendAsync("Subscribe", path, cancellationToken);
                LogDebug($"Subscribed to {path}");
            }
            
            LogDebug($"Subscribed");
        }

        private async Task InvokeHandlerAsync(WebhookRequest request)
        {
            var queryIndex = request.Url.IndexOf("?", StringComparison.Ordinal);
            
            var encodedPath = queryIndex == -1 
                ? request.Url
                : request.Url.Substring(0, queryIndex);
            
            var path = Uri.UnescapeDataString(encodedPath);
            if (_handlers.TryGetValue(path, out var handler))
            {
                await handler(request);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (State == ManagerState.Stopped)
            {
                return;
            }
            
            LogDebug("Disconnecting...");
            _stoppingTokenSource.Cancel();

            await _connection.StopAsync(cancellationToken);
            
            State = ManagerState.Stopped;
            LogDebug("Disconnected");
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
            await _connection.DisposeAsync();
            _stoppingTokenSource.Dispose();
        }

        private void LogWarn(string message, Exception? exception = null)
        {
            _logger?.Invoke(this, new LogArgs(BrokerRLogLevel.Warning, message)
            {
                Exception = exception
            });
        }
        
        private void LogDebug(string message)
        {
            _logger?.Invoke(this, new LogArgs(BrokerRLogLevel.Debug, message));
        }
    }
}