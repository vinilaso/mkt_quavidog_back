using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.Behaviors
{
    internal static class ResultFailureFactory<TResult>
    {
        private static readonly Func<Error, TResult> _create = Build();

        public static TResult Create(Error error) => _create(error);

        private static Func<Error, TResult> Build()
        {
            var type = typeof(TResult);

            if (type == typeof(Result))
                return error => (TResult)(object)Result.Failure(error);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = typeof(Result)
                    .GetMethod(nameof(Result.Failure), genericParameterCount: 1, [typeof(Error)])!
                    .MakeGenericMethod(type.GetGenericArguments()[0]);

                return (Func<Error, TResult>)Delegate.CreateDelegate(typeof(Func<Error, TResult>), failureMethod);
            }

            return _ => throw new InvalidOperationException($"{type.Name} não é Result nem Result<T>.");
        }
    }
}
