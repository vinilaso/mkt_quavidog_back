namespace Sienna.Domain.Exceptions.Persistence
{
    public sealed class DuplicateEntryException(string? constraintName, Exception innerException) : Exception("O registro viola uma restrição de unicidade.", innerException)
    {
        public string? ConstraintName { get; } = constraintName;
    }
}
