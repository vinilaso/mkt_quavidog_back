using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Entities.Media
{
    public class Post : IDbEntity
    {
        private readonly List<PostAsset> _assets = [];
        private readonly List<CampaignPost> _campaigns = [];

        public Guid Id { get; set; }

        public Guid? AuthorId { get; set; }
        public User? Author { get; set; }

        public string Caption { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public PostStatus Status { get; set; }

        public IReadOnlyCollection<PostAsset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<CampaignPost> Campaigns => _campaigns.AsReadOnly();

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

        public void UpdateCaption(string caption)
        {
            Caption = caption ?? string.Empty;
        }

        public bool HasAsset(Guid mediaId) => _assets.Exists(asset => asset.MediaId == mediaId);

        public Result RemoveAsset(Guid mediaId)
        {
            var asset = _assets.Find(a => a.MediaId == mediaId);

            if (asset is null)
                return AssetNotFound(mediaId);

            _assets.Remove(asset);

            var sequenceOrder = 1;

            foreach (var remaining in _assets.OrderBy(a => a.SequenceOrder).ToList())
            {
                remaining.SequenceOrder = sequenceOrder++;
            }

            return Result.Success();
        }

        public Result ReorderAssets(IReadOnlyList<Guid> mediaIds)
        {
            ArgumentNullException.ThrowIfNull(mediaIds);

            if (mediaIds.Count != _assets.Count || mediaIds.Distinct().Count() != mediaIds.Count || !mediaIds.All(HasAsset))
                return Error.Validation("Asset.InvalidOrder", "Informe todas as imagens da postagem, cada uma uma única vez, na nova ordem.");

            for (var index = 0; index < mediaIds.Count; index++)
            {
                _assets.Find(a => a.MediaId == mediaIds[index])!.SequenceOrder = index + 1;
            }

            return Result.Success();
        }

        public static Error AssetNotFound(Guid mediaId) =>
            Error.NotFound("Asset.NotFound", $"A postagem não possui uma imagem com o ID {mediaId}.");
    }
}
