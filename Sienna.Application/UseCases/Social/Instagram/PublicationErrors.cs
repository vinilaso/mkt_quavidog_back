using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram
{
    internal static class PublicationErrors
    {
        internal static readonly Error NotFound = Error.NotFound("Publication.NotFound", "Não foi encontrada uma publicação do time com o ID informado.");
        internal static readonly Error NotRequester = Error.Forbidden("Publication.NotRequester", "Apenas quem solicitou a publicação ou um gestor do time pode alterá-la.");
        internal static readonly Error ConcurrentUpdate = Error.Conflict("Publication.ConcurrentUpdate", "A publicação foi alterada por outra operação. Atualize os dados e tente novamente.");
    }
}
