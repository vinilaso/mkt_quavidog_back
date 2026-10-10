using Microsoft.AspNetCore.Mvc;
using Sienna.Domain.Abstractions.Pagination;

namespace Sienna.WebApi.Endpoints.Models.Pagination
{
    /// <summary>
    /// Parâmetros de paginação lidos da query string (?page=1&amp;pageSize=20)
    /// </summary>
    /// <param name="Page">Número da página, começando em 1. Padrão: 1.</param>
    /// <param name="PageSize">Itens por página, de 1 a 100. Padrão: 20.</param>
    public readonly record struct PaginationParameters(
        [FromQuery] int? Page,
        [FromQuery] int? PageSize
    )
    {
        public PageRequest ToPageRequest() => new(Page, PageSize);
    }
}
