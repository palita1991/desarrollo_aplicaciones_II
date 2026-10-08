using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppPrimerParcial.Interfaces;
using MauiAppPrimerParcial.Models;
using MauiAppPrimerParcial.Services;

namespace MauiAppPrimerParcial.ViewModels;

// Lógica del detalle de usuario
public partial class DetailViewModel : ObservableObject, IQueryAttributable
{
    private readonly IApiService _apiService;
    private readonly IUserRepository _userRepository;

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private User currentUser = new User();

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public DetailViewModel(IApiService apiService, IUserRepository userRepository)
    {
        _apiService = apiService;
        _userRepository = userRepository;
    }

    private async Task LoadUserDetailsAsync()
    {
        if (UserId == 0) return;

        IsBusy = true;
        StatusMessage = "Cargando detalle...";

        // Buscar en API
        var (data, error) = await _apiService.GetUserByIdAsync(UserId);

        if (data != null)
        {
            CurrentUser = data;
            StatusMessage = string.Empty;
        }
        else
        {
            // Si la API falla, se busca en la base de datos
            var localUser = await _userRepository.GetUserByIdAsync(UserId);

            if (localUser != null)
            {
                CurrentUser = localUser;
                StatusMessage = "Modo offline: Datos locales.";
            }
            else
            {
                StatusMessage = "Error: Usuario no encontrado.";
            }
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