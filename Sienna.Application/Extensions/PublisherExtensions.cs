using MediatR;
using Microsoft.Extensions.Logging;

namespace Sienna.Application.Extensions
{
    internal static class PublisherExtensions
    {
        internal static async Task PublishSafelyAsync(this IPublisher publisher, INotification notification, ILogger logger, CancellationToken cancellationToken)
        {
            try
            {
                await publisher.Publish(notification, cancellationToken);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Falha ao processar a notificação {Notification}", notification.GetType().Name);
            }
        }
    }
}
