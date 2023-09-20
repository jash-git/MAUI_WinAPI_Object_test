namespace MauiApp1;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        lb01.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                // 在這裡處理點擊事件
                // 可以執行您想要的操作
                // 例如，顯示一個對話框或導航到另一個頁面
                DisplayAlert("提示", "標籤被點擊了！", "確定");
            })
        });

    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        DisplayAlert("提示", "標籤被點擊了！", "確定");
    }

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        DisplayAlert("提示", "圖片被點擊了！", "確定");
    }
}

