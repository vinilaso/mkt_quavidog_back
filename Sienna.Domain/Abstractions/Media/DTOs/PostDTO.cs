namespace Sienna.Domain.Abstractions.Media.DTOs
{
    /// <summary>Postagem com suas imagens.</summary>
    public class PostDTO
    {
        /// <summary>ID da postagem.</summary>
        public Guid Id { get; set; }

        /// <summary>Legenda.</summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary>Data de criação (UTC).</summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>Situação da postagem.</summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>Imagens da postagem, com suas posições.</summary>
        public IEnumerable<AssetDTO> Assets { get; set; } = [];
    }
}
