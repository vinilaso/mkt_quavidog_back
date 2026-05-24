using MediatR;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    internal sealed class GetUserPostsHandler(IUserRepository userRepository) : IRequestHandler<GetUserPostsQuery, Result<GetUserPostsResponse>>
    {
        public async Task<Result<GetUserPostsResponse>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await userRepository.GetUserPostsAsync(request.UserId, cancellationToken);
            return new GetUserPostsResponse(request.UserId, [..posts]);
        }
    }
}
