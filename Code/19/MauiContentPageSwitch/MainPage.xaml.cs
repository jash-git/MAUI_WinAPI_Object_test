namespace MauiContentPageSwitch;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

        //---
        //頁面切換
        var modalPage = new NewPage1();
        modalPage.Disappearing += (sender2, e2) =>
        {//您可以訂閱 ContentPage 的 Disappearing 事件。當 ContentPage 消失時，該事件將觸發。您可以使用此事件來執行在 ContentPage 關閉後要執行的操作。
            count++;//畫面關閉才會被執行
        };
        await Navigation.PushModalAsync(modalPage);
        //---頁面切換

    }
}

