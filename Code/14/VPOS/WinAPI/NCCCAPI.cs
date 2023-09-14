
using System.Data;
using System.Diagnostics;

namespace VPOS
{
    public class NCCCAPI
    {
        public static bool m_blnRunInit = false;//聯信卡機初始偵測
        public static String m_StrSysPath = Directory.GetCurrentDirectory() + "\\modules\\NCCC\\";
        public static ECR_Config m_ECR_Config = new ECR_Config("");
        public static int m_intTransAmount = 0;//紀錄交易金額;input var
        public static String m_StrApprovalNo = "";//紀錄操作授權碼
        public static String m_StrMsg = "";//輸出API呼叫結果
        public static String m_StrCreditCardJosnRESP = "";//紀錄完整API回傳JSON
        public static bool m_blnResult = false;//紀錄API呼叫結果
        public static void ECRFile2Var()//讀取目前ECR.DAT到變數之中 & 刪除舊有ECR.DAT
        {
            String StrFilePath = m_StrSysPath + "ECR.DAT";
            if (System.IO.File.Exists(StrFilePath))
            {
                m_ECR_Config = null;
                m_ECR_Config = new ECR_Config("");
                StreamReader sr = new StreamReader(StrFilePath);
                int intCount = 0;
                while (!sr.EndOfStream)// 每次讀取一行，直到檔尾
                {
                    string line = sr.ReadLine().Trim();// 讀取文字到 line 變數
                    switch(intCount)
                    {
                        case 0:
                            m_ECR_Config.Comport = line;
                            break;
                        case 1:
                            m_ECR_Config.TimeOut = line;
                            
                            break;
                        case 2:
                            m_ECR_Config.BaudRate = line;
                            break;
                        case 3:
                            m_ECR_Config.Format = line;
                            break;
                        case 4:
                            m_ECR_Config.RetriesCount = line;
                            break;
                    }
                    intCount++;
                }
                sr.Close();// 關閉串流

            }
            else
            {
                m_ECR_Config = new ECR_Config("");
            }
        }

        private static bool CallNCCC()//呼叫NCCC.EXE
        {
            bool blnResult = false;
            try
            {
                //---
                //呼叫NCCC.exe
                String StrFilePath = m_StrSysPath + "NCCC.exe";
                ProcessStartInfo start = new ProcessStartInfo();
                start.WorkingDirectory = m_StrSysPath;
                start.FileName = StrFilePath;
                start.CreateNoWindow = true;//不要有UI閃爍[CMD不要建立UI]
                start.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(start);
                if (proc != null)
                {
                    proc.WaitForExit();
                    blnResult = true;
                }
                //---呼叫NCCC.exe
            }
            catch (Exception e)
            {
                LogFile.Write("NCCCAPI.CallNCCC[C# ERROR]" + e.ToString());
            }

            return blnResult;
        }

        public static bool CreateECRFile()//建立連線設定檔ECR.DAT
        {
            bool blnResult = false;
            try
            {
                if ((m_ECR_Config != null) && (m_ECR_Config.Comport.Length > 0))
                {
                    //---
                    //建立對應REQ.json
                    String StrFilePath = m_StrSysPath + "REQ.json";
                    File.Delete(StrFilePath);//刪除舊有REQ.json

                    CreditCardJosn CreditCardJosnBuf = new CreditCardJosn();
                    CreditCardJosnBuf.Trans_Type = "00";
                    CreditCardJosnBuf.ECR_Config.Comport = m_ECR_Config.Comport;
                    CreditCardJosnBuf.ECR_Config.BaudRate = m_ECR_Config.BaudRate;
                    CreditCardJosnBuf.ECR_Config.TimeOut = m_ECR_Config.TimeOut;
                    CreditCardJosnBuf.ECR_Config.Format = m_ECR_Config.Format;
                    CreditCardJosnBuf.ECR_Config.RetriesCount = m_ECR_Config.RetriesCount;

                    String StrData = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);
                    StreamWriter sw = new StreamWriter(StrFilePath);
                    sw.Write(StrData);// 寫入文字
                    sw.Close();// 關閉串流
                    //---建立對應REQ.json

                    blnResult = CallNCCC();
                }
            }
            catch (Exception e)
            {
                LogFile.Write("NCCCAPI.CreateECRFile[C# ERROR]" + e.ToString());
            }
            return blnResult;
        }

