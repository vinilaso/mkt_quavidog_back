using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.AssignPostToCampaign
{
    public sealed class AssignPostToCampaignHandler(
        IPostRepository postRepository, 
        ICampaignRepository campaignRepository,
        IUnitOfWork uow) : IRequestHandler<AssignPostToCampaignCommand, Result>
    {
        public async Task<Result> Handle(AssignPostToCampaignCommand request, CancellationToken cancellationToken)
        {
            var post = await postRepository.FindByIdAsync(request.PostId, cancellationToken);

            if (post is null)
                return Error.NotFound("Post.NotFound", $"Não foi encontrada uma postagem no servidor com o ID '{request.PostId}'.");
            
            var campaign = await campaignRepository.FindByIdAsync(request.CampaignId, cancellationToken);

            if (campaign is null)
                return Error.NotFound("Campaign.NotFound", $"Não foi encontrada uma campanha no servidor com o ID '{request.CampaignId}'.");

            var addResult = campaign.TryAddPost(post.Id);

            if (addResult.IsFailure)
                return addResult.Error;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
