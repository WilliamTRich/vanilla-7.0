using Common;
using System.Threading.RateLimiting;
internal class Program
{
    private static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

        Settings.Init();
        Resources.Init();
        Database.Init();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>
            (httpContext => RateLimitPartition.GetFixedWindowLimiter
            (partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 35,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    await context.HttpContext.Response.WriteAsync(
                        $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). ",
                        cancellationToken: token);
                }
                else
                {
                    await context.HttpContext.Response.WriteAsync(
                        "Too many requests. Please try again later. ",
                        cancellationToken: token);
                }
            };
        });

        var service = builder.Services;

        service.AddControllers();

        var app = builder.Build();

        app.Urls.Clear();
        app.Urls.Add($"http://{Settings.Address}:{Settings.Ports[0]}");
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        app.UseRateLimiter();

#if DEBUG
        //middleware to add request logging
        app.Use((context, next) =>
        {
            SLog.Trace($"Request::{context.Request.Path}::From::{context.Connection.RemoteIpAddress}");
            return next(context);
        });
#endif

        app.Run();
    }
    static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        SLog.Fatal((Exception)e.ExceptionObject);
    }
}

#if DEBUG

#endif
