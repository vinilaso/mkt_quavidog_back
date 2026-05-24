using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Entities.Media;

namespace Sienna.Infrastructure.Repositories.Media
{
    internal sealed class PostRepository(ApplicationContext context) : AbstractRepository<Post>(context), IPostRepository
    {
        public override async Task<Post?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Post>()
                .Include(post => post.Assets)
                .FirstOrDefaultAsync(post => post.Id == id, cancellationToken);
        }
    }
}
