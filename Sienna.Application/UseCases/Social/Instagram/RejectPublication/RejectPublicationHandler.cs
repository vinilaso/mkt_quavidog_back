using MediatR;
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

            return PublicationResponse.From(publication);
        }
    }
}
