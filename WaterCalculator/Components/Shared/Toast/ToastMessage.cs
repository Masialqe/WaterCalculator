namespace WaterCalculator.Components.Shared.Toast
{
    public sealed record ToastMessage(
        Guid Id,
        string Message,
        ToastType Type,
        int DurationMs = 4000);

    public enum ToastType
    {
        Success
    }
}
