using AppProfile.ViewModels;

namespace AppProfile;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}