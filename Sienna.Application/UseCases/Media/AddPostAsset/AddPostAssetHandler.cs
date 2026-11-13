using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Application.UseCases.Media.AddPostAsset
{
    internal sealed class AddPostAssetHandler(
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IMediaRepository mediaRepository,
        IUserContext userContext,
        IUnitOfWork uow) : IRequestHandler<AddPostAssetCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddPostAssetCommand request, CancellationToken cancellationToken)
        {
            var fileResult = PostAssetFileRules.Validate(request.Extension, request.Content);

            if (fileResult.IsFailure)
                return fileResult.Error;

            var postResult = await EditablePostLoader.LoadAsync(request.PostId, userContext, postRepository, publicationRepository, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            var contentResult = await PostAssetFileRules.ReadAsync(request.Content, cancellationToken);

            if (contentResult.IsFailure)
                return contentResult.Error;

            var media = new MediaEntity
            {
                Name = request.FileName,
                Extension = request.Extension.ToLowerInvariant(),
                Content = contentResult.Value
            };

            var assetResult = postResult.Value.TryAddAsset(media.Id, request.SequenceOrder);

            if (assetResult.IsFailure)
                return assetResult.Error;

            await mediaRepository.AddAsync(media, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            return media.Id;
        }
    }
}
