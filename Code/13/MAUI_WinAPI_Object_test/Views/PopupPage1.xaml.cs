using CommunityToolkit.Maui.Views;

namespace MAUI_WinAPI_Object_test.Views;

public partial class PopupPage1 : Popup//: ContentPage
{
    public static String m_StrResult;
	public PopupPage1()
	{
		InitializeComponent();
        m_StrResult = "";
        //Size = new Size(500, 500);//設定UI大小
    }

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        m_StrResult = "CloseBtn_Clicked";
        Close();
    }
}