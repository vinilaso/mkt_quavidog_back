using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.WebApi.Endpoints
{
    public class EndpointBodyFactory
    {
        public static Func<TRequest, IMediator, Task<IResult>> Create<TRequest, TResponse>(Func<TResponse, IResult> onSuccess) where TRequest : IRequest<Result<TResponse>>
        {
            return async (request, mediator) =>
            {
                var result = await mediator.Send(request);

                if (result.IsSuccess) 
                    return onSuccess(result.Value);

                return TypedResults.Problem(
                    statusCode: GetStatusCode(result),
                    title: "A domain error ocurred.",
                    type: result.Error.Code,
                    detail: result.Error.Message
                );
            };
        }

        private static int GetStatusCode(Result result)
        {
            return result.Error.ErrorType switch
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
