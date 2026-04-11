using Sienna.Application.Interfaces.Email;
using System.Threading.Channels;

namespace Sienna.Infrastructure.Email.Queue
{
    public sealed class InMemoryEmailQueue(int capacity) : IEmailQueue
    {
        private readonly Channel<MailMessage> _channel = Channel.CreateBounded<MailMessage>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        });

        public async ValueTask EnqueueAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public IAsyncEnumerable<MailMessage> DequeueAsync(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
