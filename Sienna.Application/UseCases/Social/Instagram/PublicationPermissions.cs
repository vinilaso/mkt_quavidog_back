using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram
{
    internal static class PublicationPermissions
    {
        internal static Result EnsureCanChange(PostPublication publication, Guid userId, ITeamAccessContext teamAccess)
        {
            if (publication.RequestedById == userId || teamAccess.IsManager)
                return Result.Success();

            return PublicationErrors.NotRequester;
        }
    }
}
