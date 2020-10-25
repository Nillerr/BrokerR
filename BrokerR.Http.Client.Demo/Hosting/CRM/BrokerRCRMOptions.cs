using System;
using Microsoft.Extensions.DependencyInjection;

namespace BrokerR.Http.Client.Demo.Hosting.CRM
{
    public sealed class BrokerRCRMOptions
    {
        private readonly BrokerROptions _options;
        private readonly ICRMOptions _configuration;

        public BrokerRCRMOptions(BrokerROptions options, ICRMOptions configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public BrokerRCRMOptions WithEntity<TMessage>(string entity, Func<Guid, TMessage> messageFactory)
        {
            var path = $"/crm/{_configuration.Environment}/{entity}";
            
            _options.WithHandler<CRMWebhookRequest>(path, async (body, token) =>
            {
                var messageQueue = _options.ServiceProvider.GetRequiredService<IMessageQueue>();
                var message = messageFactory(body.PrimaryEntityId);
                await messageQueue.SendAsync(message);
            });

            return this;
        }
    }
}