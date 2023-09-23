namespace MauiContentPageSwitch;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";
		
		//---
		//頁面切換
        //NavigationPage navigationPage = new NavigationPage(new NewPage1());
        await Navigation.PushModalAsync(new NewPage1());
        //---頁面切換
    }
}

