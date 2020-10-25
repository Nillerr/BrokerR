using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;

namespace BrokerR.Http.Server
{
    public sealed class ApiKeyProvider : IApiKeyProvider
    {
        private readonly IConfiguration _configuration;

        public ApiKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Task<IApiKey?> ProvideAsync(string key)
        {
            var primaryApiKey = _configuration["Authentication:PrimaryApiKey"];
            if (primaryApiKey == key)
            {
                var apiKey = new ApiKey(key, "Primary", Array.Empty<Claim>());
                return Task.FromResult<IApiKey?>(apiKey);
            }
            
            var secondaryApiKey = _configuration["Authentication:SecondaryApiKey"];
            if (secondaryApiKey == key)
            {
                var apiKey = new ApiKey(key, "Secondary", Array.Empty<Claim>());
                return Task.FromResult<IApiKey?>(apiKey);
            }

            return Task.FromResult<IApiKey?>(null);
        }
    }
}