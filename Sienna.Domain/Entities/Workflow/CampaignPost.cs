using Sienna.Domain.Entities.Media;

namespace Sienna.Domain.Entities.Workflow
{
    public class CampaignPost
    {
        public Guid? PostId { get; set; }
        public Post? Post { get; set; }

        public Guid? CampaignId { get; set; }
        public Campaign? Campaign { get; set; }

        public DateTime AssignmentDate { get; set; }

        public CampaignPost(Guid postId, Guid campaignId)
        {
            PostId = postId;
            CampaignId = campaignId;
            AssignmentDate = DateTime.UtcNow;
        }

        protected CampaignPost()
        {
        }
    }
}
