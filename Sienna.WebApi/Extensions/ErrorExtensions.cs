using Sienna.Domain.Abstractions.Results;

namespace Sienna.WebApi.Extensions
{
    internal static class ErrorExtensions
    {
        internal static IResult CreateProblemDetails(this Error error)
        {
            return TypedResults.Problem(
                statusCode: GetStatusCode(error),
                title: "A domain error ocurred.",
                type: error.Code,
                detail: error.Message
            );
        }

        private static int GetStatusCode(Error error)
        {
            return error.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status400BadRequest
            };
        }
    }
}
