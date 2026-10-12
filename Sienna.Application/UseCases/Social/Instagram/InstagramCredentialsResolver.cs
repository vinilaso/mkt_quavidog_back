using Microsoft.Extensions.Logging;
using Sienna.Application.Interfaces.Security;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using System.Security.Cryptography;

namespace Sienna.Application.UseCases.Social.Instagram
{
    public sealed class InstagramCredentialsResolver(
        IInstagramAccountRepository accountRepository, 
        ISecretProvider secretProvider, 
        ILogger<InstagramCredentialsResolver> logger)
    {
        public async Task<Result<InstagramCredentials>> ResolveAsync(Guid teamId, CancellationToken cancellationToken)
        {
            if (await accountRepository.FindByTeamIdAsync(teamId, cancellationToken) is not InstagramAccount account)
                return Error.Validation("Instagram.NotConfigured", "O time não possui uma conta do Instagram conectada.");

            try
            {
                var accessToken = secretProvider.Unprotect(account.ProtectedAccessToken);
                return new InstagramCredentials(account.InstagramUserId, accessToken);
            }
            catch (Exception e) when (e is CryptographicException or FormatException)
            {
                logger.LogError(e, "Não foi possível decifrar o token do Instagram do time {TeamId}.", teamId);
                return Error.Validation("Instagram.UnreadableToken", "O token do Instagram do time não pôde ser lido. Reconecte a conta.");
            }
        }
    }
}
