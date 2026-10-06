using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Sienna.WebApi.OpenApi
{
    /// <summary>
    /// Define o título, a versão e a descrição geral do documento OpenAPI.
    /// </summary>
    internal sealed class ApiInfoDocumentTransformer : IOpenApiDocumentTransformer
    {
        private const string Description = """
            API do **Sienna**, plataforma de gestão de marketing: times, campanhas, postagens e publicação no Instagram.

            ## Como começar
            1. Crie um usuário em **Identity → Cadastrar usuário** e entre em **Identity → Autenticar usuário (login)**.
            2. Use o token retornado no botão de autenticação (*Bearer*). As rotas com cadeado exigem o token.

            ## Fluxo de publicação no Instagram
            1. **Workflow → Criar time.** Quem cria o time é o dono.
            2. **Social → Conectar conta do Instagram**, com um token gerado no painel da Meta.
            3. **Workflow → Criar campanha**, **Media → Cadastrar postagem** e **Media → Adicionar imagem à postagem**.
            4. **Workflow → Adicionar postagem à campanha.**
            5. **Social → Solicitar publicação no Instagram**, para agora ou agendada. Solicitações de membros aguardam a aprovação de um dono ou administrador.

            ## Erros
            As falhas seguem o formato *Problem Details* (RFC 9457): o campo `type` traz o código do erro
            (ex.: `Publication.AlreadyRequested`), e o campo `detail`, a mensagem para o usuário.

            ## Datas
            As datas retornadas estão em UTC (ISO 8601). Ao agendar, envie o horário com fuso
            (ex.: `2026-10-10T18:00:00-03:00`). Valores de enumerações trafegam como texto em camelCase
            (ex.: `"pendingApproval"`).
            """;

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Info = new OpenApiInfo
            {
                Title = "Sienna API",
                Version = "v1",
                Description = Description
            };

            return Task.CompletedTask;
        }
    }
}
