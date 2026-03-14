using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