        public static void DeviceDetect(object arg)//設備偵測
        {
            ////ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_StrCreditCardJosnRESP = "";
            m_StrMsg = "";
            m_blnResult = false;
            try
            {
                //---
                //建立對應REQ.json
                String StrFilePath = m_StrSysPath + "REQ.json";
                File.Delete(StrFilePath);//刪除舊有REQ.json

                CreditCardJosn CreditCardJosnBuf = new CreditCardJosn();
                CreditCardJosnBuf.Trans_Type = "98";
                CreditCardJosnBuf.ECR_Indicator = "E";

                String StrData = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);
                StreamWriter sw = new StreamWriter(StrFilePath);
                sw.Write(StrData);// 寫入文字
                sw.Close();// 關閉串流
                //---建立對應REQ.json

                m_blnResult = CallNCCC();

                //---
                //讀取RESP.json
                if (m_blnResult)
                {
                    StrFilePath = m_StrSysPath + "RESP.json";
                    if(File.Exists(StrFilePath))
                    {
                        StreamReader sr = new StreamReader(StrFilePath);
                        m_StrCreditCardJosnRESP = sr.ReadToEnd();
                        sr.Close();
                        CreditCardJosn CreditCardJosnRESP = JsonClassConvert.CreditCardJosn2Class(m_StrCreditCardJosnRESP);

                        m_StrMsg = CreditCardJosnRESP.ECR_Response_Msg;
                        m_blnResult = (CreditCardJosnRESP.ECR_Response_Code=="0000")?true:false;
                    }
                    else
                    {
                        m_StrMsg = "NCCC.exe執行失敗";
                    }
                }
                else
                {
                    m_StrMsg = "呼叫NCCC.exe失敗";
                }
                //---讀取RESP.json
            }
            catch (Exception e) 
            {
                m_StrMsg = "NCCCAPI.DeviceDetect[C# ERROR]" + e.ToString();
                LogFile.Write(m_StrMsg);
                
            }


