using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow
{
    internal static class InvitationErrors
    {
        internal static readonly Error NotFound = Error.NotFound("Invitation.NotFound", "Não foi encontrado um convite com o ID informado.");
        internal static readonly Error AlreadyMember = Error.Conflict("Invitation.AlreadyMember", "Este e-mail já pertence a um membro do time.");
    }
}
