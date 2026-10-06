namespace Sienna.Domain.Abstractions.Media.DTOs
{
    /// <summary>Mídia cadastrada.</summary>
    public class MediaDTO
    {
        /// <summary>ID da mídia.</summary>
        public Guid Id { get; set; }
        /// <summary>Nome do arquivo, com extensão.</summary>
        public string FileName { get; set; } = string.Empty;
    }
}
