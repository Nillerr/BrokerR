using System.Threading;
using System.Threading.Tasks;

namespace BrokerR.Http.Client
{
    public delegate Task RequestBodyHandler<in T>(T body, CancellationToken cancellationToken = default);
}