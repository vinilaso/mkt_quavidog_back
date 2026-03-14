using Microsoft.Extensions.Configuration;
using System.Text;

namespace Sienna.Application.Mappings.Identity
{
    public static class ConfigurationMapper
    {
        public static JWTSection GetJWTSection(this IConfiguration configuration)
        {
            const string JWT_SECTION = "Jwt";

            IConfigurationSection jwtSection = configuration.GetSection(JWT_SECTION)
                ?? throw new InvalidOperationException("Jwt section missing.");

            string issuer = jwtSection.GetSectionKey("Issuer");
            string audience = jwtSection.GetSectionKey("Audience");
            string key = jwtSection.GetSectionKey("Key");

            return new JWTSection(issuer, audience, Encoding.ASCII.GetBytes(key));
        }

        public static string GetSectionKey(this IConfigurationSection section, string key)
        {
            ArgumentNullException.ThrowIfNull(section, nameof(section));
            return section[key] ?? throw new InvalidOperationException($"{section.Key}:{key} missing.");
        }
    }
}
