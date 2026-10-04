using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.GetTeamAccount
{
    public record InstagramConnectionResponse(bool IsConfigured, InstagramAccountResponse? Account) 
    {
        internal static readonly InstagramConnectionResponse NotConfigured = new(false, null);
        internal static InstagramConnectionResponse From(InstagramAccount account) => new(true, InstagramAccountResponse.From(account));
    }
}
