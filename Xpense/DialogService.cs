namespace Xpense;

internal interface IDialogService
{
    public Task<bool> ConfirmAsync(string title, string message);
    public Task AlertAsync(AlertType alertType, string title, string message);
}

internal class DialogService : IDialogService
{
    private readonly Page _mainPage = Application.Current!.MainPage!;

    public Task<bool> ConfirmAsync(string title, string message)
    {
        return _mainPage.DisplayAlert(title, message, "Yes", "No");
    }

    public Task AlertAsync(AlertType alertType, string title, string message)
    {
        return _mainPage.DisplayAlert($"{alertType}: {title}", message, "Ok");
    }
}

public enum AlertType
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Success = 3
}