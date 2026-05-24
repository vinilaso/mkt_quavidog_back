namespace Sienna.Domain.Abstractions.Media.DTOs
{
    public class AssetDTO
    {
        public MediaDTO Media { get; set; } = new MediaDTO();
        public int SequenceOrder { get; set; }
    }
}
