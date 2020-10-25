using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BrokerR.Http.Client.Demo
{
    public sealed class MessageQueue : IMessageQueue
    {
        private readonly ILogger<MessageQueue> _logger;

        public MessageQueue(ILogger<MessageQueue> logger)
        {
            _logger = logger;
        }
        
        public Task SendAsync<TMessage>(TMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogInformation($"[{message.GetType().Name}]: {json}");
            return Task.CompletedTask;
        }
    }
}