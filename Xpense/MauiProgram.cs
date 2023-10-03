using Xpense.Utility;

namespace Xpense;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        builder.Services.AddScoped<IDialogService, DialogService>();
        builder.Services.AddScoped<ISettingsManager, SettingsManager>();
        builder.Services.AddSingleton<ExpensesManager>();
        builder.Services.AddSingleton<IFormatter, Formatter>();
        builder.Services.AddSingleton(_ => CreateNewDateFilter());

        return builder.Build();
    }

    #region Utility Methods

    private static DateFilter CreateNewDateFilter()
    {
        var filter = new DateFilter();

        var now = DateTimeOffset.Now;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        filter.From = firstDayOfMonth;
        filter.To = lastDayOfMonth;

        return filter;
    }

    #endregion
}
