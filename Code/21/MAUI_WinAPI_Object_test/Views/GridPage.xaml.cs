using CommunityToolkit.Maui.Views;
using Jint.Parser;

namespace MAUI_WinAPI_Object_test.Views;

public partial class GridPage : Popup //: ContentPage
{
	public GridPage()
	{
		InitializeComponent();
        // �Ыؤ@�� Grid �G��
        var grid = FullGrid;
        grid.BackgroundColor = Colors.AliceBlue;
        // �w�q�C�M��
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // �b���椤�K�[���
        var label1 = new Label { Text = "�Ĥ@��A�Ĥ@�C" };
        var label2 = new Label { Text = "�ĤG��A�Ĥ@�C" };
        var label3 = new Label { Text = "�Ĥ@��A�ĤG�C" };
        var label4 = new Label { Text = "�ĤG��A�ĤG�C" };




        grid.Add(label1, 0, 0);
        grid.Add(label2, 1, 0);
        grid.Add(label3, 0, 1);
        grid.Add(label4, 1, 1);
    }
}