using Sienna.Domain.Abstractions;

namespace Sienna.Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task<Result> SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default);
    }
}
