using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task<Result> SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default);
    }
}
