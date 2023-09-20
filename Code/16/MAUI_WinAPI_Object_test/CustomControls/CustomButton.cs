using Microsoft.Maui.Controls;

namespace MAUI_WinAPI_Object_test.CustomControls
{
    public class CustomButton : Button
    {
        // 新增您的自訂屬性
        public static readonly BindableProperty CustomPropertyProperty =
            BindableProperty.Create(nameof(CustomProperty), typeof(string), typeof(CustomButton), string.Empty);
        public string CustomProperty
        {
            get { return (string)GetValue(CustomPropertyProperty); }
            set { SetValue(CustomPropertyProperty, value); }
        }

        public CustomButton()
        {
            // 在此處設置自定義按鈕的外觀和行為
            // 例如，您可以設置背景色、文本顏色、字體大小等等
            BackgroundColor=Color.FromRgb((byte)0, (byte)0, (byte)255);
            TextColor = Color.FromRgb((byte)255, (byte)255, (byte)255);
        }

    }
}
