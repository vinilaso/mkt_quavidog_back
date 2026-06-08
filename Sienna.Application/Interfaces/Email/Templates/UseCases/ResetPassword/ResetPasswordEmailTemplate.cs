namespace Sienna.Application.Interfaces.Email.Templates.UseCases.ResetPassword
{
    internal class ResetPasswordEmailTemplate : IMailTemplate<ResetPasswordVariables>
    {
        public string Id => "password-reset-1";
    }
}
