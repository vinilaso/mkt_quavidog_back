using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Sienna.Infrastructure.Email.MailerSend
{
    public sealed class MailerSendService(
        HttpClient httpClient, 
        IOptions<MailerSendSettings> options,
        ILogger<MailerSendService> logger) : IEmailService
    {
        private record UnprocessableEntityResponse(string Message, Dictionary<string, string[]>? Errors);

        public async Task<Result> SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                var payload = message.MapMailerSend(options.Value);
                using var response = await httpClient.PostAsJsonAsync("email", payload, cancellationToken);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                    return await HandleUnprocessableEntityAsync(response, cancellationToken);

                return Error.Failure("Email.ProviderError", $"Status code {response.StatusCode}.");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Falha ao contatar a API de e-mail.");

                if (e is HttpRequestException or TaskCanceledException)
                    return Error.Failure("Email.NetworkError", "Falha de rede ao tentar contatar a API de e-mail.");

                throw;
            }
        }

        private async Task<Result> HandleUnprocessableEntityAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<UnprocessableEntityResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                    cancellationToken
                );

                if (errorResponse?.Errors is not { Count: > 0 } errors)
                    return Error.Failure("Email.ProviderError", "A entidade não pode ser processada.");

                var firstError = errors.First();
                var propertyName = firstError.Key;
                var errorMessage = firstError.Value.FirstOrDefault() ?? "A propriedade não pode ser processada.";

                return Error.Failure(propertyName, errorMessage);
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Erro ao deserializar a resposta 422 da API de e-mail.");
                return Error.Failure("Json.DeserializationError", "Resposta do provedor não é um JSON válido.");
            }
        }
    }
}
