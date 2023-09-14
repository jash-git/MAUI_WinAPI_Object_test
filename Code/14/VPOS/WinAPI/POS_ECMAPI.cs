using System.Data;

using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace VPOS
{
    public class POS_ECMAPI
    {
        public static String m_StrSysPath = Directory.GetCurrentDirectory() + "\\modules\\easycard\\";
        public static EASY_CARDModule m_EASY_CARDModule = new EASY_CARDModule();
        public static EASY_CARDModuleParams m_EASY_CARDModuleParams = new EASY_CARDModuleParams();//DB_payment_module_params參數
        public static TransXML m_EasyCardXML = new TransXML();
        public static EasyCardBlacklist m_EasyCardBlacklist = null;
        public static ECAMGetBasicInfoMsg m_GetBasicInfoMsg = null;

        //---
        //XML操作
        private static void ReadICERINIXml2Var()//讀取目前XML到變數之中 & 刪除舊有XML
        {
            m_EasyCardXML = null;
            String StrFilePath = m_StrSysPath + "ICERINI.xml";
            if (System.IO.File.Exists(StrFilePath))
            {
                //讀取目前XML到變數之中
                String StrXmlContent = File.ReadAllText(StrFilePath);//讀取文字檔
                XmlSerializer serializer = new XmlSerializer(typeof(TransXML));
                using (StringReader reader = new StringReader(StrXmlContent))
                {
                    m_EasyCardXML = (TransXML)serializer.Deserialize(reader);
                }

                File.Delete(StrFilePath);//刪除舊有XML
            }
            else
            {
                m_EasyCardXML = new TransXML();
            }
        }

        public static void CreatICERINIXml(bool blnRead=true)//建立悠遊卡ICERINI.xml檔案
        {
            DataRow[] foundRow;

            m_EASY_CARDModule = null;
            foundRow = SQLDataTableModel.m_payment_module.Select("payment_module_code='EASY_CARD'");
            if (foundRow.Length > 0)
            {
                m_EASY_CARDModule = JsonClassConvert.EASY_CARDModule2Class(foundRow[0]["def_params"].ToString());
            }

            m_EASY_CARDModuleParams = null;
            foundRow = SQLDataTableModel.m_payment_module_params.Select("payment_module_code='EASY_CARD'");
            if (foundRow.Length > 0)//確定EASY_CARDModuleParams資料再DB存在 設定m_EASY_CARDModuleParams
            {
                m_EASY_CARDModuleParams = JsonClassConvert.EASY_CARDModuleParams2Class(foundRow[0]["params"].ToString());
            }

            if(blnRead)
            {
                ReadICERINIXml2Var();//讀取目前XML到變數之中 & 刪除舊有XML
            }
            else
            {
                File.Delete(m_StrSysPath + "ICERINI.xml");//刪除舊有XML
            }

            if (m_EASY_CARDModuleParams != null)
            {
                if(m_EASY_CARDModule!=null)
                {
                    if (m_EASY_CARDModuleParams.env_type == "T")
                    {
                        m_EasyCardXML.ECCIP = m_EASY_CARDModule.server_info.sandbox.ip;
                        m_EasyCardXML.ICERIP = m_EASY_CARDModule.server_info.sandbox.ip;
                        m_EasyCardXML.CMASIP = m_EASY_CARDModule.server_info.sandbox.ip;
                        m_EasyCardXML.ECCPort = m_EASY_CARDModule.server_info.sandbox.port;
                        m_EasyCardXML.ICERPort = m_EASY_CARDModule.server_info.sandbox.port;
                        m_EasyCardXML.CMASPort = m_EASY_CARDModule.server_info.sandbox.port;
                        if (Directory.Exists(m_StrSysPath))
                        {
                            if (m_EasyCardBlacklist != null)
                            {
                                if (!File.Exists(m_StrSysPath + $"ICERData\\BlcFile\\{m_EasyCardBlacklist.data.sandbox.file_name}"))
                                {
                                    /*
                                    //---
                                    //刪除無用檔案
                                    DirectoryInfo di = new DirectoryInfo(m_StrSysPath + "ICERData\\BlcFile\\");// 取得資料夾資訊
                                    foreach (var z in di.GetFiles("*.*"))
                                    {
                                        z.Delete();  // 很好理解，把 z 刪除！
                                    }
                                    //---刪除無用檔案
                                    */
                                    HttpsFun.DownloadFile(m_EasyCardBlacklist.data.sandbox.file_info.location, m_StrSysPath + $"ICERData\\BlcFile\\{m_EasyCardBlacklist.data.sandbox.file_name}");
                                    m_EasyCardXML.BLCName = m_EasyCardBlacklist.data.sandbox.file_name;//指定檔名
                                }
                            }
                        }
                    }
                    else
                    {
                        m_EasyCardXML.ECCIP = m_EASY_CARDModule.server_info.production.ip;
                        m_EasyCardXML.ICERIP = m_EASY_CARDModule.server_info.production.ip;
                        m_EasyCardXML.CMASIP = m_EASY_CARDModule.server_info.production.ip;
                        m_EasyCardXML.ECCPort = m_EASY_CARDModule.server_info.production.port;
                        m_EasyCardXML.ICERPort = m_EASY_CARDModule.server_info.production.port;
                        m_EasyCardXML.CMASPort = m_EASY_CARDModule.server_info.production.port;
                        if (m_EasyCardBlacklist != null)
                        {
                            if (m_EasyCardBlacklist != null)
                            {
                                if (!File.Exists(m_StrSysPath + $"ICERData\\BlcFile\\{m_EasyCardBlacklist.data.production.file_name}"))
                                {
                                    /*
                                    //---
                                    //刪除無用檔案
                                    DirectoryInfo di = new DirectoryInfo(m_StrSysPath + "ICERData\\BlcFile\\");// 取得資料夾資訊
                                    foreach (var z in di.GetFiles("*.*"))
                                    {
                                        z.Delete();  // 很好理解，把 z 刪除！
                                    }
                                    //---刪除無用檔案
                                    */
                                    HttpsFun.DownloadFile(m_EasyCardBlacklist.data.production.file_info.location, m_StrSysPath + $"ICERData\\BlcFile\\{m_EasyCardBlacklist.data.production.file_name}");
                                    m_EasyCardXML.BLCName = m_EasyCardBlacklist.data.production.file_name;//指定檔名
                                }
                            }
                        }
                    }
                }


                if ((m_EASY_CARDModuleParams.cmas_mode != null) && (m_EASY_CARDModuleParams.cmas_mode.Length > 0))
                {
                    m_EasyCardXML.CMASMode = Convert.ToInt32(m_EASY_CARDModuleParams.cmas_mode);
                }

                if ((m_EASY_CARDModuleParams.newspid != null) && (m_EASY_CARDModuleParams.newspid.Length > 0))
                {
                    m_EasyCardXML.NewSPID = m_EASY_CARDModuleParams.newspid;
                }

                if ((m_EASY_CARDModuleParams.tmlocationid != null) && (m_EASY_CARDModuleParams.tmlocationid.Length > 0))
                {
                    m_EasyCardXML.TMLocationID = m_EASY_CARDModuleParams.tmlocationid;
                }
            }

            //---
            //產生新XML ~ https://learn.microsoft.com/zh-tw/dotnet/api/system.xml.xmldeclaration.encoding?view=net-7.0
            if (Directory.Exists(m_StrSysPath))//判斷目錄存在與否
            {
                // Create and load the XML document.
                XmlDocument XmlDoc = new XmlDocument();
                StringWriter sw = new StringWriter();
                XmlSerializer s = new XmlSerializer(m_EasyCardXML.GetType());
                s.Serialize(sw, m_EasyCardXML);
                string xmlString = sw.ToString();
                xmlString = xmlString.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
                xmlString = xmlString.Replace(@"<TransXML xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">", "<TransXML>");
                xmlString = xmlString.Replace(@" xsi:type=""xsd:string""", "");
                XmlDoc.Load(new StringReader(xmlString));

                // Create an XML declaration.
                XmlDeclaration xmldecl;
                xmldecl = XmlDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmldecl.Standalone = "yes";

                // Add the new node to the document.
                XmlElement root = XmlDoc.DocumentElement;
                XmlDoc.InsertBefore(xmldecl, root);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(m_StrSysPath + "ICERINI.xml", settings))
                {
                    XmlDoc.Save(writer);
                }
                //File.WriteAllText(m_StrSysPath + "ICERINI.xml", XmlDoc.OuterXml);
            }
            //---產生新XML
        }
        //---XML操作

        //---
        //模組相關
        public static bool m_blnRunInit = false;//卡機初始化旗標
        public static bool m_blnAPIResult = false;//API呼叫成功與否
        public static int m_intAmount = 0;//操作金額
        public static string m_StrTransSID = "";//操作Trans_SID
        public static String m_StrAPIJsonResult = "";//API呼叫得到JSON
        public static String m_StrAPIResult = "";//API呼叫文字資訊
        public static bool m_blnRetry = false;//重新測試旗標
        public static int m_intRetryCount = 0;//重新測試次數

        public static void InitDevice(object arg)
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            CreatICERINIXml();//建立悠遊卡ICERINI.xml檔案
            m_blnRunInit = false;//停止被執行
            m_blnAPIResult = false;
            try
            {
                if (m_EASY_CARDModule != null)
                {
                    IntPtr pOut;
                    if (Init_Module(out pOut))//模組初始化
                    {
                        m_blnAPIResult = true;
                        m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                        LogFile.Write("POS_ECMAPI.InitDevice[true]:\t" + m_StrAPIJsonResult);
                    }
                    else
                    {
                        m_blnAPIResult = false;
                        m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                        LogFile.Write("POS_ECMAPI.InitDevice[false]:\t" + m_StrAPIJsonResult);

                        EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                        if(EasyCardAPIMsgBuf!=null)
                        {
                            m_StrAPIResult = $"悠遊卡機【設備初始化】失敗！\n{EasyCardAPIMsgBuf.Trans_Msg}";
                        }
                        else
                        {
                            m_StrAPIResult = $"悠遊卡機【設備初始化】失敗！\n";
                        }                        
                    }

                    if(m_blnAPIResult)
                    {
                        if (Card_SignOn(out pOut))//卡機登錄
                        {
                            m_blnAPIResult = true;
                            m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                            LogFile.Write("POS_ECMAPI.Card_SignOn[true]:\t" + m_StrAPIJsonResult);

                            if(Get_Basic_Info(out pOut))
                            {
                                m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                                m_GetBasicInfoMsg = JsonClassConvert.ECAMGetBasicInfoMsg2Class(m_StrAPIJsonResult);
                                LogFile.Write("POS_ECMAPI.Get_Basic_Info[true]:\t" + m_StrAPIJsonResult);
                            }
                            else
                            {
                                m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                                LogFile.Write("POS_ECMAPI.Get_Basic_Info[false]:\t" + m_StrAPIJsonResult);
                            }
                        }
                        else
                        {
                            m_blnAPIResult = false;
                            m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                            LogFile.Write("POS_ECMAPI.Card_SignOn[false]:\t" + m_StrAPIJsonResult);

                            EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                            m_StrAPIResult = $"悠遊卡機【Sign On】失敗！\n{EasyCardAPIMsgBuf.Trans_Msg}";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.InitDevice[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void CloseDevice()//關閉模組
        {
            try
            {
                if (m_EASY_CARDModule != null)
                {
                    Close_Module();
                    m_blnAPIResult = true;
                    LogFile.Write("POS_ECMAPI.CloseDevice");
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.CloseDevice[C# ERROR]" + e.ToString());
            }
        }

        public static void GetCardInfo(object arg)//讀取卡片資訊
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Get_Card_Info(out pOut))//呼叫讀取卡片資訊
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.GetCardInfo[true]:\t" + m_StrAPIJsonResult);

                    ECAM_CardInfo ECAM_CardInfoBuf = JsonClassConvert.ECAM_CardInfo2Class(m_StrAPIJsonResult);
                    m_StrAPIResult = $"此悠遊卡餘額為: {ECAM_CardInfoBuf.Balance_Amount}元";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.GetCardInfo[false]:\t" + m_StrAPIJsonResult);

                    m_StrAPIResult = $"悠遊卡 卡片讀取失敗！\n請將卡片放在感應區上";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.GetCardInfo[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void CashAddValue(object arg)//悠遊卡現金加值
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Cash_Add_Value(m_intAmount, out pOut))//呼叫加值API
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.CashAddValue[true]:\t" + m_StrAPIJsonResult);

                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                    m_StrAPIResult = $"悠遊卡現金加值 {m_intAmount}元 已完成，請移開卡片！\n加值前金額: {EasyCardAPIMsgBuf.Card_Info.Befer_Amount}元\n加值後餘額: {EasyCardAPIMsgBuf.Card_Info.Balance_Amount}元";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.CashAddValue[false]:\t" + m_StrAPIJsonResult);

                    m_StrAPIResult = $"悠遊卡現金加值 {m_intAmount}元 失敗！";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.CashAddValue[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void Deduct(object arg)//悠遊卡扣款
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Deduct(m_intAmount, m_blnRetry, out pOut))//呼叫扣款API
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Deduct[true]:\t" + m_StrAPIJsonResult);

                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                    MainPage.m_StrEasyCardPhysicalID = EasyCardAPIMsgBuf.Card_Info.Physical_ID;//紀錄悠遊卡內碼，結帳電子發票用
                    m_StrAPIResult = $"悠遊卡扣款 {m_intAmount}元 已完成，請移開卡片！\n扣款前金額: {EasyCardAPIMsgBuf.Card_Info.Befer_Amount}元\n扣款後餘額: {EasyCardAPIMsgBuf.Card_Info.Balance_Amount}元";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Deduct[false]:\t" + m_StrAPIJsonResult);

                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                    m_StrAPIResult = $"悠遊卡扣款 {m_intAmount}元 失敗！\n{EasyCardAPIMsgBuf.Trans_Msg}";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.Deduct[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void Refund(object arg)//悠遊卡退貨
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Refund(m_intAmount, m_blnRetry, out pOut))//呼叫退款API
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Refund[true]:\t" + m_StrAPIJsonResult);

                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                    m_StrAPIResult = $"悠遊卡退貨 {m_intAmount}元  已完成，請移開卡片！\n退貨前金額: {EasyCardAPIMsgBuf.Card_Info.Befer_Amount}元\n退貨後餘額: {EasyCardAPIMsgBuf.Card_Info.Balance_Amount}元";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Refund[false]:\t" + m_StrAPIJsonResult);

                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(m_StrAPIJsonResult);
                    m_StrAPIResult = $"悠遊卡退貨 {m_intAmount}元 失敗！\n{EasyCardAPIMsgBuf.Trans_Msg}";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.Refund[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void GetCardBillInfo(object arg)//查詢悠遊卡交易帳單資訊(JSON)
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Get_Card_Bill_Info(m_StrTransSID, out pOut))//呼叫查詢API
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Get_Card_Bill_Info[true]:\t" + m_StrAPIJsonResult);

                    m_StrAPIResult = $"讀取悠遊卡交易帳單資訊成功:\n{m_StrAPIJsonResult}";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Get_Card_Bill_Info[false]:\t" + m_StrAPIJsonResult);

                    m_StrAPIResult = $"讀取悠遊卡交易帳單資訊失敗:\n{m_StrAPIJsonResult}";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.Get_Card_Bill_Info[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }

        public static void Checkout(object arg)//悠遊卡結帳(日結)
        {
            //ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            m_blnAPIResult = false;
            try
            {
                IntPtr pOut;
                if (Checkout(m_blnRetry, out pOut))//呼叫結帳API
                {
                    m_blnAPIResult = true;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Checkoutd[true]:\t" + m_StrAPIJsonResult);
                    m_StrAPIResult = "";
                }
                else
                {
                    m_blnAPIResult = false;
                    m_StrAPIJsonResult = Marshal.PtrToStringAuto(pOut);
                    LogFile.Write("POS_ECMAPI.Checkout[false]:\t" + m_StrAPIJsonResult);
                    m_StrAPIResult = $"悠遊卡結帳失敗！\n{m_StrAPIJsonResult}";
                }
            }
            catch (Exception e)
            {
                m_blnAPIResult = false;
                LogFile.Write("POS_ECMAPI.Checkout[C# ERROR]" + e.ToString());
            }

            //d.Invoke(new Action(d.Close));
        }
        //---模組相關

        //---
        //DLL API
        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Init_Module")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Init_Module(out IntPtr pOut);// 初始化模組

        [DllImport("POS_ECM.dll", EntryPoint = "Close_Module")]
        static extern void Close_Module(); // 關閉模組

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Card_SignOn")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Card_SignOn(out IntPtr pOut); // 認證 悠遊卡機

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Get_Card_Info")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Get_Card_Info(out IntPtr pOut);// 讀取卡片資訊

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Cash_Add_Value")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Cash_Add_Value(int Amount, out IntPtr pOut);// 悠遊卡現金加值

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Deduct")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Deduct(int Amount, bool Retry_Flag, out IntPtr pOut);// 悠遊卡扣款

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Refund")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Refund(int Amount, bool Retry_Flag, out IntPtr pOut);//悠遊卡扣款退貨

        [DllImport("POS_ECM.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Checkut")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Checkout(bool Retry_Flag, out IntPtr pOut);//悠遊卡結帳(日結)

        [DllImport("POS_ECM.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "Get_Card_Bill_Info")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Get_Card_Bill_Info([MarshalAs(UnmanagedType.LPStr)] String Trans_SID, out IntPtr pOut);//讀取悠遊卡交易帳單資訊(JSON)

        [DllImport("POS_ECM.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "Get_Basic_Info")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Get_Basic_Info(out IntPtr pOut);

        //---DLL API

    }//public class POS_ECMAPI

    /*
        //---
        //悠遊卡 Singn On
        if ((POS_ECMAPI.m_blnRunInit) &&(POS_ECMAPI.m_EASY_CARDModule != null))
        {
            WaitUIThread.ShowWaitInfo("悠遊卡機【Sign On】中，請稍後...", POS_ECMAPI.InitDevice);//透過執行序進行等待支付結果確認
            if (!POS_ECMAPI.m_blnAPIResult)
            {
                Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                MsgBuf.ShowDialog();//顯示失敗原因
            }
        }
        //---悠遊卡 Singn On
    
        //---
        //悠遊卡 卡片讀取
        if(POS_ECMAPI.m_EASY_CARDModule != null)
        {
            WaitUIThread.ShowWaitInfo("悠遊卡 卡片讀取中，請稍後...", POS_ECMAPI.GetCardInfo);
            if (POS_ECMAPI.m_blnAPIResult)
            {

                Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                MsgBuf.ShowDialog();//顯示餘額
            }
            else
            {
                Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                MsgBuf.ShowDialog();//顯示失敗原因
            }
        }
        //---悠遊卡 卡片讀取

        //---
        //悠遊卡 卡片加值
        if (POS_ECMAPI.m_EASY_CARDModule != null)
        {
            POS_ECMAPI.m_intAmount = 100;//卡片加值金額
            WaitUIThread.ShowWaitInfo($"悠遊卡 【卡片加值】 {POS_ECMAPI.m_intAmount}元中，請稍後...", POS_ECMAPI.CashAddValue);
            if (POS_ECMAPI.m_blnAPIResult)
            {

                Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                MsgBuf.ShowDialog();
            }
            else
            {
                Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                MsgBuf.ShowDialog();
            }
        }
        //---悠遊卡 卡片加值

        //---
        //悠遊卡 扣款
        if (POS_ECMAPI.m_EASY_CARDModule != null)
        {
            POS_ECMAPI.m_intAmount = 50;//扣款金額
            POS_ECMAPI.m_blnRetry = false;//重新測試旗標
            POS_ECMAPI.m_intRetryCount = 0;//重新測試次數
            do
            {
                WaitUIThread.ShowWaitInfo($"悠遊卡 【扣款】 {POS_ECMAPI.m_intAmount}元中，請稍後...", POS_ECMAPI.Deduct);
                if (POS_ECMAPI.m_blnAPIResult)
                {
                    break;
                }
                else
                {
                    POS_ECMAPI.m_blnRetry = true;
                    POS_ECMAPI.m_intRetryCount++;
                    if ((POS_ECMAPI.m_intRetryCount <= 3) && (POS_ECMAPI.m_StrAPIResult.Contains("Timeout") || POS_ECMAPI.m_StrAPIResult.Contains("T3901=-125")))
                    {
                        QuestionMsg QuestionMsgBuf = new QuestionMsg(POS_ECMAPI.m_StrAPIResult + $"\n是否進行第 {POS_ECMAPI.m_intRetryCount}重試扣款");
                        QuestionMsgBuf.ShowDialog();
                        if (!QuestionMsgBuf.m_blnRun)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            } while (POS_ECMAPI.m_intRetryCount <= 3);

            Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
            MsgBuf.ShowDialog();
        }
        //---悠遊卡 扣款

        //---
        //悠遊卡 退貨
        if (POS_ECMAPI.m_EASY_CARDModule != null)
        {
            POS_ECMAPI.m_intAmount = 40;//退貨金額
            POS_ECMAPI.m_blnRetry = false;//重新測試旗標
            POS_ECMAPI.m_intRetryCount = 0;//重新測試次數
            do
            {
                WaitUIThread.ShowWaitInfo($"悠遊卡 【退貨】 {POS_ECMAPI.m_intAmount}元中，請稍後...", POS_ECMAPI.Refund);
                if (POS_ECMAPI.m_blnAPIResult)
                {
                    break;
                }
                else
                {
                    POS_ECMAPI.m_blnRetry = true;
                    POS_ECMAPI.m_intRetryCount++;
                    if ((POS_ECMAPI.m_intRetryCount <= 3) && (POS_ECMAPI.m_StrAPIResult.Contains("Timeout") || POS_ECMAPI.m_StrAPIResult.Contains("T3901=-125")))
                    {
                        QuestionMsg QuestionMsgBuf = new QuestionMsg(POS_ECMAPI.m_StrAPIResult + $"\n是否進行第 {POS_ECMAPI.m_intRetryCount}重試退貨");
                        QuestionMsgBuf.ShowDialog();
                        if (!QuestionMsgBuf.m_blnRun)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            } while (POS_ECMAPI.m_intRetryCount <= 3);

            Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
            MsgBuf.ShowDialog();
        }
        //---悠遊卡 退貨

        //---
        //讀取悠遊卡交易帳單資訊(JSON)
        POS_ECMAPI.m_StrTransSID = JsonClassConvert.EasyCardAPIMsg2Class(POS_ECMAPI.m_StrAPIJsonResult).SID;
        WaitUIThread.ShowWaitInfo($"悠遊卡 【讀取悠遊卡交易帳單資訊】 交易序號: {POS_ECMAPI.m_StrTransSID}中，請稍後...", POS_ECMAPI.GetCardBillInfo);
        Msg MsgBuf01 = new Msg(POS_ECMAPI.m_StrAPIResult);
        MsgBuf01.ShowDialog();
        //---讀取悠遊卡交易帳單資訊(JSON)
    */
    /*
    //測試程式碼
        try
        {
            IntPtr pOut;
            string msg;
            if (POS_ECMAPI.Init_Module(out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Init_Module_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Init_Module_false:\t" + msg);
            }

            if (POS_ECMAPI.Card_SignOn(out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Card_SignOn_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Card_SignOn_false:\t" + msg);
            }

            if (POS_ECMAPI.Get_Card_Info(out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_false:\t" + msg);
            }

            if (POS_ECMAPI.Cash_Add_Value(100,out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Cash_Add_Value_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Cash_Add_Value_false:\t" + msg);
            }

            if (POS_ECMAPI.Deduct(100,true, out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Deduct_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Deduct_false:\t" + msg);
            }

            if (POS_ECMAPI.Refund(100, true, out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Refund_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Refund_false:\t" + msg);
            }

            if (POS_ECMAPI.Get_Card_Info(out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_true:\t" + msg);
            }

            if (POS_ECMAPI.Get_Card_Info(out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Get_Card_Info_true:\t" + msg);
            }

            if (POS_ECMAPI.Checkut(true,out pOut))
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Checkut_true:\t" + msg);
            }
            else
            {
                msg = Marshal.PtrToStringAuto(pOut);
                Console.WriteLine("Checkut_true:\t" + msg);
            }

            POS_ECMAPI.Close_Module();
            Console.WriteLine("Close_Module");
        }
        catch(Exception e)
        {
            Console.WriteLine("C# ERROR"+e.ToString());
        }
    */
}
