namespace Sienna.Application.Interfaces.Social.Signal
{
    public interface IPublicationSignal
    {
        void Notify();
        Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken);
    }
}
