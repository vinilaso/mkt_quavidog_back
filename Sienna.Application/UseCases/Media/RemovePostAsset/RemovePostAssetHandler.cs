using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Application.UseCases.Media.RemovePostAsset
{
    internal sealed class RemovePostAssetHandler(
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IMediaRepository mediaRepository,
        IUserContext userContext,
        IUnitOfWork uow) : IRequestHandler<RemovePostAssetCommand, Result>
    {
        public async Task<Result> Handle(RemovePostAssetCommand request, CancellationToken cancellationToken)
        {
            var postResult = await EditablePostLoader.LoadAsync(request.PostId, userContext, postRepository, publicationRepository, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            var removeResult = postResult.Value.RemoveAsset(request.MediaId);

            if (removeResult.IsFailure)
                return removeResult.Error;

            // A mídia foi criada para esta postagem; só é apagada se nenhuma outra postagem a usar.
            if (await mediaRepository.FindByIdAsync(request.MediaId, cancellationToken) is MediaEntity media &&
                !await mediaRepository.IsLinkedToOtherPostsAsync(media.Id, request.PostId, cancellationToken))
            {
                mediaRepository.Remove(media);
            }

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
