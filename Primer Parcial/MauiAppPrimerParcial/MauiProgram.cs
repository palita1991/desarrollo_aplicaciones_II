using MauiAppPrimerParcial.Views;
using MauiAppPrimerParcial.ViewModels;
using MauiAppPrimerParcial.Services;
using CommunityToolkit.Maui;

namespace MauiAppPrimerParcial
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Servicios
            builder.Services.AddSingleton<IApiService, ApiService>();

            // ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<DetailViewModel>();

            // Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetailPage>();

            return builder.Build();
        }
    }
}
