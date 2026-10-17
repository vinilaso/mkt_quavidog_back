using Sienna.Application.UseCases.Social.Instagram;
using Sienna.Domain.Abstractions.Results;
using Sienna.Tests.Common.Values;
using System.Security.Cryptography;

namespace Sienna.Application.UnitTests.UseCases.Social.Instagram
{
    [TestClass]
    public sealed class InstagramCredentialsResolverTests
    {
        private readonly TestContext _testContext;
        private readonly InstagramMocks _instagram = new();
        private readonly InstagramCredentialsResolver _resolver;
        private readonly Guid _teamId = Guid.NewGuid();

        public InstagramCredentialsResolverTests(TestContext testContext)
        {
            _testContext = testContext;
            _resolver = _instagram.CreateCredentialsResolver();
        }

        [TestMethod]
        public async Task ResolveAsync_WithoutConnectedAccount_ReturnsNotConfigured()
        {
            var result = await _resolver.ResolveAsync(_teamId, _testContext.CancellationToken);

            ResultAssert.Failure(result, "Instagram.NotConfigured", ErrorType.Validation);
        }

        [TestMethod]
        public async Task ResolveAsync_ReturnsUserIdAndDecryptedToken()
        {
            _instagram.GivenConnectedAccount(_teamId);

            var result = await _resolver.ResolveAsync(_teamId, _testContext.CancellationToken);

            var credentials = ResultAssert.Success(result);
            Assert.AreEqual(TestValues.Instagram.UserId, credentials.UserId);
            Assert.AreEqual(TestValues.Instagram.AccessToken, credentials.AccessToken);
        }

        [TestMethod]
        [DataRow(typeof(CryptographicException))]
        [DataRow(typeof(FormatException))]
        public async Task ResolveAsync_WhenTokenCannotBeDecrypted_ReturnsUnreadableToken(Type exceptionType)
        {
            _instagram.GivenConnectedAccount(_teamId);
            _instagram.GivenUnreadableToken((Exception)Activator.CreateInstance(exceptionType)!);

            var result = await _resolver.ResolveAsync(_teamId, _testContext.CancellationToken);

            ResultAssert.Failure(result, "Instagram.UnreadableToken", ErrorType.Validation);
        }
    }
}
