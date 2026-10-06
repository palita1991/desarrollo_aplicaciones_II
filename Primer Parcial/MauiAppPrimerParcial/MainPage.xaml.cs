using MauiAppPrimerParcial.ViewModels;

namespace MauiAppPrimerParcial.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}