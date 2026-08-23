using AppProfile.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppProfile.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        // Instancia del usuario
        private UserProfile _user;

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

            StatusMessage = "¡Perfil guardado con éxito!";
            IsStatusVisible = true;
            //  Notificación manual para las vistas
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Age));
            OnPropertyChanged(nameof(Description));

            await Task.Delay(5000);

            IsStatusVisible = false;
        }

        // Método para visualizar que los datos han sufrido cambios
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
