using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Assertions;
using Sienna.Tests.Common.Time;
using Sienna.Tests.Common.Values;

namespace Sienna.Tests.Common.Builders.Social
{
    public sealed class PostPublicationBuilder
    {
        private Guid _postId = Guid.NewGuid();
        private Guid _teamId = Guid.NewGuid();
        private Guid _requestedById = Guid.NewGuid();
        private PublicationFormat _format = PublicationFormat.Feed;
        private int _mediaCount = 1;
        private DateTime _utcNow = TestTime.Now;
        private PublicationStatus _status = PublicationStatus.Approved;
        private string[] _externalIds = [TestValues.Instagram.MediaId];

        public PostPublicationBuilder WithPost(Guid postId)
        {
            _postId = postId;
            return this;
        }

        public PostPublicationBuilder WithTeam(Guid teamId)
        {
            _teamId = teamId;
            return this;
        }

        public PostPublicationBuilder RequestedBy(Guid requestedById)
        {
            _requestedById = requestedById;
            return this;
        }

        public PostPublicationBuilder WithFormat(PublicationFormat format)
        {
            _format = format;
            return this;
        }

        public PostPublicationBuilder WithMediaCount(int mediaCount)
        {
            _mediaCount = mediaCount;
            return this;
        }

        public PostPublicationBuilder At(DateTime utcNow)
        {
            _utcNow = utcNow;
            return this;
        }

        public PostPublicationBuilder PendingApproval() => WithStatus(PublicationStatus.PendingApproval);
        public PostPublicationBuilder Approved() => WithStatus(PublicationStatus.Approved);
        public PostPublicationBuilder Rejected() => WithStatus(PublicationStatus.Rejected);
        public PostPublicationBuilder Canceled() => WithStatus(PublicationStatus.Canceled);
        public PostPublicationBuilder Publishing() => WithStatus(PublicationStatus.Publishing);
        public PostPublicationBuilder Failed() => WithStatus(PublicationStatus.Failed);

        public PostPublicationBuilder Published(params string[] externalIds)
        {
            if (externalIds.Length > 0)
                _externalIds = externalIds;

            return WithStatus(PublicationStatus.Published);
        }

        public PostPublication Build()
        {
            var request = new PublicationRequest
            {
                PostId = _postId,
                TeamId = _teamId,
                Format = _format,
                MediaCount = _mediaCount,
                RequestedById = _requestedById,
                RequesterIsManager = _status is not (PublicationStatus.PendingApproval or PublicationStatus.Rejected)
            };

            var publication = ResultAssert.Success(PostPublication.Request(request, _utcNow));

            switch (_status)
            {
                case PublicationStatus.PendingApproval:
                case PublicationStatus.Approved:
                    break;

                case PublicationStatus.Rejected:
                    ResultAssert.Success(publication.Reject(Guid.NewGuid(), "Motivo de teste.", _utcNow));
                    break;

                case PublicationStatus.Canceled:
                    ResultAssert.Success(publication.Cancel());
                    break;

                case PublicationStatus.Publishing:
                    ForceStatus(publication, PublicationStatus.Publishing);
                    break;

                case PublicationStatus.Published:
                    ForceStatus(publication, PublicationStatus.Publishing);
                    ResultAssert.Success(publication.MarkAsPublished(_externalIds, _utcNow));
                    break;

                case PublicationStatus.Failed:
                    ForceStatus(publication, PublicationStatus.Publishing);
                    ResultAssert.Success(publication.MarkAsFailed("Falha de teste."));
                    break;

                default:
                    throw new NotSupportedException($"O builder não sabe montar uma publicação com status {_status}.");
            }

            return publication;
        }

        private PostPublicationBuilder WithStatus(PublicationStatus status)
        {
            _status = status;
            return this;
        }

        private static void ForceStatus(PostPublication publication, PublicationStatus status)
        {
            typeof(PostPublication)
                .GetProperty(nameof(PostPublication.Status))!
                .SetValue(publication, status);
        }
    }
}
