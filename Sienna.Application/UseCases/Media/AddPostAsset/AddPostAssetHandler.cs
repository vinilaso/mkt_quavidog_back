using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Media;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Application.UseCases.Media.AddPostAsset
{
    internal sealed class AddPostAssetHandler(
        IPostRepository postRepository,
        IMediaRepository mediaRepository,
        IUnitOfWork uow) : IRequestHandler<AddPostAssetCommand, Result<Guid>>
    {
        private const long MaxFileSizeBytes = 8 * 1024 * 1024;
        private static readonly string[] SupportedExtensions = [".jpg", ".jpeg"];

        public async Task<Result<Guid>> Handle(AddPostAssetCommand request, CancellationToken cancellationToken)
        {
            if (!SupportedExtensions.Contains(request.Extension, StringComparer.OrdinalIgnoreCase))
                return Error.Validation("Asset.UnsupportedFormat", "Envie uma imagem JPEG (.jpg ou .jpeg).");

            if (request.Content.CanSeek && request.Content.Length > MaxFileSizeBytes)
                return FileTooLarge();

            if (await postRepository.FindByIdAsync(request.PostId, cancellationToken) is not Post post)
                return Error.NotFound("Post.NotFound", $"Não foi encontrada uma postagem no servidor com o ID {request.PostId}.");

            using var memoryStream = new MemoryStream();
            await request.Content.CopyToAsync(memoryStream, cancellationToken);

            if (memoryStream.Length == 0)
                return Error.Validation("Asset.EmptyFile", "O arquivo enviado está vazio.");

            if (memoryStream.Length > MaxFileSizeBytes)
                return FileTooLarge();

            var media = new MediaEntity
            {
                Name = request.FileName,
                Extension = request.Extension.ToLowerInvariant(),
                Content = memoryStream.ToArray()
            };

            var assetResult = post.TryAddAsset(media.Id, request.SequenceOrder);

            if (assetResult.IsFailure)
                return assetResult.Error;

            await mediaRepository.AddAsync(media, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            return media.Id;
        }

        private static Error FileTooLarge() =>
            Error.Validation("Asset.FileTooLarge", $"A imagem deve ter no máximo {MaxFileSizeBytes / (1024 * 1024)} MB.");
    }
}
