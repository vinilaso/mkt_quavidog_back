using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Extensions;
using Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationReviewed;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.RejectPublication
{
    public sealed class RejectPublicationHandler(
        IUserContext userContext,
        IPostPublicationRepository publicationRepository,
        IPublisher publisher,
        ILogger<RejectPublicationHandler> logger,
        IUnitOfWork uow) : IRequestHandler<RejectPublicationCommand, Result<PublicationResponse>>
    {
        public async Task<Result<PublicationResponse>> Handle(RejectPublicationCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindTeamPublicationByIdAsync(request.TeamId, request.PublicationId, cancellationToken) is not PostPublication publication)
                return PublicationErrors.NotFound;

            var rejectResult = publication.Reject(userContext.Id, request.Reason, DateTime.UtcNow);

            if (rejectResult.IsFailure)
                return rejectResult.Error;

            await uow.CommitChangesAsync(cancellationToken);
            await publisher.PublishSafelyAsync(new PublicationReviewedNotification(publication.Id), logger, cancellationToken);

            return PublicationResponse.From(publication);
        }
    }
}
