namespace Sienna.Domain.Abstractions.Results
{
    public static class ResultExtensions
    {
        public static T Match<T>(this Result result, Func<T> onSuccess, Func<Error, T> onError)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));
            return result.IsSuccess ? onSuccess() : onError(result.Error);
        }

        public static T Match<T, U>(this Result<U> result, Func<U, T> onSuccess, Func<Error, T> onError)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));
            return result.IsSuccess ? onSuccess(result.Value) : onError(result.Error);
        }
    }
}
