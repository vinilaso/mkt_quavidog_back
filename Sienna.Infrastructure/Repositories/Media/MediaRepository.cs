using Sienna.Domain.Abstractions.Media.Repositories;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Infrastructure.Repositories.Media
{
    internal class MediaRepository(ApplicationContext context) : AbstractRepository<MediaEntity>(context), IMediaRepository
    {
    }
}
