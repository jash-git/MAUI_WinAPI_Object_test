//using Android.App;

using CommunityToolkit.Maui.Views;
using Jint;
using Microsoft.Maui.Controls;
using System.Data;
using System.Data.SQLite;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_WinAPI_Object_test;
public partial class MainPage : ContentPage
{
	int count = 0;
    public IDispatcherTimer timmer { get; set; }
    public Thread thread;
    public bool blnStop = false;
    public MainPage()
	{
		InitializeComponent();

        /*
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

        thread = new Thread(ThreadFun);
        thread.Start();
    }
    private void Timmer_Tick(object sender, EventArgs e)
    {
        labtime.Text = DateTime.Now.ToString("HH:mm:ss");
    }
    private void ThreadFun()
    {
        // 模擬一些耗時的工作
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
    private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

       await DisplayAlert("Alert", $"路徑:{AppDomain.CurrentDomain.BaseDirectory}", "OK");//await

        DataTable dtbuf = GetDataTable("vpos.db", "SELECT * FROM class_data");
        //---
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//載入.net Big5編解碼函數庫(System.Text.Encoding.CodePages)
        StreamReader sr = new StreamReader(@"C:\Users\devel\Desktop\Input.json");
        string StrInput = sr.ReadLine();
        sr.Close();// 關閉串流
        ESCPOS_Receipt_RS232Print(StrInput);
        //---
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new PopupPage());
    }

    public static SerialPort m_port = new SerialPort();
    public static void CommDataReceived(Object sender, SerialDataReceivedEventArgs e)
    {
        //https://www.cnblogs.com/xinaixia/p/6216745.html
        try
        {
            //Comm.BytesToRead中为要读入的字节长度
            int len = m_port.BytesToRead;
            Byte[] readBuffer = new Byte[len];
            m_port.Read(readBuffer, 0, len); //将数据读入缓存
                                             //处理readBuffer中的数据，自定义处理过程
        }
        catch (Exception ex)
        {
        }
    }

    static void ESCPOS_Receipt_RS232Print(String StrInput = "")//收據
    {
        Console.WriteLine("Init Jint...");
        var engine = new Engine();

        engine.Execute(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Script" +
            Path.DirectorySeparatorChar + "CommonFun.js"));
        engine.Execute(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Script" +
            Path.DirectorySeparatorChar + "QrCode_57mm.js"));

        engine.SetValue("input", StrInput);


        Console.WriteLine("Create ESC_Command...");
        String StrFunName = "Main()";
        var Jsonresult = engine.Execute(StrFunName).GetCompletionValue();


        ESCPOS_OrderNew ESCPOSCommand = new ESCPOS_OrderNew();
        ESCPOSCommand = JsonSerializer.Deserialize<ESCPOS_OrderNew>(Jsonresult.AsString());

        Console.WriteLine("C# Modified ESC_Command Start");
        if ((ESCPOSCommand != null) && (ESCPOSCommand.state_code == 0) && (ESCPOSCommand.value != null) && (ESCPOSCommand.value.Count > 0))
        {
            for (int i = 0; i < ESCPOSCommand.value.Count; i++)
            {
                ESCPOSCommand.value[i] = UnescapeUnicode(ESCPOSCommand.value[i]);
            }
        }
        Console.WriteLine("C# Modified ESC_Command End");

        string[] m_comports;//= SerialPort.GetPortNames();
        m_comports = SerialPort.GetPortNames();
        if ((m_comports.Length > 0) && (!m_port.IsOpen))
        {
            m_port.PortName = m_comports[0];
            //m_port.BaudRate = 115200;//RP-700  ;
            m_port.BaudRate = 19200;//PDC325
            m_port.DataBits = 8;
            m_port.StopBits = StopBits.One;
            m_port.Parity = Parity.None;
            m_port.ReadTimeout = 1;
            m_port.ReadTimeout = 3000; //单位毫秒
            m_port.WriteTimeout = 3000; //单位毫秒
                                        //串口控件成员变量，字面意思为接收字节阀值，
                                        //串口对象在收到这样长度的数据之后会触发事件处理函数
                                        //一般都设为1
            m_port.ReceivedBytesThreshold = 1;
            m_port.DataReceived += new SerialDataReceivedEventHandler(CommDataReceived); //设置数据接收事件（监听）
            m_port.Open();

            Console.WriteLine("ESC_Command to Printer Start");
            if ((ESCPOSCommand != null) && (ESCPOSCommand.value != null))
            {
                for (int i = 0; i < ESCPOSCommand.value.Count; i++)
                {
                    //會亂碼  byte[] bytes = Encoding.UTF8.GetBytes(ESCPOSCommand.value[i]);
                    //會亂碼  byte[] bytes = Encoding.Default.GetBytes(ESCPOSCommand.value[i]);
                    //會亂碼  byte[] bytes = Encoding.ASCII.GetBytes(ESCPOSCommand.value[i]);
                    //會亂碼  byte[] bytes = Encoding.Latin1.GetBytes(ESCPOSCommand.value[i]);
                    //byte[] bytes = Encoding.GetEncoding("big5").GetBytes(ESCPOSCommand.value[i]);
                    m_port.Write(Encoding.GetEncoding("big5").GetBytes(ESCPOSCommand.value[i]), 0, Encoding.GetEncoding("big5").GetBytes(ESCPOSCommand.value[i]).Length);
                    //m_port.Write(bytes, 0, bytes.Length);
                }
            }
            //*/
            Console.WriteLine("ESC_Command to Printer End");
        }
        else
        {
            m_port.Close();
        }
    }

    private void OnClosed(object sender, EventArgs e)
    {
        if(timmer!=null)
        {
            timmer.Stop();
        }

        if(thread!=null) 
        {
            blnStop = true;
            thread = null;
        }
        // Close the active window
        //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
        Application.Current.CloseWindow(GetParentWindow());
    }

    public static string UnescapeUnicode(string str)  // 将unicode转义序列(\uxxxx)解码为字符串
    {
        //GOOGLE: C#  \uXXXX
        //https://www.cnblogs.com/netlog/p/15911016.html C#字符串Unicode转义序列编解码
        //https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.unescape?view=net-6.0
        //https://docs.microsoft.com/zh-tw/dotnet/api/system.text.regularexpressions.regex.unescape?view=net-6.0

        return (System.Text.RegularExpressions.Regex.Unescape(str));
    }

    public static SQLiteConnection OpenConn(string Database)//資料庫連線程式
    {
        string cnstr = String.Format("Data Source={0};Version=3;", Database);
        SQLiteConnection icn = new SQLiteConnection();
        icn.ConnectionString = cnstr;
        if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }
        icn.Open();
        return icn;
    }

    public static DataTable GetDataTable(string Database, string SQLiteString)//讀取資料程式
    {
        DataTable myDataTable = new DataTable(Database);
        try
        {

            SQLiteConnection icn = OpenConn(Database);
            SQLiteDataAdapter da = new SQLiteDataAdapter(SQLiteString, icn);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            myDataTable = ds.Tables[0];
            da.Dispose();
            ds.Dispose();
            da = null;
            ds = null;
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

            if (true)
            {
                String StrLog = String.Format("{0}: {1};{2}", "sync_GetDataTable", SQLiteString, "success");
            }
        }
        catch (Exception ex)
        {
            if (true)
            {
                String StrLog = String.Format("{0}: {1};{2}", "sync_GetDataTable", SQLiteString, ex.ToString());
            }
        }

        return myDataTable;
    }
}
public class ESCPOS_OrderNew
{
    public int state_code { get; set; }
    public List<string> value { get; set; }
}
