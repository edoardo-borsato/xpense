namespace Xpense
{
    internal interface IDialogService
    {
        public Task<bool> Confirm(string title, string message);
        public Task Alert(AlertType alertType, string title, string message);
    }

    internal class DialogService : IDialogService
    {
        private readonly Page _mainPage;

        public DialogService()
        {
            _mainPage = Application.Current!.MainPage!;
        }

        public Task<bool> Confirm(string title, string message)
        {
            return _mainPage.DisplayAlert(title, message, "Yes", "No");
        }

        public Task Alert(AlertType alertType, string title, string message)
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
}