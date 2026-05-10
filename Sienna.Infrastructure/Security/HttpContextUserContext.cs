using Microsoft.AspNetCore.Http;
using Sienna.Domain.Abstractions.Security;
using System.Security.Claims;

namespace Sienna.Infrastructure.Security
{
    internal class HttpContextUserContext(IHttpContextAccessor accessor) : IUserContext
    {
        public bool IsAuthenticated => accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public Guid Id
        {
            get
            {
                var claim = GetClaim(ClaimTypes.NameIdentifier);

                return Guid.TryParse(claim.Value, out var id)
                    ? id : throw new UnauthorizedAccessException("O token não contém um ID de usuário válido.");
            }
        }

        public string Email => GetClaim(ClaimTypes.Email).Value;

        public string Name => GetClaim(ClaimTypes.Name).Value;

        private Claim GetClaim(string claimType)
        {
            return accessor.HttpContext?.User?.FindFirst(claimType)
                ?? throw new UnauthorizedAccessException("O token não contém o claim especificado.");
        }
    }
}
