
namespace VPOS
{
    public class WaitUIThread
    {
        public static bool m_blnUIfinish = false;
        public static void ShowAuthorizationWaitUI(String StrMsg, ParameterizedThreadStart fun)//顯示等待動畫
        {
            //ThreadLoginWait d = new ThreadLoginWait();
            //Thread t = new Thread(fun);
            //t.Start(d);
            //d.StartPosition = FormStartPosition.CenterParent;
            //d.m_StrInput = StrMsg;
            //d.ShowDialog();
        }

        public static void ShowSyncDBWaitUI(String StrMsg, ParameterizedThreadStart fun)//DB同步資料顯示等待動畫
        {
            //ThreadWait d = new ThreadWait();
            //Thread t = new Thread(fun);
            //t.Start(d);
            //d.StartPosition = FormStartPosition.CenterParent;
            //d.m_StrInput = StrMsg;
            //d.ShowDialog();
        }

        public static void ShowWaitInfo(String StrMsg, ParameterizedThreadStart fun)//呼叫外部API時，顯示等待UI
        {
            ////ShowInfo d = new ShowInfo(StrMsg);
            //Thread.Sleep(500);
            //Thread t = new Thread(fun);
            //t.Start(d);
            //d.StartPosition = FormStartPosition.CenterParent;
            //d.ShowDialog();
        }



        public static void Thread_Sample(object arg)
        {
            //ThreadWait d = (ThreadWait)arg;
            //Thread.Sleep(500);

            ////---
            ////do something

            ////---do something

            //d.Invoke(new Action(d.Close));
        }


    }
}
