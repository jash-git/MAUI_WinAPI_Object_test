using VPOS.CustomControls;

namespace VPOS
{
    public partial class MainPage : ContentPage
    {
        public CustomButton [] CategoryBtn = new CustomButton[8];
        public MainPage()
        {
            InitializeComponent();

            CategoryBtn[0] = Btn00;
            CategoryBtn[1] = Btn01;
            CategoryBtn[2] = Btn02;
            CategoryBtn[3] = Btn03;
            CategoryBtn[4] = Btn04;
            CategoryBtn[5] = Btn05;
            CategoryBtn[6] = Btn06;
            CategoryBtn[7] = Btn07;
            CategoryBtn[0].m_SID = 0;
            CategoryBtn[1].m_SID = 1;
            CategoryBtn[2].m_SID = 2;
            CategoryBtn[3].m_SID = 3;
            CategoryBtn[4].m_SID = 4;
            CategoryBtn[5].m_SID = 5;
            CategoryBtn[6].m_SID = 6;
            CategoryBtn[7].m_SID = 7;
            for(int i = 0;i< CategoryBtn.Length;i++)
            {
                CategoryBtn[i].Clicked += CategoryBtn_Clicked;
            }
        }
        private async void CategoryBtn_Clicked(object sender, EventArgs e)
        {
            CustomButton CustomButtonBuf = (CustomButton)(sender);

            await DisplayAlert("Alert", $"{CustomButtonBuf.m_SID};{CustomButtonBuf.Text}", "OK");//await
        }
        private void CloseBtn_Clicked(object sender, EventArgs e)
        {
            // Close the active window
            //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
            Application.Current.CloseWindow(GetParentWindow());
        }

    }
}