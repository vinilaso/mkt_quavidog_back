using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Media;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.AssignPostToCampaign
{
    public sealed class AssignPostToCampaignHandler(
        IPostRepository postRepository, 
        ICampaignRepository campaignRepository,
        IUserContext userContext,
        IUnitOfWork uow) : IRequestHandler<AssignPostToCampaignCommand, Result>
    {
        public async Task<Result> Handle(AssignPostToCampaignCommand request, CancellationToken cancellationToken)
        {
            if (await campaignRepository.FindByIdAsync(request.CampaignId, cancellationToken) is not Campaign campaign || campaign.TeamId != request.TeamId)
                return Error.NotFound("Campaign.NotFound", "Não foi encontrada uma campanha do time com o ID informado.");

            if (await postRepository.FindByIdAsync(request.PostId, cancellationToken) is not Post post)
                return Error.NotFound("Post.NotFound", "Não foi encontrada uma postagem com o ID informado.");

            if (post.AuthorId != userContext.Id)
                return Error.Forbidden("Post.NotAuthor", "Apenas o autor da postagem pode adicioná-la a uma campanha.");

            var addResult = campaign.TryAddPost(post.Id);

            if (addResult.IsFailure)
                return addResult.Error;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
