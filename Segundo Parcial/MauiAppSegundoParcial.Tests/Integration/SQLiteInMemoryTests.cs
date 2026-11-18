using SQLite;
using MauiAppSegundoParcial.Models;
using System.Threading.Tasks;
using Xunit;

namespace MauiAppSegundoParcial.Tests.Integration
{
    public class SQLiteInMemoryTests
    {
        [Fact]
        public async Task InsertarUsuario_EnSQLiteInMemory_DeberiaGuardarYRecuperarDatos()
        {
            var connection = new SQLiteAsyncConnection(":memory:");

            // Creamos la tabla real en la memoria
            await connection.CreateTableAsync<User>();

            var usuarioPrueba = new User
            {
                Id = 1,
                Name = "Mariano Test In-Memory",
                Username = "mariano_db"
            };

            await connection.InsertAsync(usuarioPrueba);
            var usuarioRecuperado = await connection.Table<User>().FirstOrDefaultAsync(u => u.Id == 1);

            Assert.NotNull(usuarioRecuperado);
            Assert.Equal("Mariano Test In-Memory", usuarioRecuperado.Name);

            // Limpieza
            await connection.CloseAsync();
        }
    }
}