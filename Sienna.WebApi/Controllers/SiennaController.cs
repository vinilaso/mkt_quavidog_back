using Microsoft.AspNetCore.Mvc;
using Sienna.Domain.Abstractions;

namespace Sienna.WebApi.Controllers
{
    [ApiController]
    public abstract class SiennaController : ControllerBase
    {
        protected IActionResult HandleFailure(Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot handle failure for a successful result.");

            return result.Error.ErrorType switch
            {
                ErrorType.NotFound => NotFound(CreateProblemDetails(StatusCodes.Status404NotFound, result.Error)),
                ErrorType.Validation => BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, result.Error)),
                ErrorType.Conflict => Conflict(CreateProblemDetails(StatusCodes.Status409Conflict, result.Error)),
                ErrorType.Unauthorized => Unauthorized(CreateProblemDetails(StatusCodes.Status401Unauthorized, result.Error)),
                ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, CreateProblemDetails(StatusCodes.Status403Forbidden, result.Error)),
                _ => BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, result.Error))
            };
        }

        private static ProblemDetails CreateProblemDetails(int status, Error error)
        {
            return new()
            {
                Status = status,
                Type = error.Code,
                Title = "A domain error ocurred.",
                Detail = error.Message
            };
        }
    }
}
