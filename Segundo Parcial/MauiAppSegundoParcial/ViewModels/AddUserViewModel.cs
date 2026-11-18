using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppSegundoParcial.Models;
using MauiAppSegundoParcial.Services;

namespace MauiAppSegundoParcial.ViewModels;

public partial class AddUserViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string phone;

    [ObservableProperty]
    private bool isBusy;

    public ICommand SaveCommand { get; }

    public AddUserViewModel(IApiService apiService)
    {
        _apiService = apiService;
        SaveCommand = new Command(ExecuteSave);
    }

    private async void ExecuteSave()
    {
        // 1. Validaciones básicas
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "El nombre y el email son obligatorios.", "OK");
            return;
        }

        IsBusy = true;

        // 2. Armamos el objeto con los datos del formulario
        var newUser = new User
        {
            Name = this.Name,
            Username = this.Username,
            Email = this.Email,
            Phone = this.Phone
        };

        // 3. Llamamos al POST de la API
        var (createdUser, errorMessage) = await _apiService.CreateUserAsync(newUser);

        IsBusy = false;

        if (createdUser != null)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario creado correctamente.", "OK");

            // 4. Volvemos a la pantalla principal mandándole el usuario recién creado
            var parametros = new Dictionary<string, object>
            {
                { "NuevoUsuario", createdUser }
            };
            await Shell.Current.GoToAsync("..", parametros);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
        }
    }
}