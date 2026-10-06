namespace Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns
{
    /// <summary>Postagem de uma campanha.</summary>
    public class CampaignPostDTO
    {
        /// <summary>ID da postagem.</summary>
        public Guid PostId { get; set; }
        /// <summary>Legenda.</summary>
        public string? Caption { get; set; }
        /// <summary>Situação da postagem.</summary>
        public string? Status { get; set; }
    }
}
