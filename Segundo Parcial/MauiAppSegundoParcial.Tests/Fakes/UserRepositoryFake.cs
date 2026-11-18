using MauiAppSegundoParcial.Interfaces;
using MauiAppSegundoParcial.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiAppSegundoParcial.Tests.Fakes
{
    // Se simula la base de datos para las pruebas unitarias
    public class UserRepositoryFake : IUserRepository
    {
        private readonly List<User> _usuarios = new();

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_usuarios.ToList());
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return Task.FromResult(_usuarios.FirstOrDefault(u => u.Id == id));
        }

        public Task<int> SaveUserAsync(User user)
        {
            var existente = _usuarios.FirstOrDefault(u => u.Id == user.Id);
            if (existente != null) _usuarios.Remove(existente);

            _usuarios.Add(user);
            return Task.FromResult(1);
        }

        public Task<int> SaveUsersAsync(IEnumerable<User> users)
        {
            _usuarios.Clear();
            _usuarios.AddRange(users);
            return Task.FromResult(users.Count());
        }
    }
}