using Sienna.Shared.RegularExpressions;
using System.Text;

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

        public static string ToBase64(this string str)
        {
            return str.ToBase64(Encoding.UTF8);
        }

        public static string ToBase64(this string str, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var bytes = encoding.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
    }
}
