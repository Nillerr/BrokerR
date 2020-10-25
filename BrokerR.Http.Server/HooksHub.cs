using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using BrokerR;

namespace BrokerR.Http.Server
{
    [PublicAPI]
    [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
    public sealed class HooksHub : Hub<IHooksClient>, IHooksHub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task Subscribe(string path)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, path);
        }

        public async Task Unsubscribe(string path)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, path);
        }
    }
}