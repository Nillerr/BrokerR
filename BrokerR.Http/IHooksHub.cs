using System.Threading.Tasks;

namespace BrokerR.Http
{
    public interface IHooksHub
    {
        Task Subscribe(string path);
        Task Unsubscribe(string path);
    }
}