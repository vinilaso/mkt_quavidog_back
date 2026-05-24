using MediatR;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.GetMedia
{
    public sealed class GetMediaHandler(IMediaRepository repository) : IRequestHandler<GetMediaQuery, Result<MediaResponse>>
    {
        public async Task<Result<MediaResponse>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
        {
            var media = await repository.FindByIdAsync(request.MediaId, cancellationToken);

            if (media is null)
                return Error.NotFound("Media.NotFound", $"Não foi encontrada uma mídia no sistema com o ID {request.MediaId}");

            return new MediaResponse(
                Content: media.Content,
                FileName: Path.ChangeExtension(media.Name, media.Extension)
            );
        }
    }
}
