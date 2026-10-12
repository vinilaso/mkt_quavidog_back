using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Infrastructure.Repositories.Social
{
    internal sealed class PublicationInsightSnapshotRepository(ApplicationContext context) : AbstractRepository<PublicationInsightSnapshot>(context), IPublicationInsightSnapshotRepository
    {
    }
}
