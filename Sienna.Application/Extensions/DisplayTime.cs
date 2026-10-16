namespace Sienna.Application.Extensions
{
    internal static class DisplayTime
    {
        private static readonly TimeZoneInfo BrazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        internal static string Format(DateTime utc) =>
            TimeZoneInfo.ConvertTimeFromUtc(utc, BrazilTimeZone).ToString("dd/MM/yyyy 'às' HH:mm");

        internal static DateOnly ToBrazilDate(DateTime utc) =>
            DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(utc, BrazilTimeZone));
    }
}
