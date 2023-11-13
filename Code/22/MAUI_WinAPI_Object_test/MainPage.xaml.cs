using CommunityToolkit.Maui.Views;
using System.Threading;
using MAUI_WinAPI_Object_test.Views;
using MAUI_WinAPI_Object_test.CustomControls;


namespace MAUI_WinAPI_Object_test
{
    public partial class MainPage : ContentPage
    {
        public int count = 0;
        public IDispatcherTimer timmer { get; set; }
        public Thread thread;
        public bool blnStop = false;
        public MainPage()
        {
            InitializeComponent();
            //*
            //---
            //Timer Mode
            //https://learn.microsoft.com/en-us/answers/questions/1207012/how-to-stop-device-starttimer
            timmer = Application.Current.Dispatcher.CreateTimer();
            timmer.Interval = new TimeSpan(0, 0, 0, 0, 100);//天/時/分/秒/毫秒
            timmer.Tick += Timmer_Tick;
            timmer.IsRepeating = true;//the timer will keep recurring, you can set false
            timmer.Start();
            //---Timer Mode
            //*/
            // 在新執行緒中執行工作

            /*
            thread = new Thread(ThreadFun);
            thread.Start();
            */
        }
        private void Timmer_Tick(object sender, EventArgs e)
        {
            //---
            //抓螢幕解析度
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double width = displayInfo.Width;
            double height = displayInfo.Height;
            double density = displayInfo.Density;
            //---抓螢幕解析度
            labDisplayInfo.Text = $"{width} x {height} ; {density}";

            labtime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        [Obsolete]
        private void ThreadFun()
        {
            do
            {
                // 更新使用者介面 (UI) 元素
                Device.BeginInvokeOnMainThread(() =>
                {
                    // 在這裡更新 UI 控制項
                    labtime.Text = DateTime.Now.ToString("HH:mm:ss");
                });

                Thread.Sleep(1000);
            } while (!blnStop);

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

        private void CloseBtn_Clicked(object sender, EventArgs e)
        {
            if (timmer != null)
            {
                timmer.Stop();
            }

            if (thread != null)
            {
                blnStop = true;
                thread = null;
            }
            // Close the active window
            //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
            Application.Current.CloseWindow(GetParentWindow());
        }

        private async void PopupBtn_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Alert", $"路徑:{AppDomain.CurrentDomain.BaseDirectory}", "OK");//await

            Class_Test.MainPagePoint = this;

            //await Class_Test.ShowPopup();
            //int i=await CallShowPopup();
            //if (await Class_Test.CallShowPopup()>100)
            if (await Class_Call.CallShowPopup() > 100)
            {
                labtime.IsVisible = !labtime.IsVisible;//元件隱藏/顯示

                await DisplayAlert("Alert", $"{PopupBtn.CustomProperty};{PopupPage1.m_StrResult}", "OK");//await
            }


        }

        private async Task<int> CallShowPopup()
        {
            int intResult = 0;
            await Class_Test.ShowPopup();
            return intResult;
        }
    }
}