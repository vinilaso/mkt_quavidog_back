using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Entities.Media;

namespace Sienna.Application.UseCases.Media.RegisterPost
{
    public sealed class RegisterPostHandler(
        IUserContext userContext, 
        IPostRepository postRepository,
        IUnitOfWork uow) : IRequestHandler<RegisterPostCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterPostCommand request, CancellationToken cancellationToken)
        {
            if (!userContext.IsAuthenticated)
                return Error.Unauthorized("User.Unauthorized", "É necessário estar autenticado para cadastrar uma postagem.");

            var post = new Post(userContext.Id, request.Caption);

            await postRepository.AddAsync(post, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            return post.Id;
        }
    }
}
