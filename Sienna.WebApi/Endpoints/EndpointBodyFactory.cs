using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.WebApi.Extensions;

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

                return result.Error.CreateProblemDetails();
            };
        }

        public static Func<TRequest, IMediator, Task<IResult>> Create<TRequest>(Func<IResult> onSuccess) where TRequest : IRequest<Result>
        {
            return async (request, mediator) =>
            {
                var result = await mediator.Send(request);

                if (result.IsSuccess)
                    return onSuccess();

                return result.Error.CreateProblemDetails();
            };
        }
    }
}
