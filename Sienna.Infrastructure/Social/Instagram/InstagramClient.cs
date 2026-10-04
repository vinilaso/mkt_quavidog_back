using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions.Results;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed class InstagramClient(
        HttpClient httpClient, 
        IOptions<InstagramSettings> settings,
        ILogger<InstagramClient> logger) : IInstagramClient
    {
        private record IsResponse(string Id);
        private record StatusResponse([property: JsonPropertyName("status_code")] string StatusCode);

        private record ProfileResponse(
            [property: JsonPropertyName("user_id")] string UserId,
            [property: JsonPropertyName("username")] string Username,
            [property: JsonPropertyName("account_type")] string AccountType
        );

        private record GraphError(string Message, string Code);
        private record GraphErrorResponse(GraphError? Error);

        private sealed class InstagramApiException(string message) : Exception(message);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public Task<Result<InstagramProfile>> GetProfileAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(async () =>
            {
                var profile = await GetAsync<ProfileResponse>("me?fields=user_id,username,account_type", accessToken, cancellationToken);
                return new InstagramProfile(profile.UserId, profile.Username, profile.AccountType);
            });
        }

        private async Task<T> GetAsync<T>(string path, string accessToken, CancellationToken cancellationToken)
        {
            var separator = path.Contains('?') ? '&' : '?';
            var url = $"{path}{separator}access_token={Uri.EscapeDataString(accessToken)}";

            using var response = await httpClient.GetAsync(url, cancellationToken);
            return await ReadAsync<T>(response, cancellationToken);
        }

        private async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return Result.Success(await action());
            }
            catch (InstagramApiException e)
            {
                logger.LogWarning("A API do Instagram recusou a requisição: {Message}", e.Message);
                return Error.Failure("Instagram.ApiError", e.Message);
            }
            catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
            {
                logger.LogError(e, "Falha ao contatar a API do Instagram.");
                return Error.External("Instagram.NetworkError", "Falha de rede ao tentar contatar a API do Instagram.");
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Erro ao deserializar a resposta da API do Instagram.");
                return Error.External("Json.DeserializationError", "A resposta da API do Instagram não é um JSON válido.");
            }
        }

        private static async Task<T> ReadAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                GraphErrorResponse? errorResponse = null;

                try
                {
                    errorResponse = await response.Content.ReadFromJsonAsync<GraphErrorResponse>(JsonOptions, cancellationToken);
                }
                catch (JsonException) { }

                throw new InstagramApiException(errorResponse?.Error?.Message ?? $"A API do Instagram retornou o status {(int)response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
                ?? throw new InstagramApiException("A API do Instagram retornou uma resposta vazia.");
        }
    }
}
