namespace Sienna.Domain.Entities.Workflow
{
    public class Campaign
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public CampaignStatus Status { get; set; }

        public Campaign(string name, Guid teamId)
        {
            Name = name;
            TeamId = teamId;
        }

        protected Campaign()
        {
            Name = string.Empty;
        }
    }
}
