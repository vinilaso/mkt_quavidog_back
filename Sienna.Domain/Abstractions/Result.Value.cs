namespace Sienna.Domain.Abstractions
{
    public class Result<TSuccess> : Result
    {
        private readonly TSuccess? _value;
        public TSuccess Value => IsFailure ? throw new InvalidOperationException("The value of a failure result cannot be accessed.") : _value!;

        protected internal Result(TSuccess? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public static implicit operator Result<TSuccess>(TSuccess value) => Success(value);
        public static implicit operator Result<TSuccess>(Error error) => Failure<TSuccess>(error);
    }
}
