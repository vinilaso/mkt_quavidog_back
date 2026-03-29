using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Sienna.WebApi.OpenApi
{
    internal sealed class BearerSecuritySchemeDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Components ??= new();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes.TryAdd("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            return Task.CompletedTask;
        }
    }
}
