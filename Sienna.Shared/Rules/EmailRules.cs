namespace Sienna.Shared.Rules
{
    public static class EmailRules
    {
        public static string Normalize(string email) =>
            email?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}
