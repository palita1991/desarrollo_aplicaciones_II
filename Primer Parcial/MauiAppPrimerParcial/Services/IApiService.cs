using MauiAppPrimerParcial.Models;

namespace MauiAppPrimerParcial.Services;

// Interfaz que define el contrato de un servicio de acceso a datos vía HTTP.
public interface IApiService
{
    Task<(List<User> Data, string ErrorMessage)> GetUsersAsync();
    Task<(User Data, string ErrorMessage)> GetUserByIdAsync(int id);
}