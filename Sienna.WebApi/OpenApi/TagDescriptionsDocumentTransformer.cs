using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Sienna.WebApi.OpenApi
{
    /// <summary>
    /// Adiciona a descrição de cada grupo (tag) ao documento OpenAPI.
    /// </summary>
    internal sealed class TagDescriptionsDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Tags ??= new HashSet<OpenApiTag>();

            foreach (var (name, description) in ApiTags.All)
            {
                var tag = document.Tags.FirstOrDefault(existing => existing.Name == name);

                if (tag is null)
                    document.Tags.Add(new OpenApiTag { Name = name, Description = description });
                else
                    tag.Description = description;
            }

            return Task.CompletedTask;
        }
    }
}
