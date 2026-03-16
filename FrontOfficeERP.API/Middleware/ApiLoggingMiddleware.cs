namespace FrontOfficeERP.API.Middleware;

public class ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("API Request {Method} {Path}", context.Request.Method, context.Request.Path);
        await next(context);
        logger.LogInformation("API Response {StatusCode}", context.Response.StatusCode);
    }
}
