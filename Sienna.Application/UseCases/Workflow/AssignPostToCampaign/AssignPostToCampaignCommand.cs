using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.AssignPostToCampaign
{
    public record AssignPostToCampaignCommand(Guid CampaignId, Guid PostId) : IRequest<Result>;
}
