using Radzen;
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
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<ISettingsManager, SettingsManager>();
        builder.Services.AddSingleton<ServiceClient>();
        builder.Services.AddSingleton<IFormatter, Formatter>();
        builder.Services.AddSingleton(_ => CreateNewDateFilter());
        builder.Services.AddRadzenComponents();

        return builder.Build();
    }

    #region Utility Methods

    private static DateFilter CreateNewDateFilter()
    {
        var filter = new DateFilter();

        var now = DateTimeOffset.Now;
        var firstDayOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
        var firstDayOfNextMonth = firstDayOfCurrentMonth.AddMonths(1);
        filter.From = firstDayOfCurrentMonth;
        filter.To = firstDayOfNextMonth;

        return filter;
    }

    #endregion
}
