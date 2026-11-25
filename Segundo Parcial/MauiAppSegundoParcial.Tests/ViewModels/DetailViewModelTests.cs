using MauiAppSegundoParcial.Models;
using MauiAppSegundoParcial.Tests.Fakes;
using MauiAppSegundoParcial.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace MauiAppSegundoParcial.Tests.ViewModels
{
    public class DetailViewModelTests
    {
        [Fact]
        public async Task LoadUserDetailsAsync_ConIdValido_DeberiaAsignarCurrentUser()
        {
            // Se prepara entorno de prueba con fakes
            var repoFake = new UserRepositoryFake();
            var apiFake = new ApiServiceFake(); // Asumiendo que creaste un fake para tu API

            // Insertamos un usuario de prueba en nuestro Fake local
            var usuarioPrueba = new User { Id = 1, Name = "Mariano", Username = "mpalavecino" };
            await repoFake.SaveUserAsync(usuarioPrueba);

            // Instanciamos el ViewModel inyectando los Fakes
            var viewModel = new DetailViewModel(apiFake, repoFake)
            {
                UserId = 1 
            };

            await viewModel.LoadUserDetailsAsync();

            // Verificación de resultados
            Assert.NotNull(viewModel.CurrentUser);
            Assert.Equal("Leanne Graham", viewModel.CurrentUser.Name);
            Assert.Equal(string.Empty, viewModel.StatusMessage);
            Assert.False(viewModel.IsBusy);
        }

        [Fact]
        public async Task LoadUserDetailsAsync_ConIdInvalido_CurrentUserDeberiaTenerDatosVacios()
        {
            var apiFake = new ApiServiceFake();
            var repoFake = new UserRepositoryFake();
            var viewModel = new DetailViewModel(apiFake, repoFake);

            viewModel.UserId = 999;

            await viewModel.LoadUserDetailsAsync();

            Assert.NotNull(viewModel.CurrentUser);
            Assert.Equal(0, viewModel.CurrentUser.Id);
        }

        [Fact]
        public async Task CreateUserAsync_UsuarioValido_DeberiaRetornarUsuarioConIdAsignado()
        {
            var apiFake = new ApiServiceFake();
            var nuevoUsuario = new User
            {
                Name = "Mariano Palavecino",
                Username = "palita1991",
                Email = "mariano@ejemplo.com"
            };

            var resultado = await apiFake.CreateUserAsync(nuevoUsuario);

            Assert.NotNull(resultado.Data);
            Assert.Equal("Mariano Palavecino", resultado.Data.Name);
            Assert.Null(resultado.ErrorMessage);
        }

        [Fact]
        public async Task DeleteUserAsync_ConIdValido_DeberiaRetornarExito()
        {
            var apiFake = new ApiServiceFake();
            int idAEliminar = 1;

            var resultado = await apiFake.DeleteUserAsync(idAEliminar);

            Assert.True(resultado.IsSuccess);
            Assert.Null(resultado.ErrorMessage);
        }
    }
}