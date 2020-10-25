using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BrokerR.Http.Client.Demo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await AsyncProgram.RunAsync(args, InvokeAsync);
        }

        private static async Task<Func<Task>> InvokeAsync(string[] args, CancellationToken cancellationToken)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<AssemblyToken>(optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            var services = new ServiceCollection();
            
            services
                .AddLogging(logging => logging
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole()
                );
            
            var startup = new Startup(configuration);
            startup.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            
            var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
            
            var hostedServices = serviceProvider.GetServices<IHostedService>().ToList();
            await StartHostedServices(hostedServices, logger, cancellationToken);

            return async () =>
            {
                await StopHostedServices(hostedServices, logger);
                await serviceProvider.DisposeAsync();
            };
        }

        private static async Task StartHostedServices(List<IHostedService> hostedServices, ILogger<Startup> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Hosted Services...");
            foreach (var hostedService in hostedServices)
            {
                logger.LogInformation($"Starting {hostedService.GetType().Name}...");
                await hostedService.StartAsync(cancellationToken);
            }

            logger.LogInformation("Started Hosted Services");
        }

        private static async Task StopHostedServices(IEnumerable<IHostedService> hostedServices, ILogger logger)
        {
            logger.LogInformation("Stopping Hosted Services...");

            foreach (var hostedService in hostedServices)
            {
                logger.LogInformation($"Stopping {hostedService.GetType().Name}...");
                await hostedService.StopAsync(default);
            }

            logger.LogInformation("Stopped Hosted Services");
        }
    }
}