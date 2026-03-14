using Sienna.Shared.RegularExpressions;

namespace Sienna.Shared.Extensions.System
{
    public static class StringExtensions
    {
        public static string ToSnakeCaseUpper(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            string snakeCase = RegexHelper.CamelToSnakeCase().Replace(str, "$1_$2");
            return snakeCase.ToUpperInvariant();
        }
    }
}
