using WaterCalculator.Common.Abstractions;

namespace WaterCalculator.Domain;

public static class Errors
{
    public static Error AlreadyExistsError => new Error(nameof(AlreadyExistsError), "Resource already exists.");
    public static Error ApplicationError => new Error(nameof(ApplicationError), "Application error.");
}