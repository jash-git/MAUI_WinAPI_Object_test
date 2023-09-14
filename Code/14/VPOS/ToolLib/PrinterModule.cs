using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class PrinterModule
    {
        private static bool TelnetPort(String StrIP,int intPort,int intTimeOut=5000)
        {
            //https://gist.github.com/relyky/1ea60499e1ecd107b138b5541908bf76
            bool blnResult = false;
            String StrLog = "";

            try
            {
                TcpClient tcpClient = new TcpClient();

                Task aTask = tcpClient.ConnectAsync(StrIP, intPort); // 非同步連線
                
                aTask.Wait(intTimeOut); // 連線時間只等五秒

                if (tcpClient.Connected)
                {
                    // show result
                    StrLog = (String.Format("{0}:{1} Connect OK", StrIP, intPort) + Environment.NewLine);

                    // 模擬 telnet，連上立即取回應值
                    NetworkStream stream = tcpClient.GetStream();
                    if (stream.CanRead)
                    {
                        stream.ReadTimeout = 1500; // 有回應就有回應，沒有只等1.5秒。
                        Byte[] data = new Byte[256];
                        Int32 dataLength = stream.Read(data, 0, data.Length);
                        String responseMsg = System.Text.Encoding.ASCII.GetString(data, 0, dataLength);
                        StrLog += (String.Format("[GET MSG] {0}", responseMsg) + Environment.NewLine);

                    }

                    // clsoe connnection
                    tcpClient.Close();
                    blnResult = true;
                }
                else
                {
                    // show result
                    StrLog = (String.Format("{0}:{1} Connect Fail", StrIP, intPort) + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                StrLog = ("Error: " + ex.Message + Environment.NewLine);
            }

            LogFile.Write("TelnetPort ; " + StrLog);
            return blnResult;
        }

        private static void CmdPing(String StrIP)
        {
            string[] StrCmd = { "ping", $"{StrIP} -n 5" };
            Process proc = new Process();
            proc.StartInfo.FileName = StrCmd[0];
            proc.StartInfo.Arguments = StrCmd[1];
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();

            string stdout = "";
            using (StreamReader reader = proc.StandardOutput)
            {
                stdout += reader.ReadToEnd();
            }

            proc.WaitForExit();

            LogFile.Write("CmdPing ; " + stdout);
        }

        private static bool PingIP(String StrIP)
        {
            bool blnResult = false;
            //---
            //C# 原生PING元件
            //https://blog.csdn.net/Andrew_wx/article/details/6628501
            //构造Ping实例
            Ping pingSender = new Ping();
            //Ping 选项设置
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据
            string data01 = "test data abcabc";
            byte[] buffer = Encoding.ASCII.GetBytes(data01);
            //设置超时时间
            int timeout = 120;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
            PingReply reply = pingSender.Send(StrIP, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                String StrLog = String.Format("答復的主機地址：" + reply.Address.ToString());
                StrLog += "\n"+String.Format("往返時間：" + reply.RoundtripTime);
                StrLog += "\n" + String.Format("生存時間（TTL）：" + reply.Options.Ttl);
                StrLog += "\n" + String.Format("是否控制數據包的分段：" + reply.Options.DontFragment);
                StrLog += "\n" + String.Format("緩沖區大小：" + reply.Buffer.Length);
                LogFile.Write("PingIP ; " + StrLog);
            }
            else
            {
                String StrLog = String.Format("{0}", reply.Status.ToString());
                LogFile.Write("PingIP ; " + StrLog);
            }
            return blnResult;
        }
        private static void CommDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            //https://www.cnblogs.com/xinaixia/p/6216745.html
            try
            {
                SerialPort R232 = (SerialPort)(sender);
                //Comm.BytesToRead中为要读入的字节长度
                int len = R232.BytesToRead;
                Byte[] readBuffer = new Byte[len];
                R232.Read(readBuffer, 0, len); //将数据读入缓存
                //处理readBuffer中的数据，自定义处理过程
            }
            catch (Exception ex)
            {
                String StrLog = String.Format("RS232 Error:{0}", ex.ToString());
                LogFile.Write("PrinterModule ; " + StrLog);
            }
        }

        public static bool RS232Mode(String StrPortName, int intBaudRate, PT_CommandOutput PT_CommandOutputs)
        {
            bool blnResult = false;

            string[] m_comports;//= SerialPort.GetPortNames();
            m_comports = SerialPort.GetPortNames();
            SerialPort R232 = new SerialPort();
            try
            {
                if (m_comports.Length > 0)
                {
                    R232.PortName = StrPortName;//m_comports[0];
                    R232.BaudRate = intBaudRate;
                    R232.DataBits = 8;
                    R232.StopBits = StopBits.One;
                    R232.Parity = Parity.None;

                    R232.ReadTimeout = 1;
                    R232.ReadTimeout = 3000; //单位毫秒
                    R232.WriteTimeout = 3000;//单位毫秒
                                             //串口控件成员变量，字面意思为接收字节阀值，
                                             //串口对象在收到这样长度的数据之后会触发事件处理函数
                                             //一般都设为1
                    R232.ReceivedBytesThreshold = 1;
                    R232.DataReceived += new SerialDataReceivedEventHandler(CommDataReceived); //设置数据接收事件（监听）
                    R232.Open();

                    if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0) && (PT_CommandOutputs.value != null))
                    {
                        for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                        {
                            R232.Write(Encoding.GetEncoding("big5").GetBytes(PT_CommandOutputs.value[i]), 0, Encoding.GetEncoding("big5").GetBytes(PT_CommandOutputs.value[i]).Length);
                        }
                    }

                    R232.Close();

                    String StrLog = $"RS232Mode Finish ~ {StrPortName}:{intBaudRate}";
                    LogFile.Write("PrinterModule ; " + StrLog);
                    blnResult = true;
                }
                else
                {
                    String StrLog = $"Not found RS232 ~ {StrPortName}:{intBaudRate}";
                    LogFile.Write("PrinterModule ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                String StrLog = String.Format("RS232 Error:{0}", ex.ToString());
                LogFile.Write("PrinterModule ; " + StrLog);
            }

            return blnResult;
        }

        public static bool DriveMode(String StrPrinterName, PT_CommandOutput PT_CommandOutputs,int intPrinterMode=0)
        {
            bool blnResult = false;
            String StrLog = "";

            try
            {
                switch (intPrinterMode)
                {
                    case 0://一般印表機
                        Int32 dwError = 0, dwWritten = 0;
                        IntPtr hPrinter = new IntPtr(0);

                        DOCINFOA di = new DOCINFOA();
                        di.pDocName = "My C#.NET RAW Document";
                        di.pDataType = "RAW";

                        if (PrinterAPI.OpenPrinter(StrPrinterName, out hPrinter, IntPtr.Zero))
                        {
                            // 啟動文檔列印                    
                            if (PrinterAPI.StartDocPrinter(hPrinter, 1, di))
                            {
                                // 開始列印                        
                                if (PrinterAPI.StartPagePrinter(hPrinter))
                                {
                                    if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0) && (PT_CommandOutputs.value != null))
                                    {
                                        for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                                        {
                                            byte[] bytes = Encoding.GetEncoding("big5").GetBytes(PT_CommandOutputs.value[i]);

                                            Int32 dwCount = bytes.Length;
                                            // 非託管指針              
                                            IntPtr pBytes = Marshal.AllocHGlobal(dwCount);
                                            // 將託管位元組陣列複製到非託管記憶體指標          
                                            Marshal.Copy(bytes, 0, pBytes, dwCount);

                                            // 向印表機輸出位元組  
                                            blnResult = PrinterAPI.WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                                        }
                                    }

                                    PrinterAPI.EndPagePrinter(hPrinter);
                                }

                                PrinterAPI.EndDocPrinter(hPrinter);
                            }

                            PrinterAPI.ClosePrinter(hPrinter);

                            StrLog = $"DriveMode:{StrPrinterName};Mode=[0] Finish";
                            LogFile.Write("PrinterModule ; " + StrLog);
                            blnResult = true;
                        }

                        if (blnResult == false)
                        {
                            dwError = Marshal.GetLastWin32Error();
                        }
                        break;

                    case 1://TSC_標籤機
                        int intResult;
                        intResult = TSCLIB_API.openport(StrPrinterName);//找打打印机端口
                        if ((intResult>0) && (PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0) && (PT_CommandOutputs.value != null))
                        {
                            for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                            {
                                string StrBuf = PT_CommandOutputs.value[i];
                                intResult = TSCLIB_API.sendcommand(StrBuf);
                            }

                            intResult = TSCLIB_API.closeport();
                        }
                        

                        StrLog = $"DriveMode:{StrPrinterName};Mode=[1] Finish";
                        LogFile.Write("PrinterModule ; " + StrLog);
                        blnResult = true;
                        break;
                }
                // 打開印表機                

            }
            catch (Exception ex)
            {
                StrLog = String.Format("Drive Error:{0}", ex.ToString());
                LogFile.Write("PrinterModule ; " + StrLog);
            }

            return blnResult;
        }

        public static bool TcpMode(String StrIP,int intPort, PT_CommandOutput PT_CommandOutputs)
        {
            bool blnResult = false;
            TcpClient tcpSocket = new TcpClient();
            tcpSocket.ReceiveBufferSize = 1024;//1k
            tcpSocket.SendBufferSize = 1024;//1k
            int intTcpRetryCount = 0;

            try
            {
                do
                {
                    //tcpSocket.Connect("192.168.1.54", 9100);
                    //TelnetPort(StrIP, 23);//23 PORT 一定不會通
                    if (!tcpSocket.ConnectAsync(StrIP, intPort).Wait(1000))//if (!tcpSocket.Connected)
                    {
                        tcpSocket = null;
                        tcpSocket = new TcpClient();
                        intTcpRetryCount++;
                        String StrLog = String.Format("TCP RetryCount={0}", intTcpRetryCount);
                        LogFile.Write("PrinterModule ; " + StrLog);
                        CmdPing(StrIP);//PingIP(StrIP);//藉此喚醒設備
                        //TelnetPort(StrIP, 23);//23 PORT 一定不會通
                    }
                    else
                    {
                        break;
                    }
                } while ((!tcpSocket.Connected) && (intTcpRetryCount < 2));

                if (tcpSocket.Connected)
                {
                    if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code==0) && (PT_CommandOutputs.value != null))
                    {
                        for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                        {
                            byte[] bytes = Encoding.GetEncoding("big5").GetBytes(PT_CommandOutputs.value[i]);
                            NetworkStream tcpStream = tcpSocket.GetStream();
                            tcpStream.Write(bytes, 0, bytes.Length);
                            tcpStream.Flush();
                        }
                    }
                    tcpSocket.GetStream().Close();
                    tcpSocket.Close();

                    String StrLog = $"TcpMode Finish~ {StrIP}:{intPort}";
                    LogFile.Write("PrinterModule ; " + StrLog);
                    blnResult = true;
                }
                else
                {
                    String StrLog = $"TCP Connect Error~ {StrIP}:{intPort}";
                    LogFile.Write("PrinterModule ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                String StrLog = String.Format("TCP Error:{0}", ex.ToString());
                LogFile.Write("PrinterModule ; " + StrLog);
            }

            return blnResult;
        }
    }//PrinterModule
}
