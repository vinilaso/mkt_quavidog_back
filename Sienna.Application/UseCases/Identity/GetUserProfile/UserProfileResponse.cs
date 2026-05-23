namespace Sienna.Application.UseCases.Identity.GetUserProfile
{
    public record UserProfileResponse(Guid Id, string FullName, string Email);
}
