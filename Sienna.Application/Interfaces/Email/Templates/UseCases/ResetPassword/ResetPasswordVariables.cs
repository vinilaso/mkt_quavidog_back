using System.Text.Json.Serialization;

namespace Sienna.Application.Interfaces.Email.Templates.UseCases.ResetPassword
{
    internal record ResetPasswordVariables(
        [property: JsonPropertyName("resetToken")] string ResetToken
    );
}
