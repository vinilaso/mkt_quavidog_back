namespace Sienna.Application.Messaging.Teams
{
    public interface ITeamScopedRequest
    {
        Guid TeamId { get; }
    }
}
