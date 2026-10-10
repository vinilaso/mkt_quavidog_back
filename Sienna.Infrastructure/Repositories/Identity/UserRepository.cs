using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Media;
using Sienna.Infrastructure.Extensions;

namespace Sienna.Infrastructure.Repositories.Identity
{
    internal class UserRepository(ApplicationContext context) : AbstractRepository<User>(context), IUserRepository
    {
        public async Task<PagedResult<PostDTO>> GetUserPostsAsync(Guid userId, PageRequest page, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Post>()
                .Where(post => post.AuthorId == userId)
                .Select(post => new PostDTO
                {
                    Id = post.Id,
                    Caption = post.Caption,
                    CreatedAt = post.CreatedAt,
                    Status = post.Status.ToString(),
                    Assets = post.Assets
                        .OrderBy(asset => asset.SequenceOrder)
                        .Select(asset => new AssetDTO
                        {
                            SequenceOrder = asset.SequenceOrder,
                            Media = new MediaDTO
                            {
                                Id = asset.Media!.Id,
                                FileName = asset.Media!.Name + asset.Media.Extension
                            }
                        })
                })
                .OrderByDescending(dto => dto.CreatedAt)
                .ThenBy(dto => dto.Id)
                .ToPagedResultAsync(page, cancellationToken);
        }

        public async Task<UserTeamsDTO?> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<User>()
                .Where(user => user.Id == userId)
                .Select(user => new UserTeamsDTO
                {
                    UserId = user.Id,
                    Teams = user.Teams.Select(team => new UserTeamDTO
                    {
                        TeamId = team.Team!.Id,
                        TeamName = team.Team.Name,
                        Role = team.Role.ToString()
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
