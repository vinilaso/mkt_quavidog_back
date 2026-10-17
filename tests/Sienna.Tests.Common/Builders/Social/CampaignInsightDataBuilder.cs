using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Time;

namespace Sienna.Tests.Common.Builders.Social
{
    /// <summary>Monta os dados brutos que o repositório entrega para o cálculo das métricas de campanha.</summary>
    public sealed class CampaignInsightDataBuilder
    {
        private readonly Guid _campaignId = Guid.NewGuid();
        private readonly List<CampaignPublicationInsightDataDTO> _publications = [];
        private string _name = "Campanha de teste";

        public CampaignInsightDataBuilder Named(string name)
        {
            _name = name;
            return this;
        }

        public CampaignInsightDataBuilder WithPublication(params InsightPointDTO[] snapshots) =>
            WithPublication(PublicationFormat.Feed, snapshots);

        public CampaignInsightDataBuilder WithPublication(PublicationFormat format, params InsightPointDTO[] snapshots)
        {
            _publications.Add(new CampaignPublicationInsightDataDTO
            {
                PublicationId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Caption = $"Legenda {_publications.Count + 1}",
                Format = format,
                PublishedAt = snapshots.Length > 0
                    ? snapshots.Min(snapshot => snapshot.CollectedAt).AddHours(-1)
                    : TestTime.Now,
                Snapshots = snapshots
            });

            return this;
        }

        public CampaignInsightsDataDTO Build() => new()
        {
            CampaignId = _campaignId,
            CampaignName = _name,
            Publications = [.. _publications]
        };
    }
}
