using CommunityToolkit.Maui.Views;

namespace VPOS.Views;

public partial class test_PopupPage : Popup//: ContentPage
{
    public static String m_StrResult;
    public Thread thread;
    public bool blnStop = false;

    private void ThreadStopClear()
    {
        if (thread != null)
        {
            blnStop = true;
            thread = null;
        }
    }
    [Obsolete]
    private void ThreadFun()
    {
        int intCount = 0;
        do
        {
            // 更新使用者介面 (UI) 元素
            Device.BeginInvokeOnMainThread(() =>
            {
                // 在這裡更新 UI 控制項
                labtime.Text = DateTime.Now.ToString("HH:mm:ss");
            });

            intCount++;
            Thread.Sleep(1000);
            if(intCount>=3)
            {
                blnStop = true;
            }
        } while (!blnStop);

        m_StrResult = "CloseBtn_Clicked";
        ThreadStopClear();

        // 更新使用者介面 (UI) 元素
        Device.BeginInvokeOnMainThread(() =>
        {
            // 在這裡更新 UI 控制項
            Close();
        });

    }
    public test_PopupPage()
    {
        InitializeComponent();
        m_StrResult = "";
        //Size = new Size(500, 500);//設定UI大小
        
        //---
        //建立執行序
        thread = new Thread(ThreadFun);
        thread.Start();
        //---建立執行序
    }

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        m_StrResult = "CloseBtn_Clicked";
        ThreadStopClear();
        Close();
    }
}