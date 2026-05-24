using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.AddPostAsset
{
    internal sealed class AddPostAssetHandler(
        IPostRepository postRepository, 
        IUnitOfWork uow) : IRequestHandler<AddPostAssetCommand, Result>
    {
        public async Task<Result> Handle(AddPostAssetCommand request, CancellationToken cancellationToken)
        {
            var post = await postRepository.FindByIdAsync(request.PostId, cancellationToken);

            if (post is null)
                return Error.NotFound("Post.NotFound", $"Não foi encontrada uma postagem no servidor com o ID {request.PostId}.");

            var assetResult = post.TryAddAsset(request.MediaId, request.SequenceOrder);

            if (assetResult.IsFailure)
                return assetResult.Error;

            await uow.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
