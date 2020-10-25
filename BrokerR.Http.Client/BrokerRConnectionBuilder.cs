using System.Collections.Generic;
using System.Text.Json;

namespace BrokerR.Http.Client
{
    public sealed class BrokerRConnectionBuilder
    {
        private readonly string _url;
        
        private readonly Dictionary<string, RequestHandler> _handlers =
            new Dictionary<string, RequestHandler>();

        private Logger? _logger;

        public BrokerRConnectionBuilder(string url)
        {
            _url = url;
        }

        public BrokerRConnectionBuilder WithHandler(string path, RequestHandler handler)
        {
            _handlers[path] = handler;
            return this;
        }
        
        public BrokerRConnectionBuilder WithHandler<T>(string path, RequestBodyHandler<T> handler)
        {
            return WithHandler(path, async (request, cancellationToken) =>
            {
                var body = JsonSerializer.Deserialize<T>(request.Body);
                await handler(body, cancellationToken);
            });
        }

        public BrokerRConnectionBuilder WithLogging(Logger logger)
        {
            _logger = logger;
            return this;
        }

        public BrokerRConnection Build()
        {
            return new BrokerRConnection(_url, _handlers, _logger);
        }
    }
}