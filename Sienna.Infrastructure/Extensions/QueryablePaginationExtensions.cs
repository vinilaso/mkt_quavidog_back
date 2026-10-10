using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Pagination;

namespace Sienna.Infrastructure.Extensions
{
    internal static class QueryablePaginationExtensions
    {
        internal static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IOrderedQueryable<T> query, PageRequest page, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = totalCount > 0
                ? await query.Skip(page.Skip).Take(page.PageSize).ToListAsync(cancellationToken)
                : [];

            return new PagedResult<T>(items, page.Page, page.PageSize, totalCount);
        }
    }
}
