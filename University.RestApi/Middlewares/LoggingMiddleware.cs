﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace University.RestApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log request details
            _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            stopwatch.Stop();

            // Log response details
            _logger.LogInformation($"Outgoing response: {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
