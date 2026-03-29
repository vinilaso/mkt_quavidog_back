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

            int statusCode = result.Error.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status400BadRequest
            };

            return Problem(
                statusCode: statusCode,
                title: "A domain error occurred.",
                type: result.Error.Code,
                detail: result.Error.Message
            );
        }
    }
}
