using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram
{
    internal static class PublicationMediaRules
    {
        private static readonly string[] SupportedExtensions = [".jpg", ".jpeg"];

        internal static async Task<Result> ValidateAsync(IMediaRepository mediaRepository, IReadOnlyCollection<Guid> mediaIds, CancellationToken cancellationToken)
        {
            var extensions = await mediaRepository.GetExtensionsAsync(mediaIds, cancellationToken);

            if (extensions.Count != mediaIds.Distinct().Count())
                return Error.Validation("Publication.MediaNotFound", "Uma ou mais imagens da postagem não foram encontradas.");

            if (extensions.Values.Any(extension => !SupportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)))
                return Error.Validation("Publication.UnsupportedMedia", "O Instagram só aceita imagens JPEG (.jpg ou .jpeg).");

            return Result.Success();
        }
    }
}
