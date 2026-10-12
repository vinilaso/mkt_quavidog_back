using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Sienna.WebApi.RateLimiting
{
    internal static class RateLimitPolicies
    {
        internal const string Invitations = "invitations";
        internal const string Authentication = "authentication";

        internal static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
        {
            return services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = OnRejectedAsync;

                options.AddPolicy(Invitations, httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        GetPartitionKey(httpContext),
                        _ => new FixedWindowRateLimiterOptions { PermitLimit = 20, Window = TimeSpan.FromHours(1) }
                    );
                });

                options.AddPolicy(Authentication, httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        GetPartitionKey(httpContext),
                        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) }
                    );
                });
            });
        }

        private static async ValueTask OnRejectedAsync(OnRejectedContext context, CancellationToken cancellationToken)
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();

            await TypedResults.Problem(
                statusCode: StatusCodes.Status429TooManyRequests,
                title: "Muitas requisições",
                type: "RateLimit.Exceeded",
                detail: "Você atingiu o limite de tentativas para esta operação. Tente novamente mais tarde."
            ).ExecuteAsync(context.HttpContext);
        }

        private static string GetPartitionKey(HttpContext httpContext)
        {
            if (httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) is string userId)
                return $"user:{userId}";

            return $"ip:{httpContext.Connection.RemoteIpAddress}";
        }
    }
}
