namespace MauCustBtn;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        CustBtn custBtn = new CustBtn();
        custBtn.m_intSID = 100;
        // 创建一个 TapGestureRecognizer
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped1;
        custBtn.GestureRecognizers.Add(tapGestureRecognizer);
        VS01.Add(custBtn);

    }

    private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
    {
        count--;
        if(count <0) 
        {
            count= ((CustBtn)(sender)).m_intSID;
        }
        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";
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
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";
    }
}

