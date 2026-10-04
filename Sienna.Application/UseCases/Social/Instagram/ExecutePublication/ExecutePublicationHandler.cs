using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Interfaces.Security;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using System.Security.Cryptography;

namespace Sienna.Application.UseCases.Social.Instagram.ExecutePublication
{
    internal sealed class ExecutePublicationHandler(
        IPostPublicationRepository publicationRepository,
        IInstagramAccountRepository accountRepository,
        IMediaRepository mediaRepository,
        IInstagramClient instagramClient,
        ISecretProvider secretProvider,
        IUnitOfWork uow,
        ILogger<ExecutePublicationHandler> logger) : IRequestHandler<ExecutePublicationCommand, Result<ExecutePublicationResponse>>
    {
        public async Task<Result<ExecutePublicationResponse>> Handle(ExecutePublicationCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindForPublishingAsync(request.PublicationId, cancellationToken) is not PostPublication publication)
                return Error.NotFound("Publication.NotFound", "Não foi encontrada uma publicação com o ID informado.");

            if (publication.Status is not PublicationStatus.Publishing)
                return Error.Conflict("Publication.NotClaimed", $"A publicação não está reservada para envio (status {publication.Status}).");

            var publishRequest = await BuildPublishRequestAsync(publication, cancellationToken);

            if (publishRequest.IsFailure)
                return await FailAsync(publication, publishRequest.Error.Message, cancellationToken);

            var publishResult = await instagramClient.PublishAsync(publishRequest.Value, cancellationToken);

            if (publishResult.IsFailure)
                return await FailAsync(publication, publishResult.Error.Message, cancellationToken);

            var marked = publication.MarkAsPublished(publishResult.Value, DateTime.UtcNow);

            if (marked.IsFailure)
                return await FailAsync(publication, marked.Error.Message, cancellationToken);

            await uow.CommitChangesAsync(cancellationToken);

            logger.LogInformation("Publicação {PublicationId} enviada ao Instagram: {ExternalIds}.", publication.Id, string.Join(", ", publication.ExternalIds));
            return ExecutePublicationResponse.FromPublication(publication);
        }

        private async Task<Result<InstagramPublishRequest>> BuildPublishRequestAsync(PostPublication publication, CancellationToken cancellationToken)
        {
            if (await accountRepository.FindByTeamIdAsync(publication.TeamId, cancellationToken) is not InstagramAccount account)
                return Error.Validation("Publication.AccountNotConfigured", "O time não possui uma conta do Instagram conectada.");

            var mediaIds = publication.Post!.Assets
                .OrderBy(asset => asset.SequenceOrder)
                .Select(asset => asset.MediaId!.Value)
                .ToList();

            if (mediaIds.Count == 0)
                return Error.Validation("Publication.NoMedia", "A postagem não possui imagens.");

            var mediaValidation = await PublicationMediaRules.ValidateAsync(mediaRepository, mediaIds, cancellationToken);

            if (mediaValidation.IsFailure)
                return mediaValidation.Error;

            var accessTokenResult = TryGetAccessToken(account, publication.TeamId);

            if (accessTokenResult.IsFailure)
                return accessTokenResult.Error;

            return new InstagramPublishRequest
            {
                Credentials = new InstagramCredentials(account.InstagramUserId, accessTokenResult.Value),
                Format = publication.Format,
                MediaIds = mediaIds,
                Caption = publication.Post.Caption
            };
        }

        private Result<string> TryGetAccessToken(InstagramAccount account, Guid teamId)
        {
            try
            {
                return secretProvider.Unprotect(account.ProtectedAccessToken);
            }
            catch (Exception e) when (e is CryptographicException or FormatException)
            {
                logger.LogError(e, "Não foi possível decifrar o token do Instagram do time {TeamId}.", teamId);
                return Error.Validation("Publication.UnreadableToken", "O token do Instagram do time não pôde ser lido. Reconecte a conta e reenvie a publicação.");
            }
        }

        private async Task<Result<ExecutePublicationResponse>> FailAsync(PostPublication publication, string reason, CancellationToken cancellationToken)
        {
            var marked = publication.MarkAsFailed(reason);

            if (marked.IsFailure)
                return marked.Error;

            await uow.CommitChangesAsync(cancellationToken);

            logger.LogWarning("Falha ao publicar {PublicationId}: {Reason}", publication.Id, reason);
            return ExecutePublicationResponse.FromPublication(publication);
        }
    }
}
