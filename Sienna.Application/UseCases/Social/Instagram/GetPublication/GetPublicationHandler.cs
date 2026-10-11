using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.GetPublication
{
    public sealed class GetPublicationHandler(IPostPublicationRepository publicationRepository) : IRequestHandler<GetPublicationQuery, Result<PublicationDetailsDTO>>
    {
        public async Task<Result<PublicationDetailsDTO>> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.GetDetailsAsync(request.TeamId, request.PublicationId, cancellationToken) is not PublicationDetailsDTO details)
                return PublicationErrors.NotFound;

            return details;
        }
    }
}
