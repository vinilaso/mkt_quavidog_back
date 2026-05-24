namespace Sienna.Domain.Abstractions.Media.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }

        public string Caption { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<AssetDTO> Assets { get; set; } = [];
    }
}
