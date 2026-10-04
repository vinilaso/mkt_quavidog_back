using MediatR;
using Sienna.Application.Interfaces.Security;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.ConfigureAccount
{
    public sealed class ConfigureInstagramAccountHandler(
        IInstagramClient instagramClient,
        ISecretProvider secretProvider,
        IInstagramAccountRepository accountRepository,
        IUnitOfWork uow) : IRequestHandler<ConfigureInstagramAccountCommand, Result<InstagramAccountResponse>>
    {
        private const string InstagramApiErrorCode = "Instagram.ApiError";

        public async Task<Result<InstagramAccountResponse>> Handle(ConfigureInstagramAccountCommand request, CancellationToken cancellationToken)
        {
            var accessToken = request.AccessToken?.Trim();

            if (string.IsNullOrWhiteSpace(accessToken))
                return Error.Validation("Instagram.TokenRequired", "O token de acesso do Instagram é obrigatório.");

            var profileResult = await instagramClient.GetProfileAsync(accessToken, cancellationToken);

            if (profileResult.IsFailure)
            {
                if (profileResult.Error.Code == InstagramApiErrorCode)
                    return Error.Validation("Instagram.InvalidToken", $"O token foi recusado pela Meta: {profileResult.Error.Message}");

                return profileResult.Error;
            }

            var profile = profileResult.Value;

            if (profile.AccountType is not ("BUSINESS" or "MEDIA_CREATOR"))
                return Error.Validation("Instagram.NotProfessional", $"A conta @{profile.Username} precisa ser Business ou Creator.");

            var protectedToken = secretProvider.Protect(accessToken);

            if (await accountRepository.FindByTeamIdAsync(request.TeamId, cancellationToken) is InstagramAccount account)
            {
                account.UpdateCredentials(profile.UserId, profile.Username, protectedToken);
            }
            else
            {
                account = new InstagramAccount(request.TeamId, profile.UserId, profile.Username, protectedToken);
                await accountRepository.AddAsync(account, cancellationToken);
            }

            await uow.CommitChangesAsync(cancellationToken);
            return InstagramAccountResponse.From(account);
        }
    }
}
