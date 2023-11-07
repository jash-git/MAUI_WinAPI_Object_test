using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;


namespace MAUI_WinAPI_Object_test.CustomControls;

public class CustmImageButton : ContentView
{
	public CustmImageButton()
	{
        var imageButton = new Button
        {
            /*
            Source = "dotnet_bot.png", // 替換為您的圖像文件路徑
            BackgroundColor = Colors.Transparent,
            Aspect = Aspect.AspectFit
            */
            Text = ""
        };
        var labelText = new Label
        {
            Text = "按鈕文本",
            TextColor = Colors.Red,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End
        };
        // 使用絕對佈局將文本覆蓋在圖片上


        AbsoluteLayout.SetLayoutBounds(imageButton, new Rect(0, 0, 10.0, 10.0));
        AbsoluteLayout.SetLayoutFlags(imageButton, AbsoluteLayoutFlags.All);


        AbsoluteLayout.SetLayoutBounds(labelText, new Rect(5.0, 8.0, 5.0, 2.0));
        AbsoluteLayout.SetLayoutFlags(labelText, AbsoluteLayoutFlags.PositionProportional);
        
        Content = new AbsoluteLayout
        {
            Children = { imageButton, labelText }
        };
    }
}