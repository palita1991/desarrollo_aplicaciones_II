
using System.Windows.Input;

namespace AppProfile.ViewModels
{
    public class MainViewModel
    {
        public string ProfileImageUrl { get; } = "avatar_boca_palavecino.jpeg";
        public string Name { get; } = "Palavecino Mariano Andres" ;
        public string Age { get; } = "35";
        public string Description { get; } = "Soy de SC de Bariloche pero actualmente vivo en Neuquen Capital. Trabajo de forma remota en una empresa desarrollando aplicaciones web. El contenido de esta materia no tuve oportunidad de utilizarlo en ningún proyecto.";
        public ICommand NavigateToProfileCommand { get; }

        public MainViewModel()
        {
            // Enlace al botón que nos redirige a MenuPage
            NavigateToProfileCommand = new Command(ExecuteNavigate);
        }

        private async void ExecuteNavigate()
        {
            // ## Armado del objeto ##
            // El dictionary es para enviar identificadores entre ViewModels.
            // Cuando funcione de forma dinámica, es decir, no este hardcodeado se reemplaza el id de prueba con el real.
            var parametros = new Dictionary<string, object>
            {
                { "UserId", 456 }
            };

            // ## Navegación ## 
            // Se ejecuta de forma asincronica la navegación a la pagina destino con los parámetros,
            // Se mantiene la vista separa de la lógica de enrutamiento
            await Shell.Current.GoToAsync(nameof(MainPage), parametros);
        }
    }
}
