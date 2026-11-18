using MauiAppSegundoParcial.ViewModels;

namespace MauiAppSegundoParcial.Views;

public partial class AddUserPage : ContentPage
{
	public AddUserPage(AddUserViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}