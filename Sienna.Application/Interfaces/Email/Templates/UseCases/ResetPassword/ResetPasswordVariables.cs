using System.Text.Json.Serialization;

namespace Sienna.Application.Interfaces.Email.Templates.UseCases.ResetPassword
{
    internal record ResetPasswordVariables(
        [property: JsonPropertyName("userEmail")] string UserEmail,
        [property: JsonPropertyName("resetToken")] string ResetToken
    );
}
