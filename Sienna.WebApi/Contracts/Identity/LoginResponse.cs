namespace Sienna.WebApi.Contracts.Identity
{
    /// <summary>Resultado da autenticação.</summary>
    /// <param name="Token">Token JWT. Envie-o nas demais requisições no cabeçalho "Authorization: Bearer {token}".</param>
    public record LoginResponse(string Token);
}
