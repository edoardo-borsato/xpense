namespace Xpense;

internal class SettingsManager : ISettingsManager
{
    private const string USERNAME_KEY = "username";
    private const string THEME_KEY = "theme";
    private const string EXPENSES_SERVICE_URL_OLD = "https://expenses-service-api.azurewebsites.net";
    private const string EXPENSES_SERVICE_URL = "https://expenses-api-service.azurewebsites.net";

    public string GetUsername()
    {
        return Preferences.Get(USERNAME_KEY, null);
    }

    public void SetUsername(string username)
    {
        Preferences.Set(USERNAME_KEY, username);
    }

    public Uri GetExpensesServiceUri()
    {
        return new Uri(EXPENSES_SERVICE_URL);
    }

    public string GetTheme()
    {
		return Preferences.Get(THEME_KEY, ThemeService.DefaultTheme);
	}

    public void SetTheme(string theme)
    {
		Preferences.Set(THEME_KEY, theme);
	}
}

internal interface ISettingsManager
{
    public string GetUsername();
    public void SetUsername(string username);
    public Uri GetExpensesServiceUri();
    public string GetTheme();
    public void SetTheme(string theme);
}