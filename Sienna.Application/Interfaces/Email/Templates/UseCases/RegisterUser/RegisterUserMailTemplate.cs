namespace Sienna.Application.Interfaces.Email.Templates.UseCases.RegisterUser
{
    internal class RegisterUserMailTemplate : IMailTemplate<RegisterUserTemplateVariables>
    {
        public string Id => "boas-vindas";
    }
}
