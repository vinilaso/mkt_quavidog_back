using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute.ExceptionExtensions;
using Sienna.Application.Interfaces.Security;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Application.UseCases.Social.Instagram;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Builders.Social;
using Sienna.Tests.Common.Values;

namespace Sienna.Application.UnitTests.UseCases.Social.Instagram
{
    /// <summary>
    /// Dublês do Instagram reutilizáveis por todos os handlers da área (coleta, publicação, conexão de conta...).
    /// Por padrão, o token protegido de <see cref="TestValues.Instagram"/> é decifrado normalmente.
    /// </summary>
    internal sealed class InstagramMocks
    {
        public static InstagramCredentials Credentials { get; } = new(TestValues.Instagram.UserId, TestValues.Instagram.AccessToken);

        public IInstagramAccountRepository Accounts { get; } = Substitute.For<IInstagramAccountRepository>();
        public ISecretProvider SecretProvider { get; } = Substitute.For<ISecretProvider>();
        public IInstagramClient Client { get; } = Substitute.For<IInstagramClient>();

        public InstagramMocks()
        {
            SecretProvider.Unprotect(TestValues.Instagram.ProtectedToken).Returns(TestValues.Instagram.AccessToken);
        }

        public InstagramCredentialsResolver CreateCredentialsResolver() =>
            new(Accounts, SecretProvider, NullLogger<InstagramCredentialsResolver>.Instance);

        public void GivenConnectedAccount(Guid teamId) =>
            Accounts.FindByTeamIdAsync(teamId, Arg.Any<CancellationToken>())
                .Returns(InstagramAccounts.Connected(teamId));

        public void GivenUnreadableToken(Exception exception) =>
            SecretProvider.Unprotect(Arg.Any<string>()).Throws(exception);

        public void GivenInsights(string mediaId, InsightMetrics metrics) =>
            Client.GetMediaInsightsAsync(Arg.Any<InstagramCredentials>(), mediaId, Arg.Any<PublicationFormat>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success(metrics));

        public void GivenInsightsFailure(string mediaId, Error error) =>
            Client.GetMediaInsightsAsync(Arg.Any<InstagramCredentials>(), mediaId, Arg.Any<PublicationFormat>(), Arg.Any<CancellationToken>())
                .Returns(Result.Failure<InsightMetrics>(error));

        public Task DidNotRequestInsights() =>
            Client.DidNotReceiveWithAnyArgs().GetMediaInsightsAsync(default!, default!, default, default);
    }
}
