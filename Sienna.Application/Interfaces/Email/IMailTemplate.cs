namespace Sienna.Application.Interfaces.Email
{
    public interface IMailTemplate<in T> where T : class
    {
        string Id { get; }
    }
}
