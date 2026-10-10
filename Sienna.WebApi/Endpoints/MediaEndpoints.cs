using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Sienna.Application.UseCases.Media.AddPostAsset;
using Sienna.Application.UseCases.Media.GetMedia;
using Sienna.Application.UseCases.Media.RegisterMedia;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Media;
using Sienna.WebApi.Extensions;
using Sienna.WebApi.OpenApi;
using System.ComponentModel;

namespace Sienna.WebApi.Endpoints
{
    public class MediaEndpoints : IEndpointGroup
    {
        private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

        public void Map(IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/media").WithTags(ApiTags.Media).RequireAuthorization();

            group
                .MapGet("public/{id:guid}", GetPublicMedia)
                .AllowAnonymous()
                .WithName("GetPublicMedia")
                .WithSummary("Consultar mídia pública")
                .ProducesWithDescription<FileContentHttpResult>(StatusCodes.Status200OK, "A imagem foi encontrada e retornada para exibição.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada mídia cadastrada com o ID informado.")
                .WithDescription("Retorna uma mídia sem exigir autenticação. Usada pela Meta para baixar as imagens das publicações no Instagram.");

            group
                .MapPost("files", UploadMedia)
                .WithName("UploadMedia")
                .WithSummary("Enviar mídia")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A mídia foi criada com sucesso no servidor.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Cadastra uma mídia no sistema.")
                .DisableAntiforgery();

            group
                .MapGet("files/{id:guid}", DownloadMedia)
                .WithName("DownloadMedia")
                .WithSummary("Baixar mídia")
                .ProducesWithDescription<FileContentHttpResult>(StatusCodes.Status200OK, "A mídia foi encontrada e retornada para download.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada mídia cadastrada com o ID informado.")
                .WithDescription("Busca uma mídia do sistema.")
                .DisableAntiforgery();

            group
                .MapPost("posts", CreatePost)
                .WithName("CreatePost")
                .WithSummary("Cadastrar postagem")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A postagem foi criada com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado")
                .WithDescription("Cadastra uma postagem no sistema.");


            group
                .MapPost("posts/{postId:guid}/assets", AddPostAsset)
                .WithName("AddPostAsset")
                .WithSummary("Adicionar imagem à postagem")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A imagem foi cadastrada e associada à postagem. Retorna o ID da mídia criada.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O arquivo não foi enviado, está vazio, não é JPEG (.jpg ou .jpeg) ou tem mais de 8 MB.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma postagem do usuário autenticado com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "Já existe outra mídia na posição (sequenceOrder) informada.")
                .WithDescription("Envia uma imagem (multipart/form-data, campos file e sequenceOrder) e a associa à postagem na posição indicada. Apenas o autor da postagem pode adicionar imagens. A imagem só é gravada se a associação for possível.")
                .DisableAntiforgery();
        }

        private static async Task<IResult> GetPublicMedia([FromRoute, Description("ID da mídia.")] Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var query = new GetMediaQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(onSuccess: response =>
            {
                if (!ContentTypeProvider.TryGetContentType(response.FileName, out var contentType))
                    contentType = "application/octet-stream";

                return TypedResults.File(response.Content, contentType);
            });
        }

        private static async Task<IResult> UploadMedia(IFormFile file, IMediator mediator, CancellationToken cancellationToken)
        {
            var name = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            await using var content = file.OpenReadStream();

            var command = new RegisterMediaCommand(name, extension, content);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(id => TypedResults.CreatedAtRoute(
                value: id,
                routeName: "GetPublicMedia",
                routeValues: new { id }
            ));
        }

        private static async Task<IResult> DownloadMedia([FromRoute, Description("ID da mídia.")] Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var query = new GetMediaQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(response => TypedResults.File(
                fileContents: response.Content,
                fileDownloadName: response.FileName
            ));
        }

        private static async Task<IResult> CreatePost([FromBody]CreatePostRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);

            return result.ToHttpResult(id => TypedResults.CreatedAtRoute(
                value: id,
                routeName: "GetCurrentUserPosts"
            ));
        }

        private static async Task<IResult> AddPostAsset(
            [FromRoute, Description("ID da postagem.")] Guid postId, 
            [Description("Imagem JPEG (.jpg ou .jpeg) de até 8 MB.")] IFormFile file, 
            [FromForm, Description("Posição da imagem na postagem (1 = primeira). Define a ordem no carrossel ou nos stories.")] int sequenceOrder, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            await using var content = file.OpenReadStream();

            var command = new AddPostAssetCommand(
                PostId: postId,
                FileName: Path.GetFileNameWithoutExtension(file.FileName),
                Extension: Path.GetExtension(file.FileName),
                Content: content,
                SequenceOrder: sequenceOrder
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(id => TypedResults.CreatedAtRoute(
                value: id,
                routeName: "GetCurrentUserPosts"
            ));
        }
    }
}
