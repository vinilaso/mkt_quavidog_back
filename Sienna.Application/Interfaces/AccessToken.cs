namespace Sienna.Application.Interfaces
{
    /// <summary>
    /// Token de acesso gerado no login.
    /// </summary>
    /// <param name="Token">Token JWT.</param>
    /// <param name="ExpiresAt">Data e hora de expiração do token (UTC).</param>
    /// <param name="ExpiresInMilliseconds">Tempo de validade do token, em milissegundos.</param>
    public record AccessToken(string Token, DateTime ExpiresAt, long ExpiresInMilliseconds);
}
