using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Social;
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
        private record IdResponse(string Id);
        private record StatusResponse([property: JsonPropertyName("status_code")] string StatusCode);

        private record ProfileResponse(
            [property: JsonPropertyName("user_id")] string UserId,
            [property: JsonPropertyName("username")] string Username,
            [property: JsonPropertyName("account_type")] string AccountType
        );

        private record GraphError(
            [property: JsonPropertyName("message")] string Message, 
            [property: JsonPropertyName("code")] int Code,
            [property: JsonPropertyName("error_user_msg")] string? UserMessage
        );

        private record GraphErrorResponse(GraphError? Error);

        private sealed class InstagramApiException(string message) : Exception(message);

        private const int MaxStatusChecks = 15;
        private static readonly TimeSpan StatusCheckInterval = TimeSpan.FromSeconds(2);
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

        public Task<Result<IReadOnlyList<string>>> PublishAsync(InstagramPublishRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            return ExecuteAsync(async () =>
            {
                if (request.MediaIds.Count == 0)
                    throw new ArgumentException("É necessário ao menos uma mídia para publicar.");

                return request.Format switch
                {
                    PublicationFormat.Story => await PublishStoriesAsync(request, cancellationToken),
                    PublicationFormat.Feed => await PublishFeedAsync(request, cancellationToken),
                    _ => throw new ArgumentOutOfRangeException("request.Format", "Formato de publicação não suportado.")
                };
            });
        }

        #region Stories

        private async Task<IReadOnlyList<string>> PublishStoriesAsync(InstagramPublishRequest request, CancellationToken cancellationToken)
        {
            List<string> containerIds = [];

            foreach (var mediaId in request.MediaIds)
                containerIds.Add(await CreateStoryContainerAsync(request.Credentials, mediaId, cancellationToken));

            foreach (var containerId in containerIds)
                await WaitUntilFinishedAsync(containerId, request.Credentials.AccessToken, cancellationToken);

            List<string> publishedIds = [];

            foreach (var containerId in containerIds)
            {
                try
                {
                    publishedIds.Add(await PublishContainerAsync(request.Credentials, containerId, cancellationToken));
                }
                catch (Exception e) when (publishedIds.Count > 0 && e is InstagramApiException or HttpRequestException or TaskCanceledException)
                {
                    var publishedStories = string.Join(", ", publishedIds);
                    throw new InstagramApiException($"{publishedIds.Count} de {containerIds.Count} stories foram publicados antes da falha (IDs: {publishedStories}). Motivo: {e.Message}");
                }
            }

            return publishedIds;
        }

        private Task<string> CreateStoryContainerAsync(InstagramCredentials credentials, Guid mediaId, CancellationToken cancellationToken)
        {
            var containerFields = new ContainerFields
            {
                MediaType = ContainerMediaType.Stories,
                ImageUrl = GetPublicMediaUrl(mediaId)
            };

            return CreateContainerAsync(credentials, containerFields, cancellationToken);
        }

        #endregion

        private async Task<IReadOnlyList<string>> PublishFeedAsync(InstagramPublishRequest request, CancellationToken cancellationToken)
        {
            var containerId = request.MediaIds.Count == 1
                ? await CreateImageContainerAsync(request, cancellationToken)
                : await CreateCarouselContainerAsync(request, cancellationToken);

            await WaitUntilFinishedAsync(containerId, request.Credentials.AccessToken, cancellationToken);
            return [await PublishContainerAsync(request.Credentials, containerId, cancellationToken)];
        }

        #region Containers

        private Task<string> CreateImageContainerAsync(InstagramPublishRequest request, CancellationToken cancellationToken)
        {
            var containerFields = new ContainerFields
            {
                ImageUrl = GetPublicMediaUrl(request.MediaIds[0]),
                Caption = request.Caption
            };

            return CreateContainerAsync(request.Credentials, containerFields, cancellationToken);
        }

        private async Task<string> CreateCarouselContainerAsync(InstagramPublishRequest request, CancellationToken cancellationToken)
        {
            List<string> children = [];
            var urls = request.MediaIds.Select(GetPublicMediaUrl).ToList();

            foreach (var imageUrl in urls)
            {
                var childrenFields = new ContainerFields
                {
                    ImageUrl = imageUrl,
                    IsCarouselItem = true,
                };

                children.Add(await CreateContainerAsync(request.Credentials, childrenFields, cancellationToken));
            }

            var mainContainerFields = new ContainerFields
            {
                MediaType = ContainerMediaType.Carousel,
                Children = children,
                Caption = request.Caption
            };

            return await CreateContainerAsync(request.Credentials, mainContainerFields, cancellationToken);
        }

        private async Task<string> CreateContainerAsync(InstagramCredentials credentials, ContainerFields fields, CancellationToken cancellationToken)
        {
            var container = await PostAsync<IdResponse>($"{credentials.UserId}/media", credentials.AccessToken, fields.ToDictionary(), cancellationToken);
            return container.Id;
        }

        private async Task WaitUntilFinishedAsync(string containerId, string accessToken, CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt < MaxStatusChecks; attempt++)
            {
                var status = await GetAsync<StatusResponse>($"{containerId}?fields=status_code", accessToken, cancellationToken);

                switch (status.StatusCode)
                {
                    case ContainerStatuses.Finished:
                        return;

                    case ContainerStatuses.Error:
                    case ContainerStatuses.Expired:
                        throw new InstagramApiException($"O Instagram não conseguiu processar a mídia (status {status.StatusCode}). Verifique se a imagem é um JPEG acessível publicamente.");
                }

                await Task.Delay(StatusCheckInterval, cancellationToken);
            }

            throw new InstagramApiException("Tempo esgotado aguardando o Instagram processar a mídia.");
        }

        private async Task<string> PublishContainerAsync(InstagramCredentials credentials, string containerId, CancellationToken cancellationToken)
        {
            var published = await PostAsync<IdResponse>(
                path: $"{credentials.UserId}/media_publish",
                accessToken: credentials.AccessToken,
                fields: new() { ["creation_id"] = containerId },
                cancellationToken
            );

            return published.Id;
        }

        private string GetPublicMediaUrl(Guid mediaId)
        {
            return $"{settings.Value.PublicMediaBaseUrl.TrimEnd('/')}/{mediaId}";
        }

        #endregion

        #region HTTP

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

        private async Task<T> PostAsync<T>(string path, string accessToken, Dictionary<string, string> fields, CancellationToken cancellationToken)
        {
            fields["access_token"] = accessToken;

            using var content = new FormUrlEncodedContent(fields);
            using var response = await httpClient.PostAsync(path, content, cancellationToken);

            return await ReadAsync<T>(response, cancellationToken);
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

                var error = errorResponse?.Error;
                var message = error?.UserMessage ?? error?.Message ?? $"A API do Instagram retornou o status {(int)response.StatusCode}";

                throw new InstagramApiException(message);
            }

            return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
                ?? throw new InstagramApiException("A API do Instagram retornou uma resposta vazia.");
        }

        private async Task<T> GetAsync<T>(string path, string accessToken, CancellationToken cancellationToken)
        {
            var separator = path.Contains('?') ? '&' : '?';
            var url = $"{path}{separator}access_token={Uri.EscapeDataString(accessToken)}";

            using var response = await httpClient.GetAsync(url, cancellationToken);
            return await ReadAsync<T>(response, cancellationToken);
        }

        #endregion
    }
}
