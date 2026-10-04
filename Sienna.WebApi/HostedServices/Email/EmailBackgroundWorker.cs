using Sienna.Application.Interfaces.Email;

namespace Sienna.WebApi.HostedServices.Email
{
    public sealed class EmailBackgroundWorker(
        IEmailQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<EmailBackgroundWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Serviço de fila de e-mails iniciado.");

            await foreach (var email in queue.DequeueAsync(stoppingToken))
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var result = await emailSender.SendMessageAsync(email, stoppingToken);

                    if (result.IsFailure)
                    {
                        logger.LogError("Falha ao enviar o e-mail \"{Subject}\" para {To}: {Code} - {Message}",
                            email.Subject, string.Join(", ", email.To.Select(t => t.Email)), result.Error.Code, result.Error.Message);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Falha ao enviar e-mail para {To}", string.Join(", ", email.To));
                }
            }
        }
    }
}
