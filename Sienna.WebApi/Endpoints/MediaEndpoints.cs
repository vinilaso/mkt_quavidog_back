using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Sienna.Application.UseCases.Media.AddPostAsset;
using Sienna.Application.UseCases.Media.EditPost;
using Sienna.Application.UseCases.Media.GetMedia;
using Sienna.Application.UseCases.Media.RegisterMedia;
using Sienna.Application.UseCases.Media.RemovePostAsset;
using Sienna.Application.UseCases.Media.ReplacePostAsset;
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
                .WithDescription("Retorna uma mídia sem exigir autenticação. Usada pela Meta para baixar as imagens das publicações no Instagram.")
                .DisableRateLimiting();

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
                .MapPut("posts/{postId:guid}", EditPost)
                .WithName("EditPost")
                .WithSummary("Editar postagem")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "A postagem foi atualizada com sucesso.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário autenticado não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma postagem com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem tem uma publicação aprovada, em andamento ou já publicada.")
                .WithDescription("Altera a legenda de uma postagem. Apenas o autor pode editá-la, e somente enquanto nenhuma publicação dela estiver aprovada, sendo enviada ou publicada. Publicações aguardando aprovação usarão a nova legenda.");

            group
                .MapPost("posts/{postId:guid}/assets", AddPostAsset)
                .WithName("AddPostAsset")
                .WithSummary("Adicionar imagem à postagem")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A imagem foi cadastrada e associada à postagem. Retorna o ID da mídia criada.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O arquivo não foi enviado, está vazio, não é JPEG (.jpg ou .jpeg) ou tem mais de 8 MB.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário autenticado não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma postagem com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "Já existe outra mídia na posição (sequenceOrder) informada, ou a postagem tem uma publicação aprovada, em andamento ou já publicada.")
                .WithDescription("Envia uma imagem (multipart/form-data, campos file e sequenceOrder) e a associa à postagem na posição indicada. Apenas o autor da postagem pode adicionar imagens, e somente enquanto nenhuma publicação dela estiver aprovada, sendo enviada ou publicada. A imagem só é gravada se a associação for possível.")
                .DisableAntiforgery();

            group
                .MapPut("posts/{postId:guid}/assets/{mediaId:guid}", ReplacePostAsset)
                .WithName("ReplacePostAsset")
                .WithSummary("Substituir imagem da postagem")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "O arquivo da imagem foi substituído. O ID da mídia e a posição continuam os mesmos.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O arquivo não foi enviado, está vazio, não é JPEG (.jpg ou .jpeg) ou tem mais de 8 MB.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário autenticado não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada a postagem ou a imagem informada nela.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem tem uma publicação aprovada, em andamento ou já publicada.")
                .WithDescription("Troca o arquivo de uma imagem da postagem (multipart/form-data, campo file), mantendo o ID da mídia e a posição.")
                .DisableAntiforgery();

            group
                .MapDelete("posts/{postId:guid}/assets/{mediaId:guid}", RemovePostAsset)
                .WithName("RemovePostAsset")
                .WithSummary("Excluir imagem da postagem")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "A imagem foi removida e as restantes foram renumeradas a partir de 1.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário autenticado não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada a postagem ou a imagem informada nela.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem tem uma publicação aprovada, em andamento ou já publicada.")
                .WithDescription("Remove uma imagem da postagem e apaga a mídia do servidor. As imagens restantes mantêm a ordem relativa e são renumeradas (1, 2, 3...).");

            group
                .MapPut("posts/{postId:guid}/assets/order", ReorderPostAssets)
                .WithName("ReorderPostAssets")
                .WithSummary("Reordenar imagens da postagem")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "A nova ordem das imagens foi gravada.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "A lista não contém exatamente todas as imagens da postagem, cada uma uma única vez.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário autenticado não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma postagem com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem tem uma publicação aprovada, em andamento ou já publicada.")
                .WithDescription("Recebe os IDs de todas as mídias da postagem na ordem desejada. A primeira da lista fica na posição 1 (primeira do carrossel ou dos stories).");
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

        private static async Task<IResult> EditPost(
            [FromRoute, Description("ID da postagem.")] Guid postId,
            [FromBody] EditPostRequest request,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(postId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.NoContent
            );
        }

        private static async Task<IResult> ReplacePostAsset(
            [FromRoute, Description("ID da postagem.")] Guid postId,
            [FromRoute, Description("ID da mídia a substituir.")] Guid mediaId,
            [Description("Nova imagem JPEG (.jpg ou .jpeg) de até 8 MB.")] IFormFile file,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            await using var content = file.OpenReadStream();

            var command = new ReplacePostAssetCommand(
                PostId: postId,
                MediaId: mediaId,
                FileName: Path.GetFileNameWithoutExtension(file.FileName),
                Extension: Path.GetExtension(file.FileName),
                Content: content
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.NoContent
            );
        }

        private static async Task<IResult> RemovePostAsset(
            [FromRoute, Description("ID da postagem.")] Guid postId,
            [FromRoute, Description("ID da mídia a remover.")] Guid mediaId,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RemovePostAssetCommand(postId, mediaId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.NoContent
            );
        }

        private static async Task<IResult> ReorderPostAssets(
            [FromRoute, Description("ID da postagem.")] Guid postId,
            [FromBody] ReorderPostAssetsRequest request,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(postId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.NoContent
            );
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
