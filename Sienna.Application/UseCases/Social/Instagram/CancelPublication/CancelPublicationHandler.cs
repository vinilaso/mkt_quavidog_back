using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Extensions;
using Sienna.Application.Messaging.Teams;
using Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationCanceled;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using Sienna.Domain.Exceptions.Persistence;

namespace Sienna.Application.UseCases.Social.Instagram.CancelPublication
{
    public sealed class CancelPublicationHandler(
        IUserContext userContext,
        ITeamAccessContext teamAccess,
        IPostPublicationRepository publicationRepository,
        IPublisher publisher,
        ILogger<CancelPublicationHandler> logger,
        IUnitOfWork uow) : IRequestHandler<CancelPublicationCommand, Result<PublicationResponse>>
    {
        public async Task<Result<PublicationResponse>> Handle(CancelPublicationCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindTeamPublicationByIdAsync(request.TeamId, request.PublicationId, cancellationToken) is not PostPublication publication)
                return PublicationErrors.NotFound;

            var permission = PublicationPermissions.EnsureCanChange(publication, userContext.Id, teamAccess);

            if (permission.IsFailure)
                return permission.Error;

            var cancelResult = publication.Cancel();

            if (cancelResult.IsFailure)
                return cancelResult.Error;

            try
            {
                await uow.CommitChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                return PublicationErrors.ConcurrentUpdate;
            }

            if (publication.RequestedById != userContext.Id)
                await publisher.PublishSafelyAsync(new PublicationCanceledNotification(publication.Id, userContext.Name), logger, cancellationToken);

            return PublicationResponse.From(publication);
        }
    }
}
