using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BrokerR.Http.Client.Logging;

namespace BrokerR.Http.Client.Demo.Hosting
{
    public sealed class BrokerROptions
    {
        public BrokerROptions(IServiceProvider serviceProvider, string url)
        {
            ServiceProvider = serviceProvider;
            ConnectionBuilder = new BrokerRConnectionBuilder(url);
        }

        public BrokerRConnectionBuilder ConnectionBuilder { get; }
        
        public IServiceProvider ServiceProvider { get; }

        public BrokerROptions WithHandler(string path, RequestHandler handler)
        {
            ConnectionBuilder.WithHandler(path, handler);
            return this;
        }

        public BrokerROptions WithHandler<T>(string path, RequestBodyHandler<T> handler)
        {
            ConnectionBuilder.WithHandler(path, handler);
            return this;
        }

        /// <summary>
        /// Configures the underlying <see cref="BrokerRConnection"/> to log messages to an instance of
        /// <see cref="ILogger"/>.
        /// </summary>
        /// <param name="level">The minimum level of logging</param>
        /// <returns>The options</returns>
        public BrokerROptions ConfigureLogging(BrokerRLogLevel level)
        {
            ConnectionBuilder.WithLogging(level, ServiceProvider.GetRequiredService<ILogger<BrokerRConnection>>);
            return this;
        }
    }
}