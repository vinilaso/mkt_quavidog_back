namespace Sienna.Domain.Abstractions.Security
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        Guid Id { get; }
        string Email { get; }
        string Name { get; }
    }
}
