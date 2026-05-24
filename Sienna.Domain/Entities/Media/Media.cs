using Sienna.Domain.Abstractions;

namespace Sienna.Domain.Entities.Media
{
    public class Media : IDbEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public byte[] Content { get; set; } = []; 
    }
}
