using Microsoft.AspNetCore.Mvc;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    public readonly record struct PublicationFilterParameters(
        [FromQuery] PublicationStatus[]? Status,
        [FromQuery] PublicationFormat? Format,
        [FromQuery] DateTimeOffset? From,
        [FromQuery] DateTimeOffset? To,
        [FromQuery] SortDirection? Order
        )
    {
        public PublicationFilter ToFilter() => new()
        {
            Statuses = Status ?? [],
            Format = Format,
            From = From?.UtcDateTime,
            To = To?.UtcDateTime,
            Order = Order ?? SortDirection.Descending
        };
    }
}
