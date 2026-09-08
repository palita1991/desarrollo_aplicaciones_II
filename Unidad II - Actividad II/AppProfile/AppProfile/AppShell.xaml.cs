namespace AppProfile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Ruta de destino, en este caso la página de edición de perfil
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
