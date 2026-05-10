using MediatR;

namespace Sienna.Application.UseCases.Workflow.CreateTeam.TeamCreated
{
    internal record TeamCreatedNotification(string TeamName, string OwnerMail) : INotification;
}
