namespace Sienna.Domain.Exceptions.Persistence
{
    public sealed class ConcurrencyConflictException(Exception innerException) 
        : Exception("O registro foi alterado por outra operação.", innerException);
}
