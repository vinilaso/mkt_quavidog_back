using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram
{
    /// <summary>Conta do Instagram conectada ao time. O token de acesso nunca é retornado.</summary>
    /// <param name="InstagramUserId">ID da conta no Instagram.</param>
    /// <param name="Username">Nome de usuário da conta, sem o "@".</param>
    /// <param name="ConnectedAt">Quando a conta foi conectada ao time (UTC).</param>
    /// <param name="UpdatedAt">Última atualização do token (UTC). O token da Meta vale cerca de 60 dias.</param>
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
