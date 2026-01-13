namespace BillTrackPro.Application.Common;

/// <summary>
/// A generic result wrapper for service operations following the Result pattern.
/// Provides a clean way to return success/failure states without exceptions.
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; private init; }
    public string? Error { get; private init; }
    public List<string> Errors { get; private init; } = new();

    private Result() { }

    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error,
        Errors = new List<string> { error }
    };

    public static Result<T> Failure(IEnumerable<string> errors) => new()
    {
        IsSuccess = false,
        Error = errors.FirstOrDefault(),
        Errors = errors.ToList()
    };
}

/// <summary>
/// Result type for operations that don't return a value.
/// </summary>
public class Result
{
    public bool IsSuccess { get; private init; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; private init; }
    public List<string> Errors { get; private init; } = new();

    private Result() { }

    public static Result Success() => new() { IsSuccess = true };

    public static Result Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error,
        Errors = new List<string> { error }
    };

    public static Result Failure(IEnumerable<string> errors) => new()
    {
        IsSuccess = false,
        Error = errors.FirstOrDefault(),
        Errors = errors.ToList()
    };
}
