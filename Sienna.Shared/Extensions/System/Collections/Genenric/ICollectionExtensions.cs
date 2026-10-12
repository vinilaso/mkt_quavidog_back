namespace Sienna.Shared.Extensions.System.Collections.Genenric
{
    public static class ICollectionExtensions
    {
        public static int? SumOf<T>(this ICollection<T> collection, Func<T, int?> selector)
        {
            if (collection is null)
                return null;

            var values = collection.Select(selector).Where(value => value.HasValue).ToList();
            return values.Count == 0 ? null : values.Sum(value => value!.Value);
        }
    }
}
