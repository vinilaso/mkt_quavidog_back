namespace Sienna.Domain.Abstractions.Media.DTOs
{
    /// <summary>Imagem de uma postagem.</summary>
    public class AssetDTO
    {
        /// <summary>Mídia associada.</summary>
        public MediaDTO Media { get; set; } = new MediaDTO();
        /// <summary>Posição da imagem na postagem (ordem no carrossel ou nos stories).</summary>
        public int SequenceOrder { get; set; }
    }
}
