using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Domain.Entities.Media
{
    public class Post : IDbEntity
    {
        private readonly List<PostAsset> _assets = [];

        public Guid Id { get; set; }

        public Guid? AuthorId { get; set; }
        public User? Author { get; set; }

        public string Caption { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public PostStatus Status { get; set; }

        public IReadOnlyCollection<PostAsset> Assets => _assets.AsReadOnly();

        public Post(Guid authorId, string caption)
        {
            Id = Guid.NewGuid();

            AuthorId = authorId;
            Caption = caption;

            CreatedAt = DateTime.UtcNow;
            Status = PostStatus.Created;
        }

        protected Post()
        {
        }

        public Result TryAddAsset(Guid mediaId, int sequenceOrder)
        {
            if (_assets.Exists(a => a.SequenceOrder == sequenceOrder && a.MediaId != mediaId))
            {
                return Error.Conflict("Duplicate.SequenceOrder", $"Já existe um arquivo na posição {sequenceOrder}.");
            }

            if (!_assets.Exists(a => a.MediaId == mediaId && a.SequenceOrder == sequenceOrder))
            {
                _assets.Add(new PostAsset(Id, mediaId, sequenceOrder));
            }

            return Result.Success();
        }      
    }
}
