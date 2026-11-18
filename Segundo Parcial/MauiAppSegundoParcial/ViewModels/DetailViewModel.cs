using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppSegundoParcial.Interfaces;
using MauiAppSegundoParcial.Models;
using MauiAppSegundoParcial.Services;
using System.Windows.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.Devices.Sensors;

namespace MauiAppSegundoParcial.ViewModels;

// Lógica del detalle de usuario
public partial class DetailViewModel : ObservableObject, IQueryAttributable
{
    private readonly IApiService _apiService;
    private readonly IUserRepository _userRepository;
    public ICommand UpdateCommand { get; }

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private User currentUser = new User();

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private ImageSource profileImageSource = "dotnet_bot.png"; // Imagen por defecto

    [ObservableProperty]
    private string currentLocationText = "Ubicación desconocida";

    public ICommand TakePhotoCommand { get; }
    public ICommand GetLocationCommand { get; }

    public DetailViewModel(IApiService apiService, IUserRepository userRepository)
    {
        _apiService = apiService;
        _userRepository = userRepository;
        UpdateCommand = new Command(ExecuteUpdate);
        TakePhotoCommand = new Command(ExecuteTakePhoto);
        GetLocationCommand = new Command(ExecuteGetLocation);
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("UsuarioSeleccionado", out var obj) && obj is User user)
        {
            CurrentUser = user;
            UserId = user.Id;

            if (user.Address != null && !string.IsNullOrWhiteSpace(user.Address.City))
            {
                CurrentLocationText = $"{user.Address.Street}, {user.Address.City}";
            }
            else
            {
                CurrentLocationText = "Ubicación desconocida";
            }

            query.Clear();
            await LoadUserDetailsAsync();
        }
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
                if (!string.IsNullOrEmpty(CurrentUser.Name))
                {
                    StatusMessage = "Modo offline: Usuario nuevo en memoria local.";
                }
                else
                {
                    StatusMessage = "Error: Usuario no encontrado.";
                }
            }
        }

        IsBusy = false;
    }

    private async void ExecuteUpdate()
    {
        IsBusy = true;
        StatusMessage = "Guardando cambios...";

        // PUT de la API
        var (updatedUser, errorMessage) = await _apiService.UpdateUserAsync(CurrentUser);

        if (updatedUser != null)
        {
            // Se guarda en SQLite para el modo offline
            await _userRepository.SaveUserAsync(CurrentUser);

            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario actualizado correctamente.", "OK");

            // Se vuelve a la pantalla principal
            var parametros = new Dictionary<string, object>
        {
            { "UsuarioActualizado", CurrentUser }
        };
            await Shell.Current.GoToAsync("..", parametros);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
        }

        IsBusy = false;
    }

    private async void ExecuteTakePhoto()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                // Captura la foto usando la cámara nativa
                FileResult foto = await MediaPicker.Default.CapturePhotoAsync();

                if (foto != null)
                {
                    // Carga la foto en la interfaz y cierra el flujo (Dispose)
                    var stream = await foto.OpenReadAsync();
                    ProfileImageSource = ImageSource.FromStream(() => stream);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Cámara", "Tu dispositivo no soporta la cámara.", "OK");
            }
        }
        catch (PermissionException)
        {
            await Application.Current.MainPage.DisplayAlert("Permiso denegado", "Debe otorgar acceso a la cámara.", "Aceptar");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void ExecuteGetLocation()
    {
        try
        {
            IsBusy = true;
            StatusMessage = "Obteniendo ubicación...";

            // Geolocalización
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            var ubicacion = await Geolocation.Default.GetLocationAsync(request);

            if (ubicacion != null)
            {
                // Se verifica la plataforma Android, IOS o Windows
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    // En Windows se muestra la ubicación en coordenadas.
                    CurrentLocationText = $"Lat: {ubicacion.Latitude:F2}, Lon: {ubicacion.Longitude:F2}";
                    StatusMessage = string.Empty;
                }
                else
                {
                    // En Android/iOS se hace la traducción de coordenadas a dirección usando Geocoding.
                    StatusMessage = "Traduciendo coordenadas...";
                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(ubicacion.Latitude, ubicacion.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        string ciudad = placemark.Locality ?? placemark.SubAdminArea;
                        CurrentLocationText = $"{ciudad}, {placemark.AdminArea}";
                    }
                    else
                    {
                        CurrentLocationText = $"Lat: {ubicacion.Latitude:F2}, Lon: {ubicacion.Longitude:F2}";
                    }
                    StatusMessage = string.Empty;
                }
            }
            else
            {
                StatusMessage = "No se pudo obtener la ubicación.";
            }
        }
        catch (FeatureNotSupportedException)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "GPS no disponible en este dispositivo.", "Aceptar");
        }
        catch (PermissionException)
        {
            await Application.Current.MainPage.DisplayAlert("Permiso", "Debe otorgar acceso a ubicación.", "Aceptar");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error Real", ex.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

}