using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppPrimerParcial.Models;
using MauiAppPrimerParcial.Services;

namespace MauiAppPrimerParcial.ViewModels;

public partial class DetailViewModel : ObservableObject, IQueryAttributable
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private User currentUser = new User();

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public DetailViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    private async Task LoadUserDetailsAsync()
    {
        if (UserId == 0) return;

        IsBusy = true;
        StatusMessage = "Cargando detalle del usuario...";

        var (data, error) = await _apiService.GetUserByIdAsync(UserId);

        if (error != null)
        {
            StatusMessage = error;
        }
        else if (data != null)
        {
            CurrentUser = data;
            StatusMessage = string.Empty;
        }

        IsBusy = false;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("UserId"))
        {
            // Almacenamos el ID recibido
            UserId = Convert.ToInt32(query["UserId"]);

            // Lo usamos para cargar datos dinámicamente
            _ = LoadUserDetailsAsync();
        }
    }
}