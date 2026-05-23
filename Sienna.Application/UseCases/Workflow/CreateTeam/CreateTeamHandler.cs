using MediatR;
using Sienna.Application.UseCases.Workflow.CreateTeam.TeamCreated;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.CreateTeam
{
    public sealed class CreateTeamHandler(
        ITeamRepository teamRepository,
        IUserContext userContext,
        IUnitOfWork uow,
        IPublisher publisher) : IRequestHandler<CreateTeamCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Team(request.TeamName, userContext.Id);

            await teamRepository.AddAsync(team);
            await uow.CommitChangesAsync(cancellationToken);

            await publisher.Publish(new TeamCreatedNotification(team.Name, userContext.Email), cancellationToken);

            return team.Id;
        }
    }
}
