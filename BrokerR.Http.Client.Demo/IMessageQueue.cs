using System.Threading.Tasks;

namespace BrokerR.Http.Client.Demo
{
    public interface IMessageQueue
    {
        Task SendAsync<TMessage>(TMessage message);
    }
}