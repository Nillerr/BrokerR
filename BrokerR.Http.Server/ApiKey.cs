using System.Collections.Generic;
using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

namespace BrokerR.Http.Server
{
    internal sealed class ApiKey : IApiKey
    {
        public string Key { get; }
        public string OwnerName { get; }
        public IReadOnlyCollection<Claim> Claims { get; }

        public ApiKey(string key, string ownerName, IReadOnlyCollection<Claim> claims)
        {
            Key = key;
            OwnerName = ownerName;
            Claims = claims;
        }
    }
}