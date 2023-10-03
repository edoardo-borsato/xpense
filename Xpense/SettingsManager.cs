using Xpense.Settings;

namespace Xpense
{
    internal class SettingsManager : ISettingsManager
    {
        private const string USERNAME_KEY = "username";
        private const string PASSWORD_KEY = "password";
        private const string EXPENSES_SERVICE_URL_KEY = "expensesServiceUrl";

        public ExpensesServiceSettings Get()
        {
            var username = Preferences.Get(USERNAME_KEY, "Username");
            var password = Preferences.Get(PASSWORD_KEY, "Password");
            var expensesServiceUrl = Preferences.Get(EXPENSES_SERVICE_URL_KEY, "Url");

            return new ExpensesServiceSettings
            {
                Credentials = new Credentials
                {
                    Username = username,
                    Password = password
                },
                Url = expensesServiceUrl
            };
        }

        public void Set(ExpensesServiceSettings settings)
        {
            Preferences.Set(USERNAME_KEY, settings.Credentials.Username);
            Preferences.Set(PASSWORD_KEY, settings.Credentials.Password);
            Preferences.Set(EXPENSES_SERVICE_URL_KEY, settings.Url);
        }
    }

    internal interface ISettingsManager
    {
        public ExpensesServiceSettings Get();
        public void Set(ExpensesServiceSettings settings);
    }
}
