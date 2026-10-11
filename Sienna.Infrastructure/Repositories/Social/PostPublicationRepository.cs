using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Social;
using Sienna.Infrastructure.Extensions;

namespace Sienna.Infrastructure.Repositories.Social
{
    internal sealed class PostPublicationRepository(ApplicationContext applicationContext) : AbstractRepository<PostPublication>(applicationContext), IPostPublicationRepository
    {
        private static readonly PublicationStatus[] BlockingStatuses =
        [
            PublicationStatus.PendingApproval,
            PublicationStatus.Approved,
            PublicationStatus.Publishing,
            PublicationStatus.Published
        ];

        public async Task<IReadOnlyList<Guid>> ClaimDueAsync(DateTime utcNow, int maxCount, CancellationToken cancellationToken = default)
        {
            var candidateIds = await Context.Set<PostPublication>()
                .Where(publication => publication.Status == PublicationStatus.Approved && publication.ScheduledFor <= utcNow)
                .OrderBy(publication => publication.ScheduledFor)
                .Select(publication => publication.Id)
                .Take(maxCount)
                .ToListAsync(cancellationToken);

            List<Guid> claimed = [];

            foreach (var id in candidateIds)
            {
                var affected = await Context.Set<PostPublication>()
                    .Where(publication => publication.Id == id && publication.Status == PublicationStatus.Approved)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(publication => publication.Status, PublicationStatus.Publishing)
                        .SetProperty(publication => publication.ConcurrencyStamp, Guid.NewGuid()), 
                        cancellationToken);

                if (affected == 1)
                    claimed.Add(id);
            }

            return claimed;
        }

        public async Task<int> FailInterruptedAsync(string reason, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .Where(publication => publication.Status == PublicationStatus.Publishing)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(publication => publication.Status, PublicationStatus.Failed)
                    .SetProperty(publication => publication.FailureReason, reason)
                    .SetProperty(publication => publication.ConcurrencyStamp, Guid.NewGuid()), 
                    cancellationToken);
        }

        public async Task<PostPublication?> FindForPublishingAsync(Guid publicationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .Include(publication => publication.Post)
                    .ThenInclude(post => post!.Assets)
                .FirstOrDefaultAsync(publication => publication.Id == publicationId, cancellationToken);
        }

        public async Task<PostPublication?> FindTeamPublicationByIdAsync(Guid teamId, Guid publicationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .FirstOrDefaultAsync(publication => publication.TeamId == teamId && publication.Id == publicationId, cancellationToken);
        }

        public async Task<PublicationDetailsDTO?> GetDetailsAsync(Guid teamId, Guid publicationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .Where(publication => publication.TeamId == teamId && publication.Id == publicationId)
                .Select(publication => new PublicationDetailsDTO
                {
                    Id = publication.Id,
                    PostId = publication.PostId,
                    Caption = publication.Post!.Caption,

                    Assets = publication.Post.Assets
                        .OrderBy(asset => asset.SequenceOrder)
                        .Select(asset => new AssetDTO
                        {
                            SequenceOrder = asset.SequenceOrder,
                            Media = new MediaDTO
                            {
                                Id = asset.Media!.Id,
                                FileName = asset.Media.Name + asset.Media.Extension
                            }
                        }),

                    Format = publication.Format,
                    Status = publication.Status,
                    ScheduledFor = publication.ScheduledFor,
                    RequestedById = publication.RequestedById,

                    RequestedByName = Context.Set<User>()
                        .Where(user => user.Id == publication.RequestedById)
                        .Select(user => user.FullName)
                        .FirstOrDefault() ?? string.Empty,

                    RequestedAt = publication.RequestedAt,
                    ReviewedById = publication.ReviewedById,

                    ReviewedByName = Context.Set<User>()
                        .Where(user => user.Id == publication.ReviewedById)
                        .Select(user => user.FullName)
                        .FirstOrDefault() ?? string.Empty,

                    ReviewedAt = publication.ReviewedAt,
                    RejectionReason = publication.RejectionReason,
                    PublishedAt = publication.PublishedAt,
                    ExternalIds = publication.ExternalIds,
                    FailureReason = publication.FailureReason
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> HasBlockingPublicationAsync(Guid postId, PublicationFormat format, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .AnyAsync(publication =>
                    publication.PostId == postId &&
                    publication.Format == format &&
                    BlockingStatuses.Contains(publication.Status),
                    cancellationToken);
        }

        public async Task<PagedResult<PublicationSummaryDTO>> SearchAsync(Guid teamId, PublicationFilter filter, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = Context.Set<PostPublication>()
                .Where(publication => publication.TeamId == teamId);

            if (filter.Statuses.Count > 0)
            {
                var statuses = filter.Statuses.ToArray();
                query = query.Where(publication => statuses.Contains(publication.Status));
            }

            if (filter.Format is PublicationFormat format)
                query = query.Where(publication => publication.Format == format);

            if (filter.From is DateTime from)
                query = query.Where(publication => publication.ScheduledFor >= from);

            if (filter.To is DateTime to)
                query = query.Where(publication => publication.ScheduledFor <= to);

            var projected = query.Select(publication => new PublicationSummaryDTO
            {
                Id = publication.Id,
                PostId = publication.PostId,
                Caption = publication.Post!.Caption,

                ThumbnailMediaId = publication.Post.Assets
                    .OrderBy(asset => asset.SequenceOrder)
                    .Select(asset => asset.MediaId)
                    .FirstOrDefault(),

                MediaCount = publication.Post.Assets.Count,
                Format = publication.Format,
                Status = publication.Status,
                ScheduledFor = publication.ScheduledFor,
                RequestedById = publication.RequestedById,

                RequestedByName = Context.Set<User>()
                    .Where(user => user.Id == publication.RequestedById)
                    .Select(user => user.FullName)
                    .FirstOrDefault() ?? string.Empty,

                RequestedAt = publication.RequestedAt,
                PublishedAt = publication.PublishedAt
            });

            var ordered = filter.Order is SortDirection.Ascending
                ? projected.OrderBy(dto => dto.ScheduledFor).ThenBy(dto => dto.Id)
                : projected.OrderByDescending(dto => dto.ScheduledFor).ThenBy(dto => dto.Id);

            return await ordered.ToPagedResultAsync(page, cancellationToken);
        }
    }
}