            //d.Invoke(new Action(d.Close));
        }

        public static bool Payments(int intAmount, ref String StrResult)//信用卡 支付
        {
            m_intTransAmount = intAmount;//扣款金額
            WaitUIThread.ShowWaitInfo($"信用卡 【扣款】 {m_intTransAmount}元中，\n請將卡片放在感應區上並稍後片刻...", Deduct);
            
            CreditCardJosn CreditCardJosnBuf = JsonClassConvert.CreditCardJosn2Class(m_StrCreditCardJosnRESP);
            CreditCardJosnBuf.ECR_Config = null;
            CreditCardJosnBuf.Device_Titel = "聯合信用卡小額交易持卡人存根";

            String StrMsg = "";
            if (m_blnResult)
            {
                StrMsg = "聯信刷卡機 ~ " + m_StrMsg;
            }
            else
            {
                StrMsg = "聯信刷卡機交易失敗 ~ " + m_StrMsg;
            }

            if((m_intTransAmount<=1000) && m_blnResult)
            {
                StrMsg += "\n請問顧客需要額外列印【聯合信用卡小額交易持可人存根】嗎?";
                //QuestionMsg QMsg = new QuestionMsg(StrMsg);
                //QMsg.ShowDialog();
                //CreditCardJosnBuf.Print_Receipt = (QMsg.m_blnRun) ? "Y" : "N";
            }
            else
            {
                //Msg MsgBuf = new Msg(StrMsg);
                //MsgBuf.ShowDialog();
            }

            StrResult = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);

            return m_blnResult;
        }

        private static void Deduct(object arg)//付款
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_StrCreditCardJosnRESP = "";
            m_StrMsg = "";
            m_blnResult = false;
            try
            {
                //---
                //建立對應REQ.json
                String StrFilePath = m_StrSysPath + "REQ.json";
                File.Delete(StrFilePath);//刪除舊有REQ.json

                CreditCardJosn CreditCardJosnBuf = new CreditCardJosn();
                CreditCardJosnBuf.ECR_Indicator = "E";//電子發票載具檢查 由收單行檢查
                CreditCardJosnBuf.Trans_Type = "01";
                CreditCardJosnBuf.CUP_SP_ESVC_Indicator = "N";//卡片指定 ~ N:信用卡/C:CUP卡/S:Smart Pay/E:電子票證(一卡通,悠遊卡...)
                CreditCardJosnBuf.Trans_Amount = $"{m_intTransAmount}";

                String StrData = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);
                StreamWriter sw = new StreamWriter(StrFilePath);
                sw.Write(StrData);// 寫入文字
                sw.Close();// 關閉串流
                //---建立對應REQ.json

                m_blnResult = CallNCCC();

                //---
                //讀取RESP.json
                if (m_blnResult)
                {
                    StrFilePath = m_StrSysPath + "RESP.json";
                    if (File.Exists(StrFilePath))
                    {
                        StreamReader sr = new StreamReader(StrFilePath);
                        m_StrCreditCardJosnRESP = sr.ReadToEnd();
                        sr.Close();
                        CreditCardJosn CreditCardJosnRESP = JsonClassConvert.CreditCardJosn2Class(m_StrCreditCardJosnRESP);

                        m_StrMsg = CreditCardJosnRESP.ECR_Response_Msg;
                        m_blnResult = (CreditCardJosnRESP.ECR_Response_Code == "0000") ? true : false;
                    }
                    else
                    {
                        m_StrMsg = "NCCC.exe執行失敗";
                    }
                }
                else
                {
                    m_StrMsg = "呼叫NCCC.exe失敗";
                }
                //---讀取RESP.json
            }
            catch (Exception e)
            {
                m_StrMsg = "NCCCAPI.Payment[C# ERROR]" + e.ToString();
                LogFile.Write(m_StrMsg);

            }

            //d.Invoke(new Action(d.Close));
        }

        public static bool Refund(int intAmount,String StrApprovalNo, ref String StrResult)
        {
            m_intTransAmount = intAmount;//退款金額
            m_StrApprovalNo = StrApprovalNo;
            WaitUIThread.ShowWaitInfo($"信用卡 【退貨】 {m_intTransAmount}元中，\n請將卡片放在感應區上並稍後片刻...", Refund);
            StrResult = m_StrCreditCardJosnRESP;
            String StrMsg = "";
            if (m_blnResult)
            {
                StrMsg = "聯信刷卡機 ~ " + m_StrMsg;
            }
            else
            {
                StrMsg = "聯信刷卡機交易失敗 ~ " + m_StrMsg;
            }
            //Msg MsgBuf = new Msg(StrMsg);
            //MsgBuf.ShowDialog();

            return m_blnResult;
        }
        public static void Refund(object arg)//退貨
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_StrCreditCardJosnRESP = "";
            m_StrMsg = "";
            m_blnResult = false;
            try
            {
                //---
                //建立對應REQ.json
                String StrFilePath = m_StrSysPath + "REQ.json";
                File.Delete(StrFilePath);//刪除舊有REQ.json

                CreditCardJosn CreditCardJosnBuf = new CreditCardJosn();
                CreditCardJosnBuf.ECR_Indicator = "E";
                CreditCardJosnBuf.Trans_Type = "02";
                CreditCardJosnBuf.CUP_SP_ESVC_Indicator = "N";
                CreditCardJosnBuf.Trans_Amount = $"{m_intTransAmount}";
                CreditCardJosnBuf.Approval_No = m_StrApprovalNo;

                String StrData = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);
                StreamWriter sw = new StreamWriter(StrFilePath);
                sw.Write(StrData);// 寫入文字
                sw.Close();// 關閉串流
                //---建立對應REQ.json

                m_blnResult = CallNCCC();

                //---
                //讀取RESP.json
                if (m_blnResult)
                {
                    StrFilePath = m_StrSysPath + "RESP.json";
                    if (File.Exists(StrFilePath))
                    {
                        StreamReader sr = new StreamReader(StrFilePath);
                        m_StrCreditCardJosnRESP = sr.ReadToEnd();
                        sr.Close();
                        CreditCardJosn CreditCardJosnRESP = JsonClassConvert.CreditCardJosn2Class(m_StrCreditCardJosnRESP);

                        m_StrMsg = CreditCardJosnRESP.ECR_Response_Msg;
                        m_blnResult = (CreditCardJosnRESP.ECR_Response_Code == "0000") ? true : false;
                    }
                    else
                    {
                        m_StrMsg = "NCCC.exe執行失敗";
                    }
                }
                else
                {
                    m_StrMsg = "呼叫NCCC.exe失敗";
                }
                //---讀取RESP.json
            }
            catch (Exception e)
            {
                m_StrMsg = "NCCCAPI.Payment[C# ERROR]" + e.ToString();
                LogFile.Write(m_StrMsg);

            }

            //d.Invoke(new Action(d.Close));
        }

        public static bool Checkout(ref String StrResult)//關帳
        {
            if ((SQLDataTableModel.m_payment_module_params != null) && (SQLDataTableModel.m_payment_module_params.Rows.Count > 0))
            {
                DataRow[] foundRow;
                foundRow = SQLDataTableModel.m_payment_module_params.Select("payment_module_code='NCCC_CREDIT_CARD'");//偵測聯信刷卡模組是否起啟用
                if ((foundRow != null) && (foundRow.Length > 0))
                {
                    WaitUIThread.ShowWaitInfo($"聯合信用卡卡機 【關帳】中，請稍後...", Checkout);
                    StrResult = m_StrCreditCardJosnRESP;

                    if(!m_blnResult)
                    {
                        //Msg MsgBuf = new Msg(m_StrMsg);
                        //MsgBuf.ShowDialog();
                    }
                    
                    return m_blnResult;
                }//if ((foundRow != null) && (foundRow.Length > 0))
            }//if ((SQLDataTableModel.m_payment_module_params != null) && (SQLDataTableModel.m_payment_module_params.Rows.Count > 0))

            return false;
        }

        public static void Checkout(object arg)//結帳
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_StrCreditCardJosnRESP = "";
            m_StrMsg = "";
            m_blnResult = false;
            try
            {
                //---
                //建立對應REQ.json
                String StrFilePath = m_StrSysPath + "REQ.json";
                File.Delete(StrFilePath);//刪除舊有REQ.json

                CreditCardJosn CreditCardJosnBuf = new CreditCardJosn();
                CreditCardJosnBuf.ECR_Indicator = "E";
                CreditCardJosnBuf.Trans_Type = "50";

                String StrData = JsonClassConvert.CreditCardJosn2String(CreditCardJosnBuf);
                StreamWriter sw = new StreamWriter(StrFilePath);
                sw.Write(StrData);// 寫入文字
                sw.Close();// 關閉串流
                //---建立對應REQ.json

                m_blnResult = CallNCCC();

                //---
                //讀取RESP.json
                if (m_blnResult)
                {
                    StrFilePath = m_StrSysPath + "RESP.json";
                    if (File.Exists(StrFilePath))
                    {
                        StreamReader sr = new StreamReader(StrFilePath);
                        m_StrCreditCardJosnRESP = sr.ReadToEnd();
                        sr.Close();
                        CreditCardJosn CreditCardJosnRESP = JsonClassConvert.CreditCardJosn2Class(m_StrCreditCardJosnRESP);

                        m_StrMsg = CreditCardJosnRESP.ECR_Response_Msg;
                        m_blnResult = (CreditCardJosnRESP.ECR_Response_Code == "0000") ? true : false;
                    }
                    else
                    {
                        m_StrMsg = "NCCC.exe執行失敗";
                    }
                }
                else
                {
                    m_StrMsg = "呼叫NCCC.exe失敗";
                }
                //---讀取RESP.json
            }
            catch (Exception e)
            {
                m_StrMsg = "NCCCAPI.Checkout[C# ERROR]" + e.ToString();
                LogFile.Write(m_StrMsg);

            }

            //d.Invoke(new Action(d.Close));
        }
    }//public class NCCCAPI
}
