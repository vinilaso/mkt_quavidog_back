using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Sienna.WebApi.OpenApi
{
    internal sealed class BearerSecurityRequirementOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var metadata = context.Description.ActionDescriptor.EndpointMetadata;

            bool hasAuthorize = metadata.OfType<IAuthorizeData>().Any();
            bool hasAllowAnonymous = metadata.OfType<IAllowAnonymous>().Any();

            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Security ??= [];
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer")] = []
                });
            }

            return Task.CompletedTask;
        }
    }
}
