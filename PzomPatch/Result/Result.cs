using System;
using System.Threading.Tasks;

namespace PzomPatch;

public class Result : IResult
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) ||
            (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public Error Error { get; }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static IResult Success()
    {
        return new Result(true, Error.None);
    }

    public static Task<IResult> SuccessFromResult()
    {
        return Task.FromResult(Success());
    }

    public static IResult Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Task<IResult> FailureFromResult(Error error)
    {
        return Task.FromResult(Failure(error));
    }
}