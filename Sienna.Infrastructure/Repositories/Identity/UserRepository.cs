using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Media;

namespace Sienna.Infrastructure.Repositories.Identity
{
    internal class UserRepository(ApplicationContext context) : AbstractRepository<User>(context), IUserRepository
    {
        public async Task<IEnumerable<PostDTO>> GetUserPostsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Post>()
                .Where(post => post.AuthorId == userId)
                .Select(post => new PostDTO
                {
                    Caption = post.Caption,
                    CreatedAt = post.CreatedAt,
                    Id = post.Id,
                    Status = post.Status.ToString(),
                    Assets = post.Assets.Select(asset => new AssetDTO
                    {
                        Media = new MediaDTO 
                        { 
                            Id = asset.MediaId.GetValueOrDefault(), 
                            FileName = asset.Media.Name + asset.Media.Extension 
                        },
                        SequenceOrder = asset.SequenceOrder
                    })
                })
                .ToListAsync(cancellationToken);
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
                        TeamId = team.Team.Id,
                        TeamName = team.Team.Name,
                        Role = team.Role.ToString()
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
