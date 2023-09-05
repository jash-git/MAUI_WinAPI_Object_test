//using Android.App;
using Microsoft.Maui.Controls;

namespace MAUI_WinAPI_Object_test;
public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";
	}
    private void OnClosed(object sender, EventArgs e)
    {
        // Close the active window
        //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
        Application.Current.CloseWindow(GetParentWindow());
    }
}

