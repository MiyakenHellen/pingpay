namespace PingPay.Shared.Kernel.Primitives
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; private set; }
        public string? Error { get; private set; }

        private Result(T value) { Value = value; IsSuccess = true; }
        private Result(string error) { Error = error; IsSuccess = false; }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(string error) => new(error);
    }
}