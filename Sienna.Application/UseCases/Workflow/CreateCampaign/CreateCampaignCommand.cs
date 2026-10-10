using MediatR;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.CreateCampaign
{
    public record CreateCampaignCommand(string CampaignName, Guid TeamId) : ITeamScopedRequest, IRequest<Result<Guid>>;
}
