using MediatR;
using Sienna.Application.Interfaces.Social.Signal;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.ApprovePublication
{
    public sealed class ApprovePublicationHandler(
        IUserContext userContext,
        IPostPublicationRepository publicationRepository,
        IPublicationSignal publicationSignal,
        IUnitOfWork uow) : IRequestHandler<ApprovePublicationCommand, Result<PublicationResponse>>
    {
        public async Task<Result<PublicationResponse>> Handle(ApprovePublicationCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindTeamPublicationByIdAsync(request.TeamId, request.PublicationId, cancellationToken) is not PostPublication publication)
                return PublicationErrors.NotFound;

            var approveResult = publication.Approve(userContext.Id, DateTime.UtcNow);

            if (approveResult.IsFailure)
                return approveResult.Error;

            await uow.CommitChangesAsync(cancellationToken);

            publicationSignal.Notify();

            return PublicationResponse.From(publication);
        }
    }
}
