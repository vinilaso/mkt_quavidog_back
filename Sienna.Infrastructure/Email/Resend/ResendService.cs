using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace Sienna.Infrastructure.Email.Resend
{
    public sealed class ResendService(
        HttpClient httpClient,
        IOptions<ResendSettings> settings,
        ILogger<ResendService> logger) : IEmailService
    {
        private record ResendErrorResponse(string Name, string Message);

        public async Task<Result> SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                var payload = message.MapSend(settings.Value);
                using var response = await httpClient.PostAsJsonAsync("emails", payload, cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return await HandleUnsuccessfulResponse(response, cancellationToken);

                return Result.Success();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Falha ao contatar a API de e-mail.");

                if (e is HttpRequestException or TaskCanceledException)
                    return Error.Failure("Email.NetworkError", "Falha de rede ao tentar contatar a API de e-mail.");

                throw;
            }
        }

        private async Task<Result> HandleUnsuccessfulResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ResendErrorResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                    cancellationToken
                );

                if (errorResponse is null)
                    return Error.Failure("Email.ProviderError", $"A api de e-mails retornou o status {response.StatusCode}.");

                return Error.Failure(errorResponse.Name, errorResponse.Message);
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Erro ao deserializar a resposta da API de e-mail.");
                return Error.Failure("Json.DeserializationError", "Resposta do provedor não é um JSON válido.");
            }
        }
    }
}
