using Microsoft.JSInterop;

namespace WaterCalculator.Components.Shared.Toast
{
    public sealed class ToastService(IJSRuntime jsRuntime)
    {
        public async Task ShowSuccessAsync(string message, int durationMs = 4000)
        {
            await jsRuntime.InvokeVoidAsync("appToast.showSuccess", message, durationMs);
        }
        public async Task ShowFailureAsync(string message, int durationMs = 5000)
        {
            await jsRuntime.InvokeVoidAsync("appToast.showFailure", message, durationMs);
        }
    }
}
