namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private record GraphErrorResponse(GraphError? Error);
    }
}
