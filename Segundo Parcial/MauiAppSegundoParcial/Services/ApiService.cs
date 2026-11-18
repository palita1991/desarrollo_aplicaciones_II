using MauiAppSegundoParcial.Services;
using MauiAppSegundoParcial.Models;
using System.Net;
using System.Text.Json;
using System.Text;

namespace MauiAppSegundoParcial.Services;

// Implementación del servicio de API para interactuar con el endpoint 
public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            // Definimos un límite de tiempo para que pueda dispararse TaskCanceledException si la red cuelga
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<(List<User>? Data, string? ErrorMessage)> GetUsersAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("users");
            return await ProcessResponse<List<User>>(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            return (null, "Error de red: Verifique su conexión a internet.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            return (null, $"Error HTTP inesperado ({(int)ex.StatusCode}): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "La solicitud tardó demasiado. Intente nuevamente.");
        }
        // Capturamos errores si el JSON recibido está roto o no coincide con la clase
        catch (JsonException)
        {
            return (null, "Los datos recibidos no tienen el formato esperado.");
        }
        // Red de contención final para cualquier otro error no previsto
        catch (Exception ex)
        {
            return (null, $"Ocurrió un error inesperado: {ex.Message}");
        }
    }

    public async Task<(User? Data, string? ErrorMessage)> GetUserByIdAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"users/{id}");
            return await ProcessResponse<User>(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            return (null, "Error de red: Verifique su conexión a internet.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            return (null, $"Error HTTP inesperado ({(int)ex.StatusCode}): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "La solicitud tardó demasiado. Intente nuevamente.");
        }
        catch (JsonException)
        {
            return (null, "Los datos recibidos no tienen el formato esperado.");
        }
        catch (Exception ex)
        {
            return (null, $"Error inesperado: {ex.Message}");
        }
    }

    public async Task<(User? Data, string? ErrorMessage)> CreateUserAsync(User user)
    {
        try
        {
            string json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("users", content);
            return await ProcessResponse<User>(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            return (null, "Error de red: Verifique su conexión a internet.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            return (null, $"Error HTTP inesperado ({(int)ex.StatusCode}): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "La solicitud tardó demasiado. Intente nuevamente.");
        }
        catch (JsonException)
        {
            return (null, "Los datos recibidos no tienen el formato esperado.");
        }
        catch (Exception ex)
        {
            return (null, $"Error inesperado: {ex.Message}");
        }
    }

    public async Task<(User? Data, string? ErrorMessage)> UpdateUserAsync(User user)
    {
        try
        {
            string json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"users/{user.Id}", content);
            return await ProcessResponse<User>(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            return (null, "Error de red: Verifique su conexión a internet.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            return (null, $"Error HTTP inesperado ({(int)ex.StatusCode}): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "La solicitud tardó demasiado. Intente nuevamente.");
        }
        catch (JsonException)
        {
            return (null, "Los datos recibidos no tienen el formato esperado.");
        }
        catch (Exception ex)
        {
            return (null, $"Error inesperado: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteUserAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"users/{id}");

            if (response.IsSuccessStatusCode)
                return (true, null);

            // Si falla, reutilizamos tu manejador de errores
            var result = await ProcessResponse<object>(response);
            return (false, result.ErrorMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == null)
        {
            return (false, "Error de red: Verifique su conexión a internet.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            return (false, $"Error HTTP inesperado ({(int)ex.StatusCode}): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (false, "La solicitud tardó demasiado. Intente nuevamente.");
        }
        catch (JsonException)
        {
            return (false, "Los datos recibidos no tienen el formato esperado.");
        }
        catch (Exception ex)
        {
            return (false, $"Error inesperado: {ex.Message}");
        }
    }

    // Método auxiliar para manejar los diferentes estados de respuesta HTTP y deserializar el contenido JSON.
    private async Task<(T? Data, string? ErrorMessage)> ProcessResponse<T>(HttpResponseMessage response)where T : class
    {
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<T>(json, options);

            return (data, null);
        }

        // Manejo de errores
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => (null, "Error 400: Petición incorrecta."),
            HttpStatusCode.Unauthorized => (null, "Error 401: No autorizado."),
            HttpStatusCode.NotFound => (null, "Error 404: Recurso no encontrado."),
            HttpStatusCode.InternalServerError => (null, "Error 500: Error interno del servidor."),
            _ => (null, $"Error HTTP: {(int)response.StatusCode} ({response.ReasonPhrase}).")
        };
    }
}