namespace Sienna.WebApi.OpenApi
{
    /// <summary>
    /// Grupos (tags) da documentação da API. A ordem da lista define a ordem dos grupos na documentação.
    /// </summary>
    internal static class ApiTags
    {
        internal const string Identity = "Identity";
        internal const string Workflow = "Workflow";
        internal const string Media = "Media";
        internal const string Social = "Social";

        internal static readonly IReadOnlyList<(string Name, string Description)> All =
        [
            (Identity,
                "Cadastro de usuários, autenticação por token JWT e redefinição de senha. " +
                "Inclui as consultas sobre o usuário autenticado: perfil, times e postagens."),

            (Workflow,
                "Organização do trabalho em times: membros e papéis (dono, administrador e membro), " +
                "campanhas e associação de postagens às campanhas."),

            (Media,
                "Envio e consulta de mídias e cadastro de postagens com suas imagens. " +
                "A rota pública de mídias é usada pela Meta para baixar as imagens publicadas no Instagram."),

            (Social,
                "Integração com o Instagram por time: conexão da conta por meio de um token da Meta e solicitação " +
                "de publicações (feed, carrossel e stories), imediatas ou agendadas. Solicitações de membros " +
                "passam pela aprovação de um dono ou administrador do time.")
        ];
    }
}
