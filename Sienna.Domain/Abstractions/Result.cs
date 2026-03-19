namespace Sienna.Domain.Abstractions
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error => IsSuccess ? throw new InvalidOperationException("The error of a success result cannot be accessed.") : field;

        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("A success result cannot have an error.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("A failure result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result<TSuccess> Success<TSuccess>(TSuccess value) => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);
        public static Result<TSuccess> Failure<TSuccess>(Error error) => new(default, false, error);


        public static implicit operator Result(Error error) => Failure(error);
    }
}
