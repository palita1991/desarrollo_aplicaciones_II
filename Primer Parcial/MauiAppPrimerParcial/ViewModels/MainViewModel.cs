using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppPrimerParcial.Interfaces;
using MauiAppPrimerParcial.Models;
using MauiAppPrimerParcial.Services;
using System.Collections.ObjectModel;

namespace MauiAppPrimerParcial.ViewModels;

// Planificación de la lógica de la pantalla principal
public partial class MainViewModel : ObservableObject
{
    //Dependencias
    private readonly IApiService _apiService;
    private readonly IUserRepository _userRepository;

    [ObservableProperty]
    private ObservableCollection<User> users;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public MainViewModel(IApiService apiService, IUserRepository userRepository)
    {
        _apiService = apiService;
        _userRepository = userRepository;
        Users = new ObservableCollection<User>();
    }

    // Sincronización al intentar leer la API pero en el modo offline recurre a la base de datos
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        StatusMessage = "Cargando datos...";

        // Buscar en API
        var (apiUsers, error) = await _apiService.GetUsersAsync();

        if (apiUsers != null && apiUsers.Any())
        {
            // Si hay éxito, guardamos en la base de datos (Persistencia)
            await _userRepository.SaveUsersAsync(apiUsers);
            Users = new ObservableCollection<User>(apiUsers);
            StatusMessage = "Datos actualizados desde la red.";
        }
        else
        {
            // Si falla la API por falta de internet, se recurre a la base de datos local
            var localUsers = await _userRepository.GetAllUsersAsync();
            if (localUsers.Any())
            {
                Users = new ObservableCollection<User>(localUsers);
                StatusMessage = "Modo offline: Mostrando datos guardados base de datos local.";
            }
            else
            {
                StatusMessage = "Error de red y no hay datos locales guardados.";
            }
        }

        IsBusy = false;
    }

    // Se pasa la ID del usuario seleccionado para una busqueda concreta
    [RelayCommand]
    private async Task GoToDetailsAsync(int selectedUserId)
    {
        if (selectedUserId <= 0)
        {
            StatusMessage = "Error: El ID del usuario no es válido.";
            return;
        }


        var parameters = new Dictionary<string, object>
        {
            { "UserId", selectedUserId }
        };

        await Shell.Current.GoToAsync("UserDetail", parameters);
    }
}