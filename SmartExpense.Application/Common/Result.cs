namespace SmartExpense.Application.Common;

/// <summary>
/// Outcome of a use case that can fail with one or more validation errors.
/// Avoids using exceptions for expected validation failures.
/// </summary>
public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private Result(bool isSuccess, T? value, IReadOnlyCollection<string> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(true, value, Array.Empty<string>());

    public static Result<T> Failure(IReadOnlyCollection<string> errors) => new(false, default, errors);
}
