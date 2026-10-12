using MediatR;
using Microsoft.Extensions.Options;
using Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.WebApi.HostedServices.Insights
{
    public sealed class InsightsBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<InsightsWorkerSettings> settings,
        ILogger<InsightsBackgroundWorker> logger) : BackgroundService
    {
        private readonly Dictionary<Guid, DateTime> _cooldownUntil = [];

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Serviço de coleta de métricas iniciado.");

            var interval = TimeSpan.FromMinutes(settings.Value.IntervalMinutes);
            using var timer = new PeriodicTimer(interval);

            do
            {
                try
                {
                    await RunCycleAsync(stoppingToken);
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    logger.LogError(e, "Falha no ciclo de coleta de métricas.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task RunCycleAsync(CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;
            RemoveExpiredCooldown(now);

            var dueIds = await GetDueAsync(now, stoppingToken);
            var collected = 0;

            foreach (var publicationId in dueIds)
            {
                if (_cooldownUntil.ContainsKey(publicationId))
                    continue;

                if (await CollectAsync(publicationId, stoppingToken))
                    collected++;
                else
                    _cooldownUntil[publicationId] = DateTime.UtcNow.AddMinutes(settings.Value.FailureCooldownMinutes);
            }

            if (collected > 0)
                logger.LogInformation("Métricas coletadas de {Count} publicação(ões).", collected);
        }

        private async Task<IReadOnlyList<Guid>> GetDueAsync(DateTime now, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPostPublicationRepository>();

            var limit = settings.Value.BatchSize + _cooldownUntil.Count;
            return await repository.GetDueForInsightsAsync(now, limit, stoppingToken);
        }

        private async Task<bool> CollectAsync(Guid publicationId, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(new CollectPublicationInsightsCommand(publicationId), stoppingToken);

                if (result.IsSuccess)
                    return true;

                if (result.Error.Code is "Instagram.UnreadableToken")
                    logger.LogError("Métricas da publicação {PublicationId}: {Message}", publicationId, result.Error.Message);
                else
                    logger.LogWarning("Métricas da publicação {PublicationId} não coletadas: {Message}", publicationId, result.Error.Message);

                return false;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Erro inesperado ao coletar as métricas da publicação {PublicationId}.", publicationId);
                return false;
            }
        }

        private void RemoveExpiredCooldown(DateTime now)
        {
            foreach (var expired in _cooldownUntil.Where(entry => entry.Value <= now).Select(entry => entry.Key).ToList())
            {
                _cooldownUntil.Remove(expired);
            }
        }
    }
}
