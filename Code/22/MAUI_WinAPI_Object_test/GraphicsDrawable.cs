//https://learn.microsoft.com/zh-tw/dotnet/maui/user-interface/controls/graphicsview
//https://learn.microsoft.com/zh-tw/dotnet/maui/user-interface/graphics/draw


namespace MAUI_WinAPI_Object_test
{
    public class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Drawing code goes here
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine(0, 0, 30, 40);
            canvas.DrawLine(30, 0, 0, 40);

            canvas.FontColor = Colors.Blue;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("S", 10, 10, 20, 20, HorizontalAlignment.Left, VerticalAlignment.Top);
        }
    }
}
