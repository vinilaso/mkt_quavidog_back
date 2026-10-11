using MediatR;
using Sienna.Application.Interfaces.Social.Signal;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using Sienna.Domain.Exceptions.Persistence;

namespace Sienna.Application.UseCases.Social.Instagram.ReschedulePublication
{
    public sealed class ReschedulePublicationHandler(
        IUserContext userContext,
        ITeamAccessContext teamAccess,
        IPostPublicationRepository publicationRepository,
        IPublicationSignal publicationSignal,
        IUnitOfWork uow) : IRequestHandler<ReschedulePublicationCommand, Result<PublicationResponse>>
    {
        public async Task<Result<PublicationResponse>> Handle(ReschedulePublicationCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindTeamPublicationByIdAsync(request.TeamId, request.PublicationId, cancellationToken) is not PostPublication publication)
                return PublicationErrors.NotFound;

            var permission = PublicationPermissions.EnsureCanChange(publication, userContext.Id, teamAccess);

            if (permission.IsFailure)
                return permission.Error;

            var rescheduleResult = publication.Reschedule(request.ScheduledFor, teamAccess.IsManager, DateTime.UtcNow);

            if (rescheduleResult.IsFailure)
                return rescheduleResult.Error;

            try
            {
                await uow.CommitChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                return PublicationErrors.ConcurrentUpdate;
            }

            if (publication.Status is PublicationStatus.Approved)
                publicationSignal.Notify();

            return PublicationResponse.From(publication);
        }
    }
}
