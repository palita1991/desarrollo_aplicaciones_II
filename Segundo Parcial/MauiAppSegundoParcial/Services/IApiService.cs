using MauiAppSegundoParcial.Models;

namespace MauiAppSegundoParcial.Services;

// Contrato para el consumo de la API externa
public interface IApiService
{
    Task<(List<User> Data, string ErrorMessage)> GetUsersAsync();
    Task<(User Data, string ErrorMessage)> GetUserByIdAsync(int id);
    Task<(User? Data, string? ErrorMessage)> CreateUserAsync(User user);
    Task<(User? Data, string? ErrorMessage)> UpdateUserAsync(User user);
    Task<(bool IsSuccess, string? ErrorMessage)> DeleteUserAsync(int id);
}