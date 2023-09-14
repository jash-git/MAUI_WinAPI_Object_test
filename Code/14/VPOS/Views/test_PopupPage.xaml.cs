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
            // ��s�ϥΪ̤��� (UI) ����
            Device.BeginInvokeOnMainThread(() =>
            {
                // �b�o�̧�s UI ���
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

        // ��s�ϥΪ̤��� (UI) ����
        Device.BeginInvokeOnMainThread(() =>
        {
            // �b�o�̧�s UI ���
            Close();
        });

    }
    public test_PopupPage()
    {
        InitializeComponent();
        m_StrResult = "";
        //Size = new Size(500, 500);//�]�wUI�j�p
        
        //---
        //�إ߰����
        thread = new Thread(ThreadFun);
        thread.Start();
        //---�إ߰����
    }

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        m_StrResult = "CloseBtn_Clicked";
        ThreadStopClear();
        Close();
    }
}