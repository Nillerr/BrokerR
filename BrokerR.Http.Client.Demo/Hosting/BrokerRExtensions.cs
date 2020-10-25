using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrokerR.Http.Client.Demo.Hosting
{
    public static class BrokerRExtensions
    {
        public static IServiceCollection AddBrokerR(
            this IServiceCollection services,
            string url,
            Action<IServiceProvider, BrokerROptions> configure
        )
        {
            services.AddTransient<IHostedService>(serviceProvider =>
            {
                var options = new BrokerROptions(serviceProvider, url);
                configure(serviceProvider, options);

                var connection = options.ConnectionBuilder.Build();
                return new BrokerRConnectionService(connection);
            });

            return services;
        }
    }
}