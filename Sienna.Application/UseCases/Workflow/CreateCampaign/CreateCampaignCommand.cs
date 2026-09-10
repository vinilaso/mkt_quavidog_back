using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.CreateCampaign
{
    public record CreateCampaignCommand(string CampaignName, Guid TeamId) : IRequest<Result<Guid>>;
}
