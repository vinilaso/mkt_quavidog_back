using Sienna.Domain.Entities.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Sienna.Domain.Abstractions.Identity.Authentication
{
    public record AuthenticationResult(
        AuthenticationStatus Status,
        User? User = null
    );
}
