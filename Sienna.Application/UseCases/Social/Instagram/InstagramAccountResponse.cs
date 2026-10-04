using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram
{
    public record InstagramAccountResponse(string InstagramUserId, string Username, DateTime ConnectedAt, DateTime UpdatedAt)
    {
        internal static InstagramAccountResponse From(InstagramAccount account)
        {
            ArgumentNullException.ThrowIfNull(account);

            return new InstagramAccountResponse(
                InstagramUserId: account.InstagramUserId,
                Username: account.Username,
                ConnectedAt: account.ConnectedAt,
                UpdatedAt: account.UpdatedAt
            );
        }
    }
}
