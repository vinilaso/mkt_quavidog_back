using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Domain.Entities.Workflow
{
    public class Campaign : IDbEntity
    {
        private readonly List<CampaignPost> _posts = [];

        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public CampaignStatus Status { get; set; }
        public IReadOnlyList<CampaignPost> Posts => _posts.AsReadOnly();

        public Campaign(string name, Guid teamId)
        {
            Id = Guid.NewGuid();
            Name = name;
            TeamId = teamId;
            Status = CampaignStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        protected Campaign()
        {
            Name = string.Empty;
        }

        public Result TryAddPost(Guid postId)
        {
            if (_posts.Exists(p => p.PostId == postId))
                return Error.Conflict("CampaignPost.DuplicatePost", $"A postagem de ID '{postId}' já pertence à campanha '{Name}'.");

            _posts.Add(new CampaignPost(postId, Id));
            return Result.Success();
        }
    }
}
