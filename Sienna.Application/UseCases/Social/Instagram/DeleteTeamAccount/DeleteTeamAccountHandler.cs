using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.DeleteTeamAccount
{
    public sealed class DeleteTeamAccountHandler(IInstagramAccountRepository accountRepository, IUnitOfWork uow) : IRequestHandler<DeleteTeamAccountCommand, Result>
    {
        public async Task<Result> Handle(DeleteTeamAccountCommand request, CancellationToken cancellationToken)
        {
            if (await accountRepository.FindByTeamIdAsync(request.TeamId, cancellationToken) is not InstagramAccount account)
                return Error.NotFound("Instagram.NotConfigured", "O time não possui uma conta do Instagram vinculada.");

            accountRepository.Remove(account);
            await uow.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
