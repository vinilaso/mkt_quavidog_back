namespace Sienna.Application.Mappings.Identity
{
    public record JWTSection(string Issuer, string Audience, byte[] Key);
}
