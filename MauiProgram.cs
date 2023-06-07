using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SugahriStore.Datos;


namespace SugahriStore;

public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
        using (var dbContext = new BaseDeDatosContext())
        {
            dbContext.Database.EnsureDeletedAsync();
            dbContext.Database.EnsureCreatedAsync();
        }
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("QuicksandMedium500.ttf", "QuickSandMedium500");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
