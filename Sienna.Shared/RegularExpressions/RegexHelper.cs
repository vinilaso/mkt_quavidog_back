using System.Text.RegularExpressions;

namespace Sienna.Shared.RegularExpressions
{
    public static partial class RegexHelper
    {
        [GeneratedRegex(@"([a-z0-9])([A-Z])", RegexOptions.Compiled)]
        public static partial Regex CamelToSnakeCase();
    }
}
