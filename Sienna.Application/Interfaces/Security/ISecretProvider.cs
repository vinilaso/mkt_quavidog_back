namespace Sienna.Application.Interfaces.Security
{
    public interface ISecretProvider
    {
        string Protect(string plainText);
        string Unprotect(string protectedText);
    }
}
