using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.GetTeamAccount
{
    public sealed class GetTeamInstagramAccountHandler(IInstagramAccountRepository accountRepository) : IRequestHandler<GetTeamInstagramAccountQuery, Result<InstagramConnectionResponse>>
    {
        public async Task<Result<InstagramConnectionResponse>> Handle(GetTeamInstagramAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.FindByTeamIdAsync(request.TeamId, cancellationToken);

            return account is null
                ? InstagramConnectionResponse.NotConfigured
                : InstagramConnectionResponse.From(account);
        }
    }
}
