#if ANDROID
using MartaPol.Domain.Abstractions;
using Microsoft.Maui.Controls; // Application, Keyboard

namespace MartaPol.Infrastructure.Services;

public class DialogService : IDialogService
{
    public Task<bool> ConfirmAsync(string title, string message)
        => Application.Current?.MainPage?.DisplayAlert(title, message, "Tak", "Nie")
           ?? Task.FromResult(false);

    public Task<string?> PromptAsync(string title, string message)
        => Application.Current?.MainPage?.DisplayPromptAsync(title, message, "OK", "Anuluj", keyboard: Keyboard.Numeric)
           ?? Task.FromResult<string?>(null);

    public Task ToastAsync(string message)
    {
        if (Application.Current?.MainPage is { } page)
            return page.DisplayAlert("Info", message, "OK");
        return Task.CompletedTask;
    }
}
#else
using MartaPol.Domain.Abstractions;

namespace MartaPol.Infrastructure.Services;

public class DialogService : IDialogService
{
    public Task<bool> ConfirmAsync(string title, string message) => Task.FromResult(false);
    public Task<string?> PromptAsync(string title, string message) => Task.FromResult<string?>(null);
    public Task ToastAsync(string message) => Task.CompletedTask;
}
#endif
