using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram
{
    internal static class PublicationErrors
    {
        internal static readonly Error NotFound = Error.NotFound("Publication.NotFound", "Não foi encontrada uma publicação do time com o ID informado.");
    }
}
