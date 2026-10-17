using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Values;

namespace Sienna.Tests.Common.Builders.Social
{
    public static class InstagramAccounts
    {
        /// <summary>Conta conectada com o token protegido de <see cref="TestValues.Instagram"/>.</summary>
        public static InstagramAccount Connected(Guid teamId) => new(
            teamId,
            TestValues.Instagram.UserId,
            TestValues.Instagram.Username,
            TestValues.Instagram.ProtectedToken);
    }
}
