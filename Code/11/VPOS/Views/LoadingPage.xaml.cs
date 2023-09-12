namespace VPOS.Views;
/*  	
    Building a login flow with .NET MAUI
        https://www.c-sharpcorner.com/blogs/building-a-login-flow-with-net-maui
*/
public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
	}
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        /*
        if (await isAuthenticated())
        {
            await Shell.Current.GoToAsync("///home");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
        */
        await Task.Delay(7000);
        await Shell.Current.GoToAsync("main");
        base.OnNavigatedTo(args);
    }

    async Task<bool> isAuthenticated()
    {
        
        await Task.Delay(2000);
        var hasAuth = await SecureStorage.GetAsync("hasAuth");
        return !(hasAuth == null);
    }
}