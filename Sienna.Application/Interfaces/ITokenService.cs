using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.Interfaces
{
    public interface ITokenService
    {
        AccessToken GenerateToken(User user);
    }
}
