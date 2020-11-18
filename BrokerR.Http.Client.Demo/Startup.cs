using BrokerR.Http.Client.Demo.Hosting.CRM;
using BrokerR.Http.Client.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BrokerR.Http.Client.Logging;

namespace BrokerR.Http.Client.Demo
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageQueue, MessageQueue>();
            AddCRMBrokerR(services);
        }

        private void AddCRMBrokerR(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("BrokerR");
            var crmOptions = Configuration.GetSection(CRMOptions.CRM).Get<CRMOptions>();

            services.AddBrokerR(connectionString, (s, broker) => BrokerROptionsExtensions.WithCRM(broker
                        .ConfigureLogging(BrokerRLogLevel.Debug), crmOptions, crm => crm
                    .WithEntity("contacts", ContactChanged.Create)
                    .WithEntity("accounts", AccountChanged.Create)
                )
            );
        }
    }
}