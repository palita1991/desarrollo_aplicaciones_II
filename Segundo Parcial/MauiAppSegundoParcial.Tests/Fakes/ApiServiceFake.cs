using MauiAppSegundoParcial.Interfaces;
using MauiAppSegundoParcial.Models;
using MauiAppSegundoParcial.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiAppSegundoParcial.Tests.Fakes
{
    public class ApiServiceFake : IApiService
    {
        // Lista en memoria que simula la base de datos remota
        private readonly List<User> _apiUsers = new()
        {
            new User { Id = 1, Name = "Leanne Graham", Username = "Bret", Email = "Sincere@april.biz" },
            new User { Id = 2, Name = "Ervin Howell", Username = "Antonette", Email = "Shanna@melissa.tv" }
        };

        // Fíjate cómo ahora devolvemos (Data, ErrorMessage) en todos los métodos

        public Task<(List<User> Data, string ErrorMessage)> GetUsersAsync()
        {
            return Task.FromResult((_apiUsers.ToList(), (string)null));
        }

        public Task<(User Data, string ErrorMessage)> GetUserByIdAsync(int id)
        {
            var user = _apiUsers.FirstOrDefault(u => u.Id == id);
            return Task.FromResult((user, (string)null));
        }

        public Task<(User? Data, string? ErrorMessage)> CreateUserAsync(User user)
        {
            user.Id = _apiUsers.Any() ? _apiUsers.Max(u => u.Id) + 1 : 1;
            _apiUsers.Add(user);

            return Task.FromResult<(User?, string?)>((user, null));
        }

        public Task<(User? Data, string? ErrorMessage)> UpdateUserAsync(User user)
        {
            var existente = _apiUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existente != null)
            {
                var index = _apiUsers.IndexOf(existente);
                _apiUsers[index] = user;
                return Task.FromResult<(User?, string?)>((user, null));
            }

            return Task.FromResult<(User?, string?)>((null, "Usuario no encontrado"));
        }

        public Task<(bool IsSuccess, string? ErrorMessage)> DeleteUserAsync(int id)
        {
            var user = _apiUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _apiUsers.Remove(user);
                return Task.FromResult<(bool, string?)>((true, null));
            }

            return Task.FromResult<(bool, string?)>((false, "Usuario no encontrado"));
        }
    }
}