namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private sealed class InstagramApiException(string message) : Exception(message);
    }
}
