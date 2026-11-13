using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Media;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Application.UseCases.Media.ReplacePostAsset
{
    internal sealed class ReplacePostAssetHandler(
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IMediaRepository mediaRepository,
        IUserContext userContext,
        IUnitOfWork uow) : IRequestHandler<ReplacePostAssetCommand, Result>
    {
        public async Task<Result> Handle(ReplacePostAssetCommand request, CancellationToken cancellationToken)
        {
            var fileResult = PostAssetFileRules.Validate(request.Extension, request.Content);

            if (fileResult.IsFailure)
                return fileResult.Error;

            var postResult = await EditablePostLoader.LoadAsync(request.PostId, userContext, postRepository, publicationRepository, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            if (!postResult.Value.HasAsset(request.MediaId) ||
                await mediaRepository.FindByIdAsync(request.MediaId, cancellationToken) is not MediaEntity media)
                return Post.AssetNotFound(request.MediaId);

            var contentResult = await PostAssetFileRules.ReadAsync(request.Content, cancellationToken);

            if (contentResult.IsFailure)
                return contentResult.Error;

            media.Name = request.FileName;
            media.Extension = request.Extension.ToLowerInvariant();
            media.Content = contentResult.Value;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
