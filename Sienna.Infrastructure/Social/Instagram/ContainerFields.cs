namespace Sienna.Infrastructure.Social.Instagram
{
    internal class ContainerFields
    {
        internal ContainerMediaType? MediaType { get; set; }
        internal IReadOnlyCollection<string>? Children { get; set; }
        internal string? Caption { get; set; }
        internal bool IsCarouselItem { get; set; }
        internal string? ImageUrl { get; set; }

        internal bool HasChildren => Children is IReadOnlyCollection<string> { Count: > 0 };
        internal bool HasCaption => !string.IsNullOrWhiteSpace(Caption);
        internal bool HasImageUrl => !string.IsNullOrWhiteSpace(ImageUrl);

        internal Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            if (IsCarouselItem)
                dictionary["is_carousel_item"] = "true";

            if (MediaType.HasValue)
                dictionary["media_type"] = MediaType.Value.ToMetaString();

            if (HasChildren)
                dictionary["children"] = string.Join(',', Children!);

            if (HasCaption)
                dictionary["caption"] = Caption!;

            if (HasImageUrl)
                dictionary["image_url"] = ImageUrl!;

            return dictionary;
        }
    }
}
