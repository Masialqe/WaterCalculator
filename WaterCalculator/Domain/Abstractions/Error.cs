namespace WaterCalculator.Domain.Abstractions;

public record Error(
    string ErrorName,
    string ErrorDescription)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static implicit operator Result(Error error)
        => Result.Failure(error);

    public static implicit operator Error(Exception exception)
        => new(nameof(exception), exception.ToString());

}