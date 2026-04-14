using Microsoft.OpenApi;

namespace Sienna.WebApi.Endpoints.Extensions
{
    internal static class RouteHandlerBuilderExtensions
    {
        internal static RouteHandlerBuilder ProducesWithDescription<T>(this RouteHandlerBuilder builder, int statusCode, string description)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Produces<T>(statusCode).AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
            {
                return AddDescription(operation, statusCode, description);
            });
        }

        internal static RouteHandlerBuilder ProducesProblemWithDescription(this RouteHandlerBuilder builder, int statusCode, string description)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.ProducesProblem(statusCode).AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
            {
                return AddDescription(operation, statusCode, description);
            });
        }

        private static Task AddDescription(OpenApiOperation operation, int statusCode, string description)
        {
            if (operation.Responses is null)
                return Task.CompletedTask;

            if (operation.Responses.TryGetValue(statusCode.ToString(), out var response))
                response.Description = description;

            return Task.CompletedTask;
        }
    }
}
