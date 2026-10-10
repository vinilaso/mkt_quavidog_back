using Sienna.Application.UseCases.Social.Instagram.ConfigureAccount;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    /// <summary>Dados para conectar uma conta do Instagram ao time.</summary>
    /// <param name="AccessToken">Token de acesso gerado no painel da Meta (Instagram → Configuração da API com login do Instagram → Gerar token). A conta precisa ser Business ou Creator.</param>
    public record ConfigureInstagramAccountRequest(string AccessToken)
    {
        public ConfigureInstagramAccountCommand ToCommand(Guid teamId) => new(teamId, AccessToken);
    }
}
