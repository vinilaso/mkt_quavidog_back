namespace Sienna.WebApi.Contracts.Identity
{
    /// <summary>Resultado da autenticação.</summary>
    /// <param name="Token">Token JWT. Envie-o nas demais requisições no cabeçalho "Authorization: Bearer {token}".</param>
    /// <param name="ExpiresAt">Data e hora de expiração do token, em UTC (ISO 8601).</param>
    /// <param name="ExpiresInMilliseconds">Tempo de validade do token a partir da emissão, em milissegundos.</param>
    public record LoginResponse(string Token, DateTime ExpiresAt, long ExpiresInMilliseconds);
}
