namespace Maui_ListView;

public class LoginItem
{
    public string role_sid { get; set; }
    public string Image { get; set; }
    public string user_account { get; set; }
    public string user_pwd { get; set; }
    public string employee_no { get; set; }
    public string SID { get; set; }
}

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        List<LoginItem> items = new List<LoginItem>();
        for(int i=0; i<10; i++)
        {
            LoginItem loginItemBuf = new LoginItem();
            loginItemBuf.role_sid = $"0000{i}";
            loginItemBuf.Image = "dotnet_bot.png";
            loginItemBuf.user_account = $"0000{i}";
            items.Add(loginItemBuf);
        }

        lvUser.ItemsSource = items;
    }

    private async void OnLabelClicked(object sender, EventArgs e)
    {//子元件事件
        await DisplayAlert("LabelClicked", ((Label)(sender)).Text, "確定");
    }

    private async void OnImageClicked(object sender, EventArgs e)
    {//子元件事件
        await DisplayAlert("ImageClicked", ((Image)(sender)).Source.ToString(), "確定");
        ((Image)(sender)).Source = ImageSource.FromFile("user.png");
    }

    private async void lvUser_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {//主元件的事件
        if (e.SelectedItem == null)
            return;

        LoginItem selectedItem = (LoginItem)e.SelectedItem;
        await DisplayAlert("ItemSelected", selectedItem.role_sid, "確定");
    }
}

