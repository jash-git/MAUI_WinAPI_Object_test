namespace VPOS
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void CloseBtn_Clicked(object sender, EventArgs e)
        {
            // Close the active window
            //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
            Application.Current.CloseWindow(GetParentWindow());
        }
    }
}