using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Builders.Social;
using Sienna.Tests.Common.Values;

namespace Sienna.Application.UnitTests.UseCases.Social.Instagram.CollectPublicationInsights
{
    [TestClass]
    public sealed class CollectPublicationInsightsHandlerTests
    {
        private readonly TestContext _testContext;
        private readonly IPostPublicationRepository _publications = Substitute.For<IPostPublicationRepository>();
        private readonly IPublicationInsightSnapshotRepository _snapshots = Substitute.For<IPublicationInsightSnapshotRepository>();
        private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
        private readonly InstagramMocks _instagram = new();
        private readonly CollectPublicationInsightsHandler _handler;

        public CollectPublicationInsightsHandlerTests(TestContext testContext)
        {
            _testContext = testContext;
            _handler = new CollectPublicationInsightsHandler(
                _publications,
                _snapshots,
                _instagram.CreateCredentialsResolver(),
                _instagram.Client,
                _uow);
        }

        [TestMethod]
        public async Task Handle_WhenPublicationDoesNotExist_ReturnsNotFound()
        {
            var result = await Handle(Guid.NewGuid());

            ResultAssert.Failure(result, "Publication.NotFound", ErrorType.NotFound);
            await AssertNothingWasSaved();
        }

        [TestMethod]
        public async Task Handle_WhenPublicationIsNotPublished_ReturnsConflict()
        {
            var publication = GivenPublication(new PostPublicationBuilder().Approved().Build());

            var result = await Handle(publication.Id);

            ResultAssert.Failure(result, "Insights.NotPublished", ErrorType.Conflict);
            await _instagram.DidNotRequestInsights();
        }

        [TestMethod]
        public async Task Handle_WhenTeamHasNoInstagramAccount_ReturnsNotConfigured()
        {
            var publication = GivenPublication(new PostPublicationBuilder().Published().Build(), withInstagramAccount: false);

            var result = await Handle(publication.Id);

            ResultAssert.Failure(result, "Instagram.NotConfigured");
            await AssertNothingWasSaved();
        }

        [TestMethod]
        public async Task Handle_Feed_SavesSnapshotWithCollectedMetrics()
        {
            var publication = GivenPublication(new PostPublicationBuilder().Published().Build());
            var metrics = new InsightMetrics { Views = 500, Reach = 300, Likes = 40, TotalInteractions = 60 };
            _instagram.GivenInsights(TestValues.Instagram.MediaId, metrics);

            var before = DateTime.UtcNow;
            var response = ResultAssert.Success(await Handle(publication.Id));
            var after = DateTime.UtcNow;

            Assert.AreEqual(publication.Id, response.PublicationId);
            Assert.AreEqual(metrics, response.Metrics);
            Assert.IsInRange(before, after, response.CollectedAt);
            Assert.AreEqual(DateTimeKind.Utc, response.CollectedAt.Kind);

            await _snapshots.Received(1).AddAsync(
                Arg.Is<PublicationInsightSnapshot>(snapshot =>
                    snapshot.PublicationId == publication.Id &&
                    snapshot.Views == 500 &&
                    snapshot.Reach == 300 &&
                    snapshot.Likes == 40 &&
                    snapshot.TotalInteractions == 60),
                Arg.Any<CancellationToken>());

            await _uow.Received(1).CommitChangesAsync(Arg.Any<CancellationToken>());
        }

        [TestMethod]
        public async Task Handle_Feed_UsesTeamCredentials()
        {
            var publication = GivenPublication(new PostPublicationBuilder().Published().Build());
            _instagram.GivenInsights(TestValues.Instagram.MediaId, new InsightMetrics { Views = 1 });

            await Handle(publication.Id);

            await _instagram.Client.Received(1).GetMediaInsightsAsync(
                InstagramMocks.Credentials,
                TestValues.Instagram.MediaId,
                PublicationFormat.Feed,
                Arg.Any<CancellationToken>());
        }

        [TestMethod]
        public async Task Handle_Stories_SumsMetricsOfEachStory()
        {
            var publication = GivenPublication(new PostPublicationBuilder()
                .WithFormat(PublicationFormat.Story)
                .Published("story-1", "story-2")
                .Build());

            _instagram.GivenInsights("story-1", new InsightMetrics { Views = 100, Reach = 90, Replies = 2 });
            _instagram.GivenInsights("story-2", new InsightMetrics { Views = 60, Reach = 50, Replies = null });

            var response = ResultAssert.Success(await Handle(publication.Id));

            Assert.AreEqual(160, response.Metrics.Views);
            Assert.AreEqual(140, response.Metrics.Reach);
            Assert.AreEqual(2, response.Metrics.Replies);
            Assert.IsNull(response.Metrics.Saved);

            await _instagram.Client.Received(2).GetMediaInsightsAsync(
                Arg.Any<InstagramCredentials>(), Arg.Any<string>(), PublicationFormat.Story, Arg.Any<CancellationToken>());
            await _snapshots.Received(1).AddAsync(Arg.Any<PublicationInsightSnapshot>(), Arg.Any<CancellationToken>());
        }

        [TestMethod]
        public async Task Handle_WhenOneStoryFails_SavesNothing()
        {
            var publication = GivenPublication(new PostPublicationBuilder()
                .WithFormat(PublicationFormat.Story)
                .Published("story-1", "story-2")
                .Build());

            var error = Error.External("Instagram.NetworkError", "Falha de rede.");
            _instagram.GivenInsights("story-1", new InsightMetrics { Views = 100 });
            _instagram.GivenInsightsFailure("story-2", error);

            var result = await Handle(publication.Id);

            Assert.AreEqual(error, ResultAssert.Failure(result, "Instagram.NetworkError", ErrorType.External));
            await AssertNothingWasSaved();
        }

        [TestMethod]
        public async Task Handle_WhenMetaReturnsNegativeValues_SavesNothing()
        {
            var publication = GivenPublication(new PostPublicationBuilder().Published().Build());
            _instagram.GivenInsights(TestValues.Instagram.MediaId, new InsightMetrics { Views = -5 });

            var result = await Handle(publication.Id);

            ResultAssert.Failure(result, "Insights.NegativeValue", ErrorType.Validation);
            await AssertNothingWasSaved();
        }

        #region Arranjos e verificações

        private Task<Result<CollectPublicationInsightsResponse>> Handle(Guid publicationId) =>
            _handler.Handle(new CollectPublicationInsightsCommand(publicationId), _testContext.CancellationToken);

        private PostPublication GivenPublication(PostPublication publication, bool withInstagramAccount = true)
        {
            _publications.FindByIdAsync(publication.Id, Arg.Any<CancellationToken>()).Returns(publication);

            if (withInstagramAccount)
                _instagram.GivenConnectedAccount(publication.TeamId);

            return publication;
        }

        private async Task AssertNothingWasSaved()
        {
            await _snapshots.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
            await _uow.DidNotReceiveWithAnyArgs().CommitChangesAsync(default);
        }

        #endregion
    }
}
