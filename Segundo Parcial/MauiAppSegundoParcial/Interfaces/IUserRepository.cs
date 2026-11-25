using MauiAppSegundoParcial.Models;

namespace MauiAppSegundoParcial.Interfaces;

// Contrato para definir las operaciones de acceso a la base de datos para la entidad User
public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<int> SaveUsersAsync(IEnumerable<User> users);
    Task<int> SaveUserAsync(User user);
    Task<User> GetUserByIdAsync(int id);
}
