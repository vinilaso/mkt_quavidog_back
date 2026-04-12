using Sienna.Domain.Entities.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Sienna.Domain.Abstractions.Identity
{
    public record AuthenticationResult(
        AuthenticationStatus Status,
        User? User = null
    );
}
