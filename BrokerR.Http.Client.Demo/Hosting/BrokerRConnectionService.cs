using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BrokerR.Http.Client.Demo.Hosting
{
    public sealed class BrokerRConnectionService : IHostedService, IAsyncDisposable
    {
        private readonly BrokerRConnection _connection;

        public BrokerRConnectionService(BrokerRConnection connection)
        {
            _connection = connection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connection.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.StopAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}