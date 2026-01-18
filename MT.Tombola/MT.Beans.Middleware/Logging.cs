using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MT.Middleware
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;

        public Logging(RequestDelegate next, ILogger<Logging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Incoming Request: {method} {url}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            _logger.LogInformation("Outgoing Response: {statusCode}",
                context.Response.StatusCode);
        }

    }
}
