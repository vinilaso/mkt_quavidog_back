using MediatR;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.AssignUserToTeam.UserAssigned
{
    public record UserAssignedNotification(string TeamName, string AssignedBy, string UserAssignedEmail, TeamMemberRole Role) : INotification;
}
