using MauiAppSegundoParcial.ViewModels;

namespace MauiAppSegundoParcial.Views
{
    public partial class MainPage : ContentPage
    {
        // Contenedor de dependencias para inyección de dependencias
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}