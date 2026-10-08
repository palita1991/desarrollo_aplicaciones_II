using MauiAppPrimerParcial.ViewModels;

namespace MauiAppPrimerParcial.Views
{
    public partial class DetailPage : ContentPage
    {
        // Contenedor de dependencias para inyección de dependencias
        public DetailPage(DetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm; 
        }
    }
}