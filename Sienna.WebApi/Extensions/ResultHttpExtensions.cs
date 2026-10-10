using Sienna.Domain.Abstractions.Results;

namespace Sienna.WebApi.Extensions
{
    internal static class ResultHttpExtensions
    {
        internal static IResult ToHttpResult(this Result result, Func<IResult> onSuccess)
            => result.IsSuccess ? onSuccess() : result.Error.CreateProblemDetails();

        internal static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
            => result.IsSuccess ? onSuccess(result.Value) : result.Error.CreateProblemDetails(); 
    }
}
