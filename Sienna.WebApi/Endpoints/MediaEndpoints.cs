using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Sienna.Application.UseCases.Media.AddPostAsset;
using Sienna.Application.UseCases.Media.GetMedia;
using Sienna.Application.UseCases.Media.RegisterMedia;
using Sienna.Application.UseCases.Media.RegisterPost;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Media;
using Sienna.WebApi.Extensions;

namespace Sienna.WebApi.Endpoints
{
    public static class MediaEndpoints
    {
        private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

        public static IEndpointRouteBuilder MapMediaEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/media").WithTags("Media").RequireAuthorization();

            group
                .MapGet("public/{id:guid}", async ([FromRoute] Guid id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetMediaQuery(id));

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    if (!ContentTypeProvider.TryGetContentType(result.Value.FileName, out var contentType))
                        contentType = "application/octet-stream";

                    return TypedResults.File(result.Value.Content, contentType);
                })
                .AllowAnonymous()
                .ProducesWithDescription<FileContentHttpResult>(StatusCodes.Status200OK, "A imagem foi encontrada e retornada para exibição.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada mídia cadastrada com o ID informado.")
                .WithDescription("Retorna uma mídia sem exigir autenticação. Usada pela Meta para baixar as imagens das publicações no Instagram.");

            group
                .MapPost("files", async(IFormFile file, IMediator mediator) =>
                {
                    var name = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    await using var content = file.OpenReadStream();

                    var result = await mediator.Send(new RegisterMediaCommand(name, extension, content));

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Created($"media/{result.Value}", result.Value);
                })
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A mídia foi criada com sucesso no servidor.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Cadastra uma mídia no sistema.")
                .DisableAntiforgery();

            group
                .MapGet("files/{id:guid}", async ([FromRoute]Guid id, IMediator mediator) =>
                {
                    var query = new GetMediaQuery(id);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.File(
                        fileContents: result.Value.Content,
                        fileDownloadName: result.Value.FileName
                    );
                })
                .ProducesWithDescription<FileContentHttpResult>(StatusCodes.Status200OK, "A mídia foi encontrada e retornada para download.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada mídia cadastrada com o ID informado.")
                .WithDescription("Busca uma mídia do sistema.")
                .DisableAntiforgery();

            group
                .MapPost("posts", EndpointBodyFactory.Create<RegisterPostCommand, Guid>(guid => TypedResults.Created($"api/media/posts/{guid}", guid)))
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A postagem foi criada com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado")
                .WithDescription("Cadastra uma postagem no sistema.");

            group
                .MapPost("posts/{postId:guid}/assets", async ([FromRoute]Guid postId, AddPostAssetRequest request, IMediator mediator) =>
                {
                    var command = new AddPostAssetCommand(postId, request.MediaId, request.SequenceOrder);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok();
                })
                .ProducesWithDescription(StatusCodes.Status200OK, "A mídia foi associada à postagem.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma postagem com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "Já existe outra mídia na posição (SequenceOrder) informada.")
                .WithDescription("Associa uma mídia já cadastrada a uma postagem, na posição indicada por SequenceOrder.");

            return builder;
        }
    }
}
