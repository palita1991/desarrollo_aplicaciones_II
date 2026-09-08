using AppProfile.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppProfile.ViewModels
{
    // Implementación deIQueryAttributable para la recepción de parámetros de navegación.
    public class ProfileViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        // Instancia del usuario
        private UserProfile _user;
        private int _currentUserId;

        // Evento INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        // Sirve para mostrar mensaje de estado en el formulario
        private string? _statusMessage;
        public string? StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        // Declaración del Command 
        public ICommand SaveCommand { get; }

        // Datos por defecto
        public ProfileViewModel()
        {
            _user = new UserProfile
            {
                Name = "Palavecino Mariano Andres",
                Age = 35,
                Description = "Soy de SC de Bariloche pero actualmente vivo en Neuquen Capital. Trabajo de forma remota en una empresa desarrollando aplicaciones web. El contenido de esta materia no tuve oportunidad de utilizarlo en ningún proyecto.",
                ProfileImageUrl = "avatar_boca_palavecino.jpeg"
            };

            Age = _user.Age.ToString();

            // Inicialización del Command
            SaveCommand = new Command(ExecuteSave);
        }

        // ## Receptor de datos "" 
        // Intercepta el AppShell que se ejecuta automáticamente al recibir parámetros en la ruta.
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // ## Verificación inicial ## 
            // Se busca la clave "UserId" de forma optimizada para saber si no existe o es nula
            // Para evitar procesamiento innesario se aborta la ejecución 
            if (!query.TryGetValue("UserId", out var userIdObj) || userIdObj == null)
            {
                toast("Error: No se recibió ningún ID de usuario.");
                return;
            }

            // ## Verificación de formato ## 
            if (userIdObj is not int parsedId || parsedId <= 0)
            {
                toast("Error: El ID no tiene un formato válido.");
                return;
            }

            // ## Caso de éxito ##
            toast("Usuario cargado correctamente.");
        }

        // Notificación (Toast):
        // Controla la visibilidad del componente en la vista mediante data binding,
        // Informa al usuario el resultado
        private async void toast(string mensaje)
        {
            StatusMessage = mensaje;
            IsStatusVisible = true;

            await Task.Delay(3000); 

            IsStatusVisible = false;
            StatusMessage = string.Empty;
        }

        // Propiedades públicas expuestas en la vista (View)
        // Se notifica a la vista con OnPropertyChanged() cuando este cambia.
        public string Name
        {
            get => _user.Name;
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string? _ageInput;
        public string? Age
        {
            get => _ageInput;
            set
            {
                // Se filtran solo los números
                string soloNumeros = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    soloNumeros = new string(value.Where(char.IsDigit).ToArray());
                }

                if (_ageInput != soloNumeros)
                {
                    _ageInput = soloNumeros;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description
        {
            get => _user.Description;
            set
            {
                if (_user.Description != value)
                {
                    _user.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? ProfileImageUrl
        {
            get => _user.ProfileImageUrl;
            set
            {
                if (_user.ProfileImageUrl != value)
                {
                    _user.ProfileImageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isStatusVisible;
        public bool IsStatusVisible
        {
            get => _isStatusVisible;
            set
            {
                if (_isStatusVisible != value)
                {
                    _isStatusVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        // Propiedades para mostrar los errores en pantalla dependiendo que dato falte completar o sea inválido
        private string? _nameError;
        public string? NameError
        {
            get => _nameError;
            set { _nameError = value; OnPropertyChanged(); }
        }

        private string? _ageError;
        public string? AgeError
        {
            get => _ageError;
            set { _ageError = value; OnPropertyChanged(); }
        }

        // Validación y Guardado. Si es exitoso se muestra un cartel abajo por 5 segundos
        private async void ExecuteSave()
        {
            NameError = string.Empty;
            AgeError = string.Empty;
            StatusMessage = string.Empty;

            bool hasErrors = false;

            // Se realiza la validación y cambia el booleano para notificar al usuario en caso de que haya un error en cualquiera de los campos
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameError = "El nombre comleto es un campo obligatorio.";
                hasErrors = true;
            }

            if (string.IsNullOrWhiteSpace(Age))
            {
                AgeError = "La edad es un campo obligatorio.";
                hasErrors = true;
            }
            else if (!int.TryParse(Age, out int edadParseada) || edadParseada <= 0)
            {
                AgeError = "Ingresa una edad válida mayor a 0.";
                hasErrors = true;
            }
            else
            {
                _user.Age = edadParseada;
            }

            if (hasErrors) return;

            //  Notificación manual para las vistas
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Age));
            OnPropertyChanged(nameof(Description));

            toast("¡Perfil guardado con éxito!");
        }

        // Método para visualizar que los datos han sufrido cambios
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
