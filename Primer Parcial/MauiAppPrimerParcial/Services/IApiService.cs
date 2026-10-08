using MauiAppPrimerParcial.Models;

namespace MauiAppPrimerParcial.Services;

// Contrato para el consumo de la API externa
public interface IApiService
{
    Task<(List<User> Data, string ErrorMessage)> GetUsersAsync();
    Task<(User Data, string ErrorMessage)> GetUserByIdAsync(int id);
}