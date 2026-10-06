namespace Sienna.Application.UseCases.Identity.GetUserProfile
{
    /// <summary>Perfil do usuário.</summary>
    /// <param name="Id">ID do usuário.</param>
    /// <param name="FullName">Nome completo.</param>
    /// <param name="Email">E-mail.</param>
    public record UserProfileResponse(Guid Id, string FullName, string Email);
}
