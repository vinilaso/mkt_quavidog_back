using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RegisterMedia
{
    public sealed class RegisterMediaHandler(IUnitOfWork uow, IMediaRepository repository) : IRequestHandler<RegisterMediaCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterMediaCommand request, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            request.Content.CopyTo(memoryStream);

            var media = new Domain.Entities.Media.Media
            {
                Content = memoryStream.ToArray(),
                Extension = request.Extension,
                Name = request.Name
            };

            await repository.AddAsync(media, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            return media.Id;
        }
    }
}
