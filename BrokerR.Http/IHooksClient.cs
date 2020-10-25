using System.Threading.Tasks;

namespace BrokerR.Http
{
    public interface IHooksClient
    {
        Task SendRequest(WebhookRequest request);
    }
}