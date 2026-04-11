namespace Sienna.Application.Interfaces.Email
{
    public interface IEmailQueue
    {
        ValueTask EnqueueAsync(MailMessage message, CancellationToken cancellationToken = default);
        IAsyncEnumerable<MailMessage> DequeueAsync(CancellationToken cancellationToken = default);
    }
}
