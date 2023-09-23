namespace MauiChangeGridView;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    private void Go_Clicked(object sender, EventArgs e)
    {
		G01.IsVisible = false;//隱藏元件
        //---
        //動態調整版面配置
        FullGrid.RowDefinitions[0] = new RowDefinition(new GridLength(0));
        FullGrid.RowDefinitions[1] = new RowDefinition(new GridLength(3, GridUnitType.Star));
        //---動態調整版面配置
    }
}

