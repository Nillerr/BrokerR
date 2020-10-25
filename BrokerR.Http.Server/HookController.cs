using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using BrokerR;

namespace BrokerR.Http.Server
{
    [ApiController]
    public sealed class HookController : Controller
    {
        private readonly IHubContext<HooksHub, IHooksClient> _hooksHub;
        private readonly ILogger<HookController> _logger;

        public HookController(IHubContext<HooksHub, IHooksClient> hooksHub, ILogger<HookController> logger)
        {
            _hooksHub = hooksHub;
            _logger = logger;
        }

        [HttpPost("/hook/{**path}")]
        [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Accept(string path)
        {
            _logger.LogInformation($"Received request: {HttpContext.Request.GetDisplayUrl()}");
            
            var query = HttpContext.Request.Query;
            if (query.TryGetValue("validationToken", out var validationToken))
            {
                return Ok(Uri.UnescapeDataString(validationToken));
            }
            
            var request = new WebhookRequest();
            
            request.Url = HttpContext.Request.GetEncodedPathAndQuery().Substring(5);
            request.Headers = HttpContext.Request.Headers
                .SelectMany(e => e.Value.Select(value => new RequestHeader {Name = e.Key, Value = value}))
                .ToList();

            using (var reader = new StreamReader(Request.Body))
            {
                request.Body = await reader.ReadToEndAsync();
            }

            await _hooksHub.Clients.Group($"/{path}").SendRequest(request);

            return Accepted();
        }
    }
}