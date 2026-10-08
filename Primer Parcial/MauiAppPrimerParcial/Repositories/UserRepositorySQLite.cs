using SQLite;
using MauiAppPrimerParcial.Models;
using MauiAppPrimerParcial.Interfaces;

namespace MauiAppPrimerParcial.Repositories;

// Clase que interactua con la base de datos SQLite para recuperar usuarios
public class UserRepositorySQLite : IUserRepository
{
    private readonly SQLiteAsyncConnection _db;

    public UserRepositorySQLite()
    {
        // Ruta de la base de datos
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db3");
        _db = new SQLiteAsyncConnection(dbPath);

        // Creación de la tabla si no existe
        _db.CreateTableAsync<User>().Wait();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _db.Table<User>().ToListAsync();
    }
    public async Task<User> GetUserByIdAsync(int id)
    {
        // Búsqueda de un usuario por su ID en la base de datos
        return await _db.Table<User>().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<int> SaveUsersAsync(IEnumerable<User> users)
    {
        // Limpieza y guardado de los usuarios en la base de datos
        await _db.DeleteAllAsync<User>();
        return await _db.InsertAllAsync(users);
    }
}