namespace miniBank.Api.Middleware;

public class CorrelationIdMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Response.Headers[CorrelationHeader] = correlationId;
        }

        using (_logger.BeginScope(new Dictionary<string, object?> { { CorrelationHeader, correlationId.ToString() } }))
        {
            _logger.LogDebug("Handling request with correlation id {CorrelationId}", correlationId);
            await _next(context);
        }
    }
}
