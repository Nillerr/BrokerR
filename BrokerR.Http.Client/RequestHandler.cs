using System.Threading;
using System.Threading.Tasks;

namespace BrokerR.Http.Client
{
    public delegate Task RequestHandler(WebhookRequest request, CancellationToken cancellationToken = default);
}