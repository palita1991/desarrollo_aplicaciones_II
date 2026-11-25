using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppSegundoParcial.Interfaces;
using MauiAppSegundoParcial.Models;
using MauiAppSegundoParcial.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppSegundoParcial.ViewModels;

// Planificación de la lógica de la pantalla principal
public partial class MainViewModel : ObservableObject, IQueryAttributable
{
    //Dependencias
    private readonly IApiService _apiService;
    private readonly IUserRepository _userRepository;
    public ICommand DeleteCommand { get; }

    public ICommand GoToAddCommand { get; }

    [ObservableProperty]
    private ObservableCollection<User> users;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    private List<User> _allUsers = new();

    [ObservableProperty]
    private string searchQuery;

    [ObservableProperty]
    private bool hasData=false;
    public MainViewModel(IApiService apiService, IUserRepository userRepository)
    {
        _apiService = apiService;
        _userRepository = userRepository;
        Users = new ObservableCollection<User>();
        DeleteCommand = new Command<int>(ExecuteDelete);
        GoToAddCommand = new Command(ExecuteGoToAdd);
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
            _allUsers = apiUsers.ToList();
            Users = new ObservableCollection<User>(apiUsers);
            HasData = true;
            StatusMessage = "Datos actualizados desde la red.";
        }
        else
        {
            // Si falla la API por falta de internet, se recurre a la base de datos local
            var localUsers = await _userRepository.GetAllUsersAsync();
            if (localUsers.Any())
            {
                _allUsers = localUsers.ToList();
                Users = new ObservableCollection<User>(localUsers);
                HasData = true;
                StatusMessage = "Modo offline: Mostrando datos guardados base de datos local.";
            }
            else
            {
                HasData = false;
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

        var userToView = Users.FirstOrDefault(u => u.Id == selectedUserId);
        if (userToView == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "UsuarioSeleccionado", userToView }
        };

        // Navegamos enviando el paquete
        await Shell.Current.GoToAsync("UserDetail", parameters);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("NuevoUsuario", out var obj) && obj is User newUser)
        {
            Users.Insert(0, newUser);

            query.Clear();
        }

        if (query.TryGetValue("UsuarioActualizado", out var updatedObj) && updatedObj is User updatedUser)
        {
            // Se busca el usuario actualizado en la lista actual y se reemplaza por el nuevo para reflejar los cambios
            var oldUser = Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (oldUser != null)
            {
                // Se obtiene el índice del usuario antiguo en la colección
                int index = Users.IndexOf(oldUser);

                // Se reemplaza el usuario antiguo por el nuevo
                Users[index] = updatedUser;
            }
            query.Clear();
        }

        if (query.TryGetValue("DeletedUserId", out var deletedIdObj) && deletedIdObj is int deletedId)
        {
            // Se busca el usuario eliminado en la lista actual y se elimina de la colección
            var userToRemove = Users.FirstOrDefault(u => u.Id == deletedId);

            if (userToRemove != null)
            {
                // Se elimina de la colección observable para que la UI se actualice automáticamente
                Users.Remove(userToRemove);
            }
        }
    }

    private async void ExecuteDelete(int selectedUserId)
    {
        var userToDelete = Users.FirstOrDefault(u => u.Id == selectedUserId);

        if (userToDelete == null) return;
        bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas eliminar este usuario?", "Sí", "No");
        if (!confirm) return;

        var (isSuccess, errorMessage) = await _apiService.DeleteUserAsync(selectedUserId);

        if (isSuccess)
        {
            Users.Remove(userToDelete);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
        }
    }

    private async void ExecuteGoToAdd()
    {
        // Navegamos al formulario
        await Shell.Current.GoToAsync("AddUserPage");
    }

    partial void OnSearchQueryChanged(string value)
    {
        // Si se borra se muestra el listado completo
        if (string.IsNullOrWhiteSpace(value))
        {
            Users = new ObservableCollection<User>(_allUsers);
            return;
        }

        // Se convierte a minúscula
        var query = value.ToLower();

        var filtrados = _allUsers.Where(u =>
            (u.Name != null && u.Name.ToLower().Contains(query)) ||
            (u.Email != null && u.Email.ToLower().Contains(query))
        ).ToList();

        Users = new ObservableCollection<User>(filtrados);
    }
}