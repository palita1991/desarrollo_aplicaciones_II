using MauiAppPrimerParcial.Views;
using MauiAppPrimerParcial.ViewModels;
using MauiAppPrimerParcial.Interfaces;
using MauiAppPrimerParcial.Repositories;
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

            // Infraestructura tipo singleton, es decir, se crea una sola instancia de la clase y se comparte en toda la aplicación
            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddSingleton<IUserRepository, UserRepositorySQLite>();

            // Aplicación tipo transient, es decir, se crea una nueva instancia de la clase cada vez que se solicita
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>(); 

            builder.Services.AddTransient<DetailViewModel>();
            builder.Services.AddTransient<DetailPage>();

            return builder.Build();
        }
    }
}
