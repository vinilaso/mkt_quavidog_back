using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.Interfaces.Social.Instagram
{
    public interface IInstagramClient
    {
        Task<Result<InstagramProfile>> GetProfileAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}
