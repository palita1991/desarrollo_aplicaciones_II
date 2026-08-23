using AppProfile.ViewModels;

namespace AppProfile
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new ProfileViewModel();
        }
        private void OnEdadTextChanged(object sender, TextChangedEventArgs e)
        {
            // Verificación que todos los caracteres sean numericos
            bool esValido = e.NewTextValue.All(char.IsDigit);

            if (!esValido)
            {
                // Al querer ingresar letras las borra automaticamente
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}
