using Microsoft.AspNetCore.Mvc;

namespace BrokerR.Http.Server
{
    [ApiController]
    public sealed class VersionController : Controller
    {
        [HttpGet("/version")]
        public string? Version()
        {
            var version = typeof(VersionController).Assembly.GetName().Version;
            return version?.ToString(3);
        }
    }
}