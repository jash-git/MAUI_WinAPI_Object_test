using Microsoft.Maui.Controls;

namespace MAUI_WinAPI_Object_test.CustomControls;

public partial class BadgeButton : ContentView
{
	public BadgeButton()
    {
		InitializeComponent();
        BaseImage.Source = ImageSource.FromFile("dotnet_bot.png");
    }

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        double widthRequest = WidthRequest;
        if(widthRequest > 150) 
        {
            CircleBoxView.WidthRequest = 40;
            CircleBoxView.HeightRequest = 40;
            CircleBoxView.CornerRadius = 20;
            BadgeLabel.Margin=new Thickness(0, 10, 13, 0);
        }
    }
}