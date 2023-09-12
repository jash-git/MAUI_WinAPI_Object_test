namespace VPOS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            /*  	
                Building a login flow with .NET MAUI
                    https://www.c-sharpcorner.com/blogs/building-a-login-flow-with-net-maui
            */
            Routing.RegisterRoute("main", typeof(MainPage));
        }
    }
}