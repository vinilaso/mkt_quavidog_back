namespace Sienna.Domain.Abstractions.Pagination
{
    public sealed record PageRequest
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 10;

        public int Page { get; }
        public int PageSize { get; }

        public int Skip => (Page - 1) * PageSize;

        public PageRequest(int? page = default, int? pageSize = default)
        {
            Page = Math.Max(page.GetValueOrDefault(1), 1);
            PageSize = Math.Clamp(pageSize.GetValueOrDefault(DefaultPageSize), 1, MaxPageSize);
        }
    }
}
