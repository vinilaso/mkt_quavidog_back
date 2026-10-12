using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights
{
    public sealed record CollectPublicationInsightsCommand(Guid PublicationId) : ICommand<Result<CollectPublicationInsightsResponse>>;
}
