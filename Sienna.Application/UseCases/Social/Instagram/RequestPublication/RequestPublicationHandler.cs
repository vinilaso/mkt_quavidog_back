using MediatR;
using Sienna.Application.Interfaces.Social.Signal;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Media;
using Sienna.Domain.Entities.Social;
using Sienna.Domain.Exceptions.Persistence;

namespace Sienna.Application.UseCases.Social.Instagram.RequestPublication
{
    internal sealed class RequestPublicationHandler(
        IUserContext userContext,
        ITeamAccessContext teamAccess,
        IInstagramAccountRepository accountRepository,
        IPostRepository postRepository,
        IPostPublicationRepository publicationRepository,
        IMediaRepository mediaRepository,
        IPublicationSignal publicationSignal,
        IUnitOfWork uow) : IRequestHandler<RequestPublicationCommand, Result<PublicationResponse>>
    {
        public async Task<Result<PublicationResponse>> Handle(RequestPublicationCommand request, CancellationToken cancellationToken)
        {
            if (!await accountRepository.ExistsByTeamIdAsync(request.TeamId, cancellationToken))
                return Error.Validation("Instagram.NotConfigured", "Conecte uma conta do Instagram ao time antes de solicitar publicações.");

            if (await postRepository.FindByIdAsync(request.PostId, cancellationToken) is not Post post || !await postRepository.BelongsToTeamAsync(post.Id, request.TeamId, cancellationToken))
                return Error.NotFound("Post.NotFound", "Não foi encontrada uma postagem do time com o ID informado.");

            if (post.AuthorId != userContext.Id && !teamAccess.IsManager)
                return Error.Forbidden("Publication.NotAuthor", "Apenas o autor da postagem ou um gestor do time pode solicitar a publicação.");

            if (await publicationRepository.HasBlockingPublicationAsync(post.Id, request.Format, cancellationToken))
                return DuplicatePublication();

            var mediaIds = post.Assets
                .OrderBy(asset => asset.SequenceOrder)
                .Select(asset => asset.MediaId!.Value)
                .ToList();

            var publicationResult = PostPublication.Request(new PublicationRequest
            {
                PostId = post.Id,
                TeamId = request.TeamId,
                Format = request.Format,
                MediaCount = mediaIds.Count,
                ScheduledFor = request.ScheduledFor,
                RequestedById = userContext.Id,
                RequesterIsManager = teamAccess.IsManager
            }, DateTime.UtcNow);

            if (publicationResult.IsFailure)
                return publicationResult.Error;

            var mediaValidation = await PublicationMediaRules.ValidateAsync(mediaRepository, mediaIds, cancellationToken);

            if (mediaValidation.IsFailure)
                return mediaValidation.Error;

            var publication = publicationResult.Value;

            await publicationRepository.AddAsync(publication, cancellationToken);

            try
            {
                await uow.CommitChangesAsync(cancellationToken);
            }
            catch (DuplicateEntryException)
            {
                return DuplicatePublication();
            }

            if (publication.Status is PublicationStatus.Approved)
                publicationSignal.Notify();

            return PublicationResponse.From(publication);
        }

        private static Error DuplicatePublication() =>
            Error.Conflict("Publication.AlreadyRequested", "Esta postagem já possui uma publicação pendente, agendada ou publicada neste formato.");
    }
}
