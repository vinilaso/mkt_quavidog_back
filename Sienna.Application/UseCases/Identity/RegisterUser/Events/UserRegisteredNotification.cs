using MediatR;

namespace Sienna.Application.UseCases.Identity.RegisterUser.Events
{
    public sealed record UserRegisteredNotification(string FullName, string Email) : INotification;
}
