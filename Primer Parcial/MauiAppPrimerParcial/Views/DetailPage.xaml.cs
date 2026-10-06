using MauiAppPrimerParcial.ViewModels;

namespace MauiAppPrimerParcial.Views // <-- Asegúrate de que tenga .Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage(DetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm; // Conecta el XAML con el ViewModel
        }
    }
}