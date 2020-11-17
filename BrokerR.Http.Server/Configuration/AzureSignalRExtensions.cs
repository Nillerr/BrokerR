using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrokerR.Http.Server.Configuration
{
    public static class AzureSignalRExtensions
    {
        public static ISignalRServerBuilder AddAzureSignalR(this ISignalRServerBuilder signalR, IConfiguration configuration)
        {
            var connectionString = configuration["Azure:SignalR:ConnectionString"];
            if (connectionString != null)
            {
                return signalR.AddAzureSignalR(connectionString);
            }

            return signalR;
        }
    }
}