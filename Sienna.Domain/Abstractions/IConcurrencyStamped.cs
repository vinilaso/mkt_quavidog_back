namespace Sienna.Domain.Abstractions
{
    public interface IConcurrencyStamped
    {
        Guid ConcurrencyStamp { get; }
        void RenewConcurrencyStamp();
    }
}
