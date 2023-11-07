namespace MAUI_WinAPI_Object_test.CustomControls;

public partial class CardControl : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CardControl), propertyChanged: (bindable, oldValue, newValue) =>
	{
		var control= (CardControl)bindable;
		control.Titlelabel.Text=newValue as string;
	});
    public string Title
	{
		get =>GetValue(TitleProperty) as string;
		set => SetValue(TitleProperty, value);
	}

    public CardControl()
	{
		InitializeComponent();
	}
}