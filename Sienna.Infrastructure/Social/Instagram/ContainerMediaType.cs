namespace Sienna.Infrastructure.Social.Instagram
{
    internal enum ContainerMediaType
    {
        Carousel,
        Stories
    }

    internal static class ContainerMediaTypeExtensions
    {
        internal static string ToMetaString(this ContainerMediaType mediaType)
        {
            return mediaType switch
            {
                ContainerMediaType.Carousel => "CAROUSEL",
                ContainerMediaType.Stories => "STORIES",
                _ => throw new ArgumentOutOfRangeException(nameof(mediaType))
            };
        }
    }
}
