namespace Sienna.Domain.Entities.Media
{
    public class PostAsset
    {
        public Guid? PostId { get; set; }
        public Post? Post { get; set; }

        public Guid? MediaId { get; set; }
        public Media? Media { get; set; }

        public int SequenceOrder { get; set; }

        public PostAsset(Guid postId, Guid mediaId, int sequenceOrder)
        {
            PostId = postId;
            MediaId = mediaId;
            SequenceOrder = sequenceOrder;
        }

        protected PostAsset()
        {
        }
    }
}
