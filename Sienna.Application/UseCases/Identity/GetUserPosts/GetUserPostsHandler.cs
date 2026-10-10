using MediatR;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    public sealed class GetUserPostsHandler(IUserRepository userRepository) : IRequestHandler<GetUserPostsQuery, Result<PagedResult<PostDTO>>>
    {
        public async Task<Result<PagedResult<PostDTO>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetUserPostsAsync(request.UserId, request.Page, cancellationToken);
        }
    }
}
