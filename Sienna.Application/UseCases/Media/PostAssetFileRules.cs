using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media
{
    /// <summary>
    /// Regras para os arquivos de imagem das postagens: apenas JPEG, com no máximo 8 MB.
    /// </summary>
    internal static class PostAssetFileRules
    {
        internal const long MaxFileSizeBytes = 8 * 1024 * 1024;
        private static readonly string[] SupportedExtensions = [".jpg", ".jpeg"];

        /// <summary>Validação rápida, antes de ler o conteúdo.</summary>
        internal static Result Validate(string extension, Stream content)
        {
            if (!SupportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return Error.Validation("Asset.UnsupportedFormat", "Envie uma imagem JPEG (.jpg ou .jpeg).");

            if (content.CanSeek && content.Length > MaxFileSizeBytes)
                return FileTooLarge();

            return Result.Success();
        }

        internal static async Task<Result<byte[]>> ReadAsync(Stream content, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await content.CopyToAsync(memoryStream, cancellationToken);

            if (memoryStream.Length == 0)
                return Error.Validation("Asset.EmptyFile", "O arquivo enviado está vazio.");

            if (memoryStream.Length > MaxFileSizeBytes)
                return FileTooLarge();

            return memoryStream.ToArray();
        }

        private static Error FileTooLarge() =>
            Error.Validation("Asset.FileTooLarge", $"A imagem deve ter no máximo {MaxFileSizeBytes / (1024 * 1024)} MB.");
    }
}
