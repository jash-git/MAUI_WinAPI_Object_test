using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace VPOS
{

    public class POS_INVAPI
    {
        //params  Var
        public static inv_params m_InvParams;//電子發票參數資訊
        public static Inv_Use_Info m_InvUseInfo;//發票資訊
        public static inv_result m_InvResult;
        public static bool m_blnAutoCheck = false;
        public static string m_strShowMsg = "";
        //params API
        public static void InitInvParamsVar()//初始化電子發票參數資訊變數函數
        {
            m_blnAutoCheck = (HttpsFun.m_intNetworkLevel > 0)? true : false ;//是否自動偵測
            m_InvParams = new inv_params();
            DataTable basic_paramsDataTable = new DataTable();
            String SQL = "SELECT param_value FROM basic_params WHERE param_key='INVOICE_PXU4T' LIMIT 0,1";
            basic_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);
            if ((basic_paramsDataTable != null) && (basic_paramsDataTable.Rows.Count > 0))
            {
                m_InvParams.Platform_Code = "PXU4T";
                m_InvParams.Platform_Name = "美麗科技電子發票";
                m_InvParams.Platform_Params = basic_paramsDataTable.Rows[0][0].ToString();
                m_InvParams.Sandbox = (SqliteDataAccess.m_company_invoice_params[0].env_type=="T") ? "Y" : "N";
                m_InvParams.Client_Id = SqliteDataAccess.m_terminal_data[0].client_id;//terminal_data中的[client_id]欄位
                m_InvParams.Client_Secret = SqliteDataAccess.m_terminal_data[0].client_secret;//terminal_data中的[client_secret]欄位

                m_InvParams.Business_Id = SqliteDataAccess.m_company[0].EIN;
                m_InvParams.Branch_No = SqliteDataAccess.m_company_invoice_params[0].branch_no;
                m_InvParams.Reg_Id = SqliteDataAccess.m_company_invoice_params[0].reg_id;
                m_InvParams.Pos_Id = SqliteDataAccess.m_terminal_data[0].pid;
                m_InvParams.Pos_No = SqliteDataAccess.m_terminal_data[0].pos_no;
                m_InvParams.QRCode_AES_Key = SqliteDataAccess.m_company_invoice_params[0].qrcode_aes_key;
                m_InvParams.Operator_Name = MainPage.m_StrUserAccount;
                m_InvParams.Operator_Id = MainPage.m_StrUserSID;
                m_InvParams.Inv_Renew_Count = SqliteDataAccess.m_company_invoice_params[0].inv_renew_count;
                m_InvParams.Booklet = SqliteDataAccess.m_company_invoice_params[0].booklet;
                m_InvParams.Terminal_SID = SqliteDataAccess.m_terminal_data[0].SID;
                m_InvParams.Last_Batch_Num = Convert.ToInt32(SqliteDataAccess.m_terminal_data[0].invoice_batch_num);//每次取號都要修改
            }
            
        }

        public static void InitUI()
        {
            if ((!m_blnCheck) && SqliteDataAccess.m_terminal_data[0].invoice_active_state == "Y")//確定有啟用電子發票
            {
                WaitUIThread.ShowWaitInfo("發票狀態初始化中，請稍後...", Check);//透過執行序進行等待支付結果確認
                if(!m_blnCheckResult)
                {
                    m_blnCheck = true;
                }
            }       
        }

        public static bool m_blnCheck = false;
        public static bool m_blnCheckResult = true;
        public static String m_StrCheckResult = "";
        public static void Check(object arg)
        {
            //ShowInfo d = (ShowInfo)arg; ;
            m_blnCheck = false;

            POS_INVAPI.InitInvParamsVar();//初始化電子發票參數資訊變數函數

            int count = 0;
            do
            {
                count += 1;
                if (!POS_INVAPI.Get_Invoice_Current_Info())//確認目前電子發票狀態
                {
                    if (POS_INVAPI.Check_Renew_Invoice_Data())//電子發票狀態檢查和狀態紀錄[初始化電子發票]
                    {
                        continue;//初始化成功，在測試一次
                    }
                    else
                    {
                        m_blnCheckResult = false;
                        m_StrCheckResult = POS_INVAPI.m_StrAPIResult;
                        break;
                    }
                }
                else
                {
                    m_blnCheck = true;
                    break;//確認電子發可用 跳離迴圈
                }

            } while (count < 2);//最多確認兩次


            //d.Invoke(new Action(d.Close));
        }

        //---
        //模組相關
        public static bool m_blnAPIResult = false;//API呼叫成功與否
        public static String m_StrAPIResult = "";//API呼叫得到JSON
        public static bool Check_Renew_Invoice_Data()//電子發票狀態檢查和狀態紀錄[初始化電子發票]
        {
            m_blnAPIResult = false;//API呼叫成功與否
            try
            {
                if ((HttpsFun.m_intNetworkLevel > 0))
                {
                    string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);

                    IntPtr sMsg;
                    m_blnAPIResult = Check_Renew_Invoice_Data(StrInvParams, out sMsg);
                    m_StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                    m_InvResult = JsonClassConvert.inv_result2Class(m_StrAPIResult);
                    m_strShowMsg = $"回傳代碼:{m_InvResult.Code}\t回傳資訊:{m_InvResult.Msg}\n";

                    LogFile.Write($"POS_INVAPI.Check_Renew_Invoice_Data[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{m_StrAPIResult}");
                }
                else
                {
                    LogFile.Write("POS_INVAPI.Check_Renew_Invoice_Data[false]: network error");
                }
            }
            catch (Exception ex)
            {
                LogFile.Write("POS_INVAPI.Check_Renew_Invoice_Data[false] ; " + ex.ToString());
            }


            return m_blnAPIResult;
        }

        public static bool Get_Invoice_Current_Info()//確認目前電子發票狀態
        {
            m_blnAPIResult = false;//API呼叫成功與否

            //if (true)
            if ((HttpsFun.m_intNetworkLevel > 0))
            {
                string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);

                IntPtr sMsg;
                m_blnAPIResult = Get_Invoice_Current_Info(StrInvParams, out sMsg);
                m_StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                m_InvUseInfo = JsonClassConvert.Inv_Use_Info2Class(m_StrAPIResult);

                LogFile.Write($"POS_INVAPI.Get_Invoice_Current_Info[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{m_StrAPIResult}");
            }
            else
            {
                LogFile.Write("POS_INVAPI.Get_Invoice_Current_Info[false]: network error");
            }

            return m_blnAPIResult;
        }

        public static bool Get_New_InvoiceNo(ref string StrInvoiceInfo, ref string StrOrderInvoiceData)//取得一個新的可用發票號碼
        {
            m_blnAPIResult = false;//API呼叫成功與否
            InitInvParamsVar();
            if (true)//if ((HttpsFun.m_intNetworkLevel > 0))
            {
                string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);

                bool blncheck = false;
                do
                {
                    IntPtr sMsg;
                    m_blnAPIResult = Get_New_InvoiceNo(StrInvParams, out sMsg);
                    m_StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                    m_InvUseInfo = JsonClassConvert.Inv_Use_Info2Class(m_StrAPIResult);
                        
                    if(m_InvUseInfo!=null)
                    {
                        String SQL = "";
                        SQL = String.Format($"UPDATE terminal_data SET invoice_batch_num = '{m_InvUseInfo.Batch_Num}'");
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        SqliteDataAccess.m_terminal_data = SqliteDataAccess.terminal_dataLoad();//設備參數資料

                        DataTable order_invoice_dataDataTable = new DataTable();
                        SQL=$"SELECT order_no FROM order_invoice_data WHERE inv_no='{m_InvUseInfo.Last_Use_No.ToString()}'";
                        order_invoice_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
                        if(order_invoice_dataDataTable!=null && order_invoice_dataDataTable.Rows.Count==0)
                        {
                            blncheck = true;//確定發票號碼沒被使用過
                        }
                    }
                    else
                    {
                        blncheck = false;//API執行錯誤
                        break;
                    }
                } while (!blncheck);

                m_blnAPIResult = blncheck;
                StrInvoiceInfo = StrInvParams;//記錄在order_data內的invoice_infor
                StrOrderInvoiceData = (blncheck) ? $"{m_InvUseInfo.Period};{m_InvUseInfo.Last_Use_No.ToString()};{m_InvUseInfo.Batch_Num};{m_InvUseInfo.Random_Code};{SqliteDataAccess.m_company_invoice_params[0].qrcode_aes_key}":"";//記錄在order_invoice_data中
                LogFile.Write($"POS_INVAPI.Get_New_InvoiceNo[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{m_StrAPIResult}");

            }
            else
            {
                StrOrderInvoiceData = "";
                LogFile.Write("POS_INVAPI.Get_New_InvoiceNo[false]: network error");
            }
            return m_blnAPIResult;
        }

        public static bool Check_InvoiceNo_Available(string Period="",string InvNo="")//發票期別(西元年格式),發票號碼
        {
            m_blnAPIResult = false;//API呼叫成功與否
            if ((HttpsFun.m_intNetworkLevel > 0))
            {
                string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);
                string strPeriod = (Period.Length > 0) ? Period : m_InvUseInfo.Period;
                string strInvNo = (InvNo.Length > 0) ? InvNo : m_InvUseInfo.Last_Use_No.ToString();

                IntPtr sMsg;
                m_blnAPIResult = Check_InvoiceNo_Available(StrInvParams, strPeriod, strInvNo, out sMsg);
                m_StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                m_InvResult = JsonClassConvert.inv_result2Class(m_StrAPIResult);

                LogFile.Write($"POS_INVAPI.Check_InvoiceNo_Available[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{strPeriod};{strInvNo};{m_StrAPIResult}");
            }
            else
            {
                LogFile.Write("POS_INVAPI.Check_InvoiceNo_Available[false]: network error");
            }
            return m_blnAPIResult;
        }

        public static bool POS_Order_2_Invoice_B2C_Order(string POS_Order,ref string StrAPIResult)
        {
            //轉換後的資料是拿來列印或上傳到 cloud的
            /*EX:
            String Strorders_new = SyncThread.Create_NOD_DOD_Json(m_StrPosOrderNumber);//直接使用訂單上傳VTEAM_CLOUD的JSON資料完整結構，包含訂單主檔，子檔、支付，載具等資訊
            POS_INVAPI.POS_Order_2_Invoice_B2C_Order(Strorders_new);//轉換後的資料是拿來列印或上傳到 cloud的
            */
            bool blnAPIResult = false;
            m_blnAPIResult = false;//API呼叫成功與否
            string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);
            try
            {
                IntPtr sMsg;
                blnAPIResult = POS_Order_2_Invoice_B2C_Order(StrInvParams, POS_Order, out sMsg);
                m_blnAPIResult = blnAPIResult;

                StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                m_StrAPIResult = StrAPIResult;

                LogFile.Write($"POS_INVAPI.POS_Order_2_Invoice_B2C_Order[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{POS_Order};{m_StrAPIResult}");
            }
            catch(Exception e) 
            {
                LogFile.Write($"POS_INVAPI.POS_Order_2_Invoice_B2C_Order[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{POS_Order};{m_StrAPIResult};{e.ToString()}");
            }

            return blnAPIResult;
        }

        public static bool POS_Report_2_Invoice_B2C_Summary(string POS_Report, ref string StrAPIResult)
        {
            bool blnAPIResult = false;
            m_blnAPIResult = false;//API呼叫成功與否
            string StrInvParams = JsonClassConvert.inv_params2String(m_InvParams);

            try
            {
                IntPtr sMsg;
                blnAPIResult = POS_Report_2_Invoice_B2C_Summary(StrInvParams, POS_Report, out sMsg);
                m_blnAPIResult = blnAPIResult;

                StrAPIResult = Marshal.PtrToStringAuto(sMsg);
                m_StrAPIResult = StrAPIResult;

                LogFile.Write($"POS_INVAPI.POS_Report_2_Invoice_B2C_Summary[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{POS_Report};{m_StrAPIResult}");
            }
            catch (Exception e)
            {
                LogFile.Write($"POS_INVAPI.POS_Report_2_Invoice_B2C_Summary[{(m_blnAPIResult ? "true" : "false")}]:\t{StrInvParams};{POS_Report};{m_StrAPIResult};{e.ToString()}");
            }


            return blnAPIResult;
        }

        //---模組相關

        //DLL API
        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Check_Renew_Invoice_Data")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Check_Renew_Invoice_Data(string Inv_Params, out IntPtr sMsg);//檢查是否需要更新發票字軌(檢查及更新)

        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Get_Invoice_Current_Info")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Get_Invoice_Current_Info(string Inv_Params, out IntPtr Inv_Use_Info);//讀取出當前可用的發票資訊

        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Get_New_InvoiceNo")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Get_New_InvoiceNo(string Inv_Params, out IntPtr Inv_Use_Info);//讀取出一組新的可用發票號，每成功讀取一次，該發票號碼將被註記

        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Check_InvoiceNo_Available")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Check_InvoiceNo_Available(string Inv_Params, string Period, string InvNo, out IntPtr State_Info);//檢查指定的電子發票號碼是否可用

        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "POS_Order_2_Invoice_B2C_Order")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool POS_Order_2_Invoice_B2C_Order(string Inv_Params, string POS_Order, out IntPtr Invoice_B2C_Order);//將POS訂單資料，轉成電子發票資料

        [DllImport("POS_INV.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "POS_Report_2_Invoice_B2C_Summary")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool POS_Report_2_Invoice_B2C_Summary(string Inv_Params, string POS_Report, out IntPtr Invoice_Summary);//將POS報表資料，轉成電子發票彙總表

    }//public class POS_INVAPI
}
