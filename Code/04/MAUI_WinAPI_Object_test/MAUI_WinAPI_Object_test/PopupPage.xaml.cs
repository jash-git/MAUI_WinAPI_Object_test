using CommunityToolkit.Maui.Views;

namespace MAUI_WinAPI_Object_test;

public partial class PopupPage: Popup // : ContentPage
{
	public PopupPage()
	{
		InitializeComponent();
		//Size = new Size(500, 500);//�]�wUI�j�p
	}
    private void OnClosed(object sender, EventArgs e)
    {
		Close();
    }
}