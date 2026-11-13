using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Media.ReorderPostAssets
{
    internal sealed class ReorderPostAssetsHandler(
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IUserContext userContext,
        IUnitOfWork uow) : IRequestHandler<ReorderPostAssetsCommand, Result>
    {
        public async Task<Result> Handle(ReorderPostAssetsCommand request, CancellationToken cancellationToken)
        {
            var postResult = await EditablePostLoader.LoadAsync(request.PostId, userContext, postRepository, publicationRepository, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            var reorderResult = postResult.Value.ReorderAssets(request.MediaIds ?? []);

            if (reorderResult.IsFailure)
                return reorderResult.Error;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
