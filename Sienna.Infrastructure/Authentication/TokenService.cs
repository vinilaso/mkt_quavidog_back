using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sienna.Application.Interfaces;
using Sienna.Application.Mappings.Identity;
using Sienna.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sienna.Infrastructure.Authentication
{
    public sealed class TokenService(IConfiguration configuration) : ITokenService
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

        public AccessToken GenerateToken(User user)
        {
            JWTSection jwtSection = configuration.GetJWTSection();

            List<Claim> claims = [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FullName!)
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSection.Key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSection.Issuer,
                Audience = jwtSection.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken(
                Token: tokenHandler.WriteToken(token),
                ExpiresAt: token.ValidTo,
                ExpiresInMilliseconds: (long)TokenLifetime.TotalMilliseconds
            );
        }
    }
}
