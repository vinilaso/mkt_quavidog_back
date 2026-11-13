using Sienna.Application.UseCases.Media.ReorderPostAssets;

namespace Sienna.WebApi.Endpoints.Models.Media
{
    /// <summary>Nova ordem das imagens de uma postagem.</summary>
    /// <param name="MediaIds">IDs de todas as mídias da postagem, cada uma uma única vez, na ordem desejada. A primeira fica na posição 1.</param>
    public record ReorderPostAssetsRequest(IReadOnlyList<Guid> MediaIds)
    {
        public ReorderPostAssetsCommand ToCommand(Guid postId) => new(postId, MediaIds ?? []);
    }
}
