using MauiAppSegundoParcial.Views;

namespace MauiAppSegundoParcial
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("UserDetail", typeof(Views.DetailPage));
            Routing.RegisterRoute("AddUserPage", typeof(Views.AddUserPage));
        }
    }
}
