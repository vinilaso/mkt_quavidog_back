namespace Sienna.Domain.Exceptions.Email
{
    public class InvalidEmailException(string invalidProperty, string message) : Exception(message)
    {
        public string InvalidProperty { get; } = invalidProperty;
    }
}
