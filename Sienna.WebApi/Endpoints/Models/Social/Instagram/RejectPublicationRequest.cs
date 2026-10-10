using Sienna.Application.UseCases.Social.Instagram.RejectPublication;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    /// <summary>Dados para reprovar uma publicação.</summary>
    /// <param name="Reason">Motivo da reprovação, que será informado ao autor. Máximo de 500 caracteres.</param>
    public record RejectPublicationRequest(string Reason)
    {
        public RejectPublicationCommand ToCommand(Guid teamId, Guid publicationId) => new(teamId, publicationId, Reason);
    }
}
