namespace StarWars.WebApi.Middlewares;

using StarWars.DAL.Entities;
using StarWars.DAL;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        dbContext.RequestHistories.Add(new RequestHistory
        {
            Endpoint = context.Request.Path,
            RequestedAt = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();

        await next(context);
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder) => builder.UseMiddleware<RequestLoggingMiddleware>();
}