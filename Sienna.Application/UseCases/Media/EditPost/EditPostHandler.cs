using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Media.EditPost
{
    internal sealed class EditPostHandler(
        IUserContext userContext,
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IUnitOfWork uow) : IRequestHandler<EditPostCommand, Result>
    {
        public async Task<Result> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var postResult = await EditablePostLoader.LoadAsync(request.PostId, userContext, postRepository, publicationRepository, cancellationToken);

            if (postResult.IsFailure)
                return postResult.Error;

            postResult.Value.UpdateCaption(request.Caption);

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
