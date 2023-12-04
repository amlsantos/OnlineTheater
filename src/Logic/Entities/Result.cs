namespace Logic.Entities;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();
        
        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException();
        
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Fail(string message) => new(false, message);

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(default(T), false, message);
    }

    public static Result Ok()
    {
        return new Result(true, String.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, String.Empty);
    }

    public static Result Combine(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
                return result;
        }

        return Ok();
    }
}

public class Result<T> : Result
{
    private readonly T _value;

    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException();

            return _value;
        }
    }

    protected internal Result(T value, bool success, string error) : base(success, error)
    {
        _value = value;
    }
}