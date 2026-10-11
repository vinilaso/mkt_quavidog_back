using MediatR;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.SearchPublication
{
    public sealed class SearchPublicationHandler(IPostPublicationRepository publicationRepository) : IRequestHandler<SearchPublicationQuery, Result<PagedResult<PublicationSummaryDTO>>>
    {
        public async Task<Result<PagedResult<PublicationSummaryDTO>>> Handle(SearchPublicationQuery request, CancellationToken cancellationToken)
        {
            if (request.Filter is { From: DateTime from, To: DateTime to } && from > to)
                return Error.Validation("Publication.InvalidPeriod", "A data inicial do período não pode ser posterior à data final.");

            return await publicationRepository.SearchAsync(request.TeamId, request.Filter, request.Page, cancellationToken);
        }
    }
}
