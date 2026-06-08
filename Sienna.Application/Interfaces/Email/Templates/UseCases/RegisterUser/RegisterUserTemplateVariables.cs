using System.Text.Json.Serialization;

namespace Sienna.Application.Interfaces.Email.Templates.UseCases.RegisterUser
{
    internal record RegisterUserTemplateVariables(
        [property: JsonPropertyName("userName")] string UserName
    );
}
