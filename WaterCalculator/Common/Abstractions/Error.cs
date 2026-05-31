namespace WaterCalculator.Common.Abstractions;

public record Error(
    string ErrorName,
    string? ErrorDescription = null)
{
    public static readonly Error None = new(string.Empty);

    public static implicit operator Result(Error error)
        => Result.Failure(error);

    public static implicit operator Error(Exception exception)
        => new(nameof(exception), exception.ToString());

}