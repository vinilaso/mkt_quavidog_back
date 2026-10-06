using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.GetTeamAccount
{
    /// <summary>Situação da integração do time com o Instagram.</summary>
    /// <param name="IsConfigured">Indica se o time possui uma conta do Instagram conectada.</param>
    /// <param name="Account">Dados da conta conectada, ou nulo quando isConfigured é falso.</param>
    public record InstagramConnectionResponse(bool IsConfigured, InstagramAccountResponse? Account) 
    {
        internal static readonly InstagramConnectionResponse NotConfigured = new(false, null);
        internal static InstagramConnectionResponse From(InstagramAccount account) => new(true, InstagramAccountResponse.From(account));
    }
}
