using MediatR;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Social.Signal;
using Sienna.Application.UseCases.Social.Instagram.ExecutePublication;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.WebApi.HostedServices.Publications
{
    public sealed class PublicationBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        IPublicationSignal publicationSignal,
        IOptions<PublicationWorkerSettings> settings,
        ILogger<PublicationBackgroundWorker> logger) : BackgroundService
    {
        private const string InterruptedReason = "O envio foi interrompido por uma reinicialização do servidor. Confira o Instagram antes de reenviar, pois a publicação pode ter sido concluída.";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Serviço de publicações iniciado.");

            await RecoverInterruptedAsync(stoppingToken);

            var interval = TimeSpan.FromSeconds(settings.Value.IntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunCycleAsync(stoppingToken);
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    logger.LogError(e, "Falha no ciclo do serviço de publicações.");
                }

                await publicationSignal.WaitAsync(interval, stoppingToken);
            }
        }

        private async Task RecoverInterruptedAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IPostPublicationRepository>();

                var recovered = await repository.FailInterruptedAsync(InterruptedReason, stoppingToken);

                if (recovered > 0)
                    logger.LogWarning("{Count} publicação(ões) interrompida(s) foram marcadas como falha.", recovered);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Falha ao recuperar publicações interrompidas.");
            }
        }

        private async Task RunCycleAsync(CancellationToken stoppingToken)
        {
            var batchSize = settings.Value.BatchSize;

            while (!stoppingToken.IsCancellationRequested)
            {
                var claimedIds = await ClaimDueAsync(batchSize, stoppingToken);

                foreach (var publicationId in claimedIds)
                    await ExecuteAsync(publicationId, stoppingToken);

                if (claimedIds.Count < batchSize)
                    return;
            }
        }

        private async Task<IReadOnlyList<Guid>> ClaimDueAsync(int batchSize, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPostPublicationRepository>();

            return await repository.ClaimDueAsync(DateTime.UtcNow, batchSize, stoppingToken);
        }

        private async Task ExecuteAsync(Guid publicationId, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(new ExecutePublicationCommand(publicationId), CancellationToken.None);

                if (result.IsFailure)
                {
                    logger.LogError("Publicação {PublicationId} não pôde ser processada: {Code} - {Message}", publicationId, result.Error.Code, result.Error.Message);
                }
                else if (result.Value.Status is PublicationStatus.Failed)
                {
                    logger.LogWarning("Publicação {PublicationId} falhou: {Reason}", publicationId, result.Value.FailureReason);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Erro inesperado ao enviar a publicação {PublicationId}.", publicationId);
            }
        }
    }
}
