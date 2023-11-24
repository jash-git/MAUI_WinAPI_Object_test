using CommunityToolkit.Maui.Views;
using Jint.Parser;

namespace MAUI_WinAPI_Object_test.Views;

public partial class GridPage : Popup //: ContentPage
{
	public GridPage()
	{
		InitializeComponent();
        // 創建一個 Grid 佈局
        var grid = FullGrid;
        grid.BackgroundColor = Colors.AliceBlue;
        // 定義列和行
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // 在網格中添加控制項
        var label1 = new Label { Text = "第一行，第一列" };
        var label2 = new Label { Text = "第二行，第一列" };
        var label3 = new Label { Text = "第一行，第二列" };
        var label4 = new Label { Text = "第二行，第二列" };




        grid.Add(label1, 0, 0);
        grid.Add(label2, 1, 0);
        grid.Add(label3, 0, 1);
        grid.Add(label4, 1, 1);
    }
}