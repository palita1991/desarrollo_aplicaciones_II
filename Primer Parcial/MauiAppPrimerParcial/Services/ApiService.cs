using System.Net.Http.Json;
using MauiAppPrimerParcial.Models;

namespace MauiAppPrimerParcial.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com/") };
    }

    public async Task<(List<User> Data, string ErrorMessage)> GetUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("users");
            return await ProcessResponse<List<User>>(response);
        }
        catch (HttpRequestException)
        {
            return (null, "Error de red: Verifique su conexión a internet.");
        }
        catch (Exception ex)
        {
            return (null, $"Error inesperado: {ex.Message}");
        }
    }

    public async Task<(User Data, string ErrorMessage)> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            return await ProcessResponse<User>(response);
        }
        catch (HttpRequestException)
        {
            return (null, "Error de red al intentar obtener los detalles.");
        }
        catch (Exception ex)
        {
            return (null, $"Error inesperado: {ex.Message}");
        }
    }

    // Método auxiliar para manejar los diferentes estados de respuesta HTTP y deserializar el contenido JSON.
    private async Task<(T Data, string ErrorMessage)> ProcessResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<T>();
            return (data, null);
        }

        // Manejo de errores
        string errorMsg = response.StatusCode switch
        {
            System.Net.HttpStatusCode.BadRequest => "Error 400: Petición incorrecta.",
            System.Net.HttpStatusCode.Unauthorized => "Error 401: No autorizado.",
            System.Net.HttpStatusCode.NotFound => "Error 404: Recurso no encontrado.",
            System.Net.HttpStatusCode.InternalServerError => "Error 500: Error interno del servidor.",
            _ => $"Error HTTP: {(int)response.StatusCode}"
        };

        return (default, errorMsg);
    }
}