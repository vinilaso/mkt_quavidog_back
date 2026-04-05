using MediatR;

namespace Sienna.Application.UseCases.Identity.Login.Events
{
    public sealed record UserLockedOutNotification(string Email, string FullName) : INotification;
}
