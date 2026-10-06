using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppPrimerParcial.Models;

using MauiAppPrimerParcial.Services;

namespace MauiAppPrimerParcial.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<User> users;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public MainViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Users = new ObservableCollection<User>();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        StatusMessage = "Cargando datos...";
        Users.Clear();

        var (data, error) = await _apiService.GetUsersAsync();

        if (error != null)
        {
            StatusMessage = error;
        }
        else if (data != null && data.Any())
        {
            StatusMessage = $"Se cargaron {data.Count} elementos.";
            // Cargamos solo los primeros 10 para no saturar la UI en esta prueba
            foreach (var post in data.Take(10))
            {
                Users.Add(post);
            }
        }

        IsBusy = false;
    }

    // Navegamos a una ruta semántica ("UserDetail") y enviamos el parámetro
    [RelayCommand]
    private async Task GoToDetailsAsync(int selectedUserId)
    {
        if (selectedUserId <= 0)
        {
            StatusMessage = "Error: El ID del usuario no es válido.";
            return;
        }

        StatusMessage = $"Navegando al usuario ID: {selectedUserId}...";

        var parameters = new Dictionary<string, object>
        {
            { "UserId", selectedUserId } // Pasamos el entero
        };

        await Shell.Current.GoToAsync("UserDetail", parameters);
    }
}