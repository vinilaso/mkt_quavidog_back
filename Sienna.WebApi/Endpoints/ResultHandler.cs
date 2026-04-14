using Sienna.Domain.Abstractions.Results;

namespace Sienna.WebApi.Endpoints
{
    public static class ResultHandler
    {
        public static IResult CreateProblemDetails(Error error)
        {
            return TypedResults.Problem(
                statusCode: error.GetStatusCode(),
                title: "A domain error ocurred.",
                type: error.Code,
                detail: error.Message
            );
        }

        public static int GetStatusCode(this Error error)
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
