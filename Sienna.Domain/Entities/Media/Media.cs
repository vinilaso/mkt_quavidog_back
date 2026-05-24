using Sienna.Domain.Abstractions;

namespace Sienna.Domain.Entities.Media
{
    public class Media : IDbEntity
    {
        private readonly List<PostAsset> _assets = [];

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];
        public IReadOnlyList<PostAsset> Assets => _assets.AsReadOnly();
    }
}
