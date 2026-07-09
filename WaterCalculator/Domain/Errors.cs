using WaterCalculator.Domain.Abstractions;

namespace WaterCalculator.Domain;

public static class Errors
{
    public static Error AlreadyExistsError => new Error(nameof(AlreadyExistsError), "Zasób już istnieje.");
    public static Error ApplicationError => new Error(nameof(ApplicationError), "Błąd aplikacji.");
    public static Error NotFoundError => new Error(nameof(NotFoundError), "Nie odnaleziono zasobu.");
    public static Error InvalidOperationError(string details) => new Error(nameof(InvalidOperationError), details);
}