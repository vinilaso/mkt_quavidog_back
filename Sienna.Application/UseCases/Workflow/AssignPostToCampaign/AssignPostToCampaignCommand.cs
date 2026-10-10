using MediatR;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.AssignPostToCampaign
{
    public record AssignPostToCampaignCommand(Guid TeamId, Guid CampaignId, Guid PostId) : ITeamScopedRequest, IRequest<Result>;
}
