using MauiAppPrimerParcial.Views;

namespace MauiAppPrimerParcial
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("UserDetail", typeof(Views.DetailPage));
        }
    }
}
