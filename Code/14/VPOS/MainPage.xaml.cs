using System.Data;
using System.Diagnostics;
using System.Reflection;
using VPOS.CustomControls;

namespace VPOS
{
    public partial class MainPage : ContentPage
    {
        public static String m_StrVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public static String m_StrDeviceCode = FileLib.GetDeviceCode();
        //---
        //訂單號相關變數
        private int m_intOrderState = 0;//0:new,1已結帳,2暫存
        public static String m_StrPosOrderNumber = "";//紀錄訂單號的成員變數
        public static String m_StrCustEinNumber = "";//紀錄客戶統一編號
        public static String m_StrCustCarrierNumber = "";//紀錄客戶發票載具
        public static bool m_blnMobileCarrier = false;//紀錄客戶發票載具類型
        public static String m_StrEasyCardPhysicalID = "";//紀錄交易悠遊卡內碼(發票載具)
        public static String m_StrLinePayCarrierNumber = "";//紀錄LinePay的paymentSID(發票載具)
        public static int m_intClassSid = 0;
        public static String m_StrClassName = "";//紀錄顯示用的班別資訊
        public static String m_StrGuestsNumber = "0";//來客數
        public static String m_StrEmployeeNo = "";//紀錄登入帳號的employee_no值
        public static String m_StrUserAccount = "";//紀錄登入帳號的user_account值
        public static String m_StrUserSID = "";//紀錄登入帳號的user_sid值
        private const int m_intOrderNumberTestMaxCount = 50;//建立訂單號最大嘗試次數
        //---訂單號相關變數

        //---
        //建立orderButton陣列
        public order_type_data_params m_order_type_data_params = null;
        public static int m_intOrderTypeIdSelected = 0;
        public static String m_StrOrderTypeNameSelected = "";
        public static String m_StrOrderTypeCodeSelected = "";
        public static int m_intOrderTypeInvoiceState = 0;//開立發票選項 0:預設 1:暫停不開 2:詢問
        public static int m_intVTS_TOGOInvoiceState = 0;//開立發票選項 0:預設 1:暫停不開 2:詢問
        public static int m_intFOODPANDAInvoiceState = 0;//開立發票選項 0:預設 1:暫停不開 2:詢問
        public static int m_intUBER_EATSInvoiceState = 0;//開立發票選項 0:預設 1:暫停不開 2:詢問
        //---建立orderButton陣列

        //---
        //客顯資料相關變數
        public int m_intcmb006LastSelectedIndex = -1;
        public cust_display_data m_cust_display_dataNew = null;//新增資料
        //---客顯資料相關變數

        //---
        //SysParmUI member Var
        public static pos_serial_param m_pos_serial_paramBuf = null;//取號參數方式物件
        public static vteam_kds_api_info m_vteam_kds_api_infoBuf = null;//KDS參數物件
        //---SysParmUI member Var

        //---
        //報表號相關變數
        public static String m_StrPosReportNumber = "";//紀錄報表號的成員變數
        private const int m_intReportNumberTestMaxCount = 50;//建立報表號最大嘗試次數
        //---報表號相關變數

        //---
        //收支號相關變數
        public static String m_StrPosExpenseNumber = "";//紀錄報表號的成員變數
        private const int m_intExpenseNumberTestMaxCount = 50;//建立報表號最大嘗試次數
        //---收支號相關變數

        //---
        //外送平台串接資料
        public static VTSTORE_params m_VTSTORE_params = new VTSTORE_params();//點點食
        public static NIDIN_POS_params m_NIDIN_POS_params = new NIDIN_POS_params();//你訂
        public static UBER_EATS_params m_UBER_EATS_params = new UBER_EATS_params();//吳柏毅
        public static FOODPANDA_params m_FOODPANDA_params = new FOODPANDA_params();//熊貓
        public static YORES_POS_params m_YORES_POS_params = new YORES_POS_params();//享什麼
        public void takeaways_params2Var()
        {
            SQLDataTableModel.takeaways_paramsLoad();
            if (SQLDataTableModel.m_takeaways_params != null)
            {
                for (int i = 0; i < SQLDataTableModel.m_takeaways_params.Rows.Count; i++)
                {
                    switch (SQLDataTableModel.m_takeaways_params.Rows[i]["SID"].ToString())
                    {
                        case "VTSTORE":
                            m_VTSTORE_params = JsonClassConvert.VTSTORE_params2Class(SQLDataTableModel.m_takeaways_params.Rows[i]["params"].ToString());
                            break;
                        case "NIDIN_POS":
                            m_NIDIN_POS_params = JsonClassConvert.NIDIN_POS_params2Class(SQLDataTableModel.m_takeaways_params.Rows[i]["params"].ToString());
                            break;
                        case "UBER_EATS":
                            m_UBER_EATS_params = JsonClassConvert.UBER_EATS_params2Class(SQLDataTableModel.m_takeaways_params.Rows[i]["params"].ToString());
                            break;
                        case "FOODPANDA":
                            m_FOODPANDA_params = JsonClassConvert.FOODPANDA_params2Class(SQLDataTableModel.m_takeaways_params.Rows[i]["params"].ToString());
                            break;
                        case "YORES_POS":
                            m_YORES_POS_params = JsonClassConvert.YORES_POS_params2Class(SQLDataTableModel.m_takeaways_params.Rows[i]["params"].ToString());
                            break;
                    }
                }
            }
        }
        //---外送平台串接資料

        //---
        //新增建立系統號碼字串函數
        public static int NowClassDataGet()//取得目前班別
        {
            String SQL = "";
            int intResult = 0;
            if (SqliteDataAccess.m_class_data.Count == 0)
            {
                intResult = 1;//沒有班別資料
            }

            if (SqliteDataAccess.m_terminal_data[0].now_class_sid == "0")
            {
                SQL = String.Format("UPDATE terminal_data SET now_class_sid = '{0}'", SqliteDataAccess.m_class_data[0].SID);
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                SqliteDataAccess.m_terminal_data = SqliteDataAccess.terminal_dataLoad();
            }

            for (int j = 0; j < SqliteDataAccess.m_class_data.Count; j++)
            {
                /*
                DateTime dt1 = Convert.ToDateTime(SqliteDataAccess.m_class_data[j].time_start);
                DateTime dt2 = Convert.ToDateTime(SqliteDataAccess.m_class_data[j].time_end);
                DateTime dt3 = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                if ((DateTime.Compare(dt2, dt3) >= 0) && (DateTime.Compare(dt3, dt1) >= 0))
                {
                    m_intClassSid = SqliteDataAccess.m_class_data[j].SID;
                    m_StrClassName = SqliteDataAccess.m_class_data[j].class_name;
                    break;
                }
                */

                if (SqliteDataAccess.m_terminal_data[0].now_class_sid == SqliteDataAccess.m_class_data[j].SID.ToString())
                {
                    m_intClassSid = SqliteDataAccess.m_class_data[j].SID;
                    m_StrClassName = SqliteDataAccess.m_class_data[j].class_name;
                    break;
                }
            }

            return intResult;
        }

        public static int SystemNumberCreate(int intType = 0)
        {
            int intResult = 0;
            String StrGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

            switch (intType)
            {
                case 0://ORDER_DATA
                    if (m_StrPosOrderNumber.Length == 0)
                    {
                        m_StrPosOrderNumber = PosOrderNumberCreate(StrGuid); //UI顯示班別資訊
                        if (m_StrPosOrderNumber.Length == 0)
                        {
                            intResult = 1;//建立失敗
                        }
                        else
                        {
                            intResult = 0;//建立成功
                        }
                    }
                    else
                    {
                        intResult = 0;//已存在不用建立
                    }
                    break;

                case 1://DAILY_REPORT
                    PosReportNumberCreate(StrGuid, intType);
                    if (m_StrPosReportNumber.Length == 0)
                    {
                        intResult = 1;//建立失敗
                    }
                    else
                    {
                        intResult = 0;//建立成功
                    }
                    break;

                case 2://CLASS_REPORT
                    PosReportNumberCreate(StrGuid, intType);
                    if (m_StrPosReportNumber.Length == 0)
                    {
                        intResult = 1;//建立失敗
                    }
                    else
                    {
                        intResult = 0;//建立成功
                    }
                    break;

                case 3://EXPENSE_DATA
                    PosExpenseNumberCreate(StrGuid);
                    if (m_StrPosExpenseNumber.Length == 0)
                    {
                        intResult = 1;//建立失敗
                    }
                    else
                    {
                        intResult = 0;//建立成功
                    }
                    break;
            }



            return intResult;
        }

        public static String SerialCodeDataGet(String StrGUID, int intType)
        {
            String SQL = "";
            String StrResultNumber = "";
            String StrSerialName = "";
            switch (intType)
            {
                case 0://PosOrderNumber
                    StrSerialName = "ORDER_DATA";
                    break;
                case 1://DAILY_REPORT
                    StrSerialName = "DAILY_REPORT";
                    break;
                case 2://CLASS_REPORT
                    StrSerialName = "CLASS_REPORT";
                    break;
                case 3://EXPENSE_REPORT
                    StrSerialName = "EXPENSE_DATA";
                    break;
            }

            SqliteDataAccess.m_serial_code_data = SqliteDataAccess.serial_code_dataLoad();//載入最新資料，以便進行比對

            if (SqliteDataAccess.m_serial_code_data.Count > 0)
            {
                for (int i = 0; i < SqliteDataAccess.m_serial_code_data.Count; i++)
                {
                    if (SqliteDataAccess.m_serial_code_data[i].serial_name == StrSerialName)
                    {
                        //---
                        //取得規則資料
                        String Strcode_first_charBuf = SqliteDataAccess.m_serial_code_data[i].code_first_char;
                        String Strcode_split_charrBuf = SqliteDataAccess.m_serial_code_data[i].code_split_char;
                        int intcode_num_lenBuf = SqliteDataAccess.m_serial_code_data[i].code_num_len;
                        //---取得規則資料

                        //----
                        //組合出今天serial_code_data資料表 的 code_str欄位資料
                        Strcode_first_charBuf = Strcode_first_charBuf.Replace("[YEAR]", "yyyy");
                        Strcode_first_charBuf = Strcode_first_charBuf.Replace("[MONTH]", "MM");
                        Strcode_first_charBuf = Strcode_first_charBuf.Replace("[DAY]", "dd");
                        Strcode_first_charBuf = Strcode_first_charBuf.Replace("[HOUR]", "HH");
                        Strcode_first_charBuf = Strcode_first_charBuf.Replace("[MINUTE]", "mm");
                        String[] strs = Strcode_first_charBuf.Split("yyyy");
                        String StrHead = strs[0];
                        Strcode_first_charBuf = "yyyy" + strs[1];
                        String StrNowcode_str = StrHead + DateTime.Now.ToString(Strcode_first_charBuf);
                        //---組合出今天serial_code_data資料表 的 code_str欄位資料

                        //---
                        //比對code_str欄位資料
                        int intNowcode_num = 0;
                        String StrNowcode_num = "";
                        String Strcode_str = SqliteDataAccess.m_serial_code_data[i].code_str;
                        int intcode_num = SqliteDataAccess.m_serial_code_data[i].code_num;

                        if (StrNowcode_str == Strcode_str)
                        {
                            intNowcode_num = intcode_num + 1;
                        }
                        else
                        {
                            intNowcode_num = 1;
                        }
                        //---比對code_str欄位資料

                        StrNowcode_num = "" + intNowcode_num;
                        StrResultNumber = String.Format("{0}{1}{2}", StrNowcode_str, Strcode_split_charrBuf, StrNowcode_num.PadLeft(intcode_num_lenBuf, '0'));

                        //---
                        //更新serial_code_data資料表紀錄
                        SQL = String.Format("UPDATE serial_code_data SET code_str = '{0}', code_num = '{1}', updated_time = '{2}', serial_owner='{3}' WHERE serial_name = '{4}';", StrNowcode_str, intNowcode_num, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), StrGUID, StrSerialName);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        //---更新serial_code_data資料表紀錄

                        break;//跳離for
                    }//if (SqliteDataAccess.m_serial_code_data[i].serial_name == "DAILY_REPORT")
                }//for (int i = 0; i < SqliteDataAccess.m_serial_code_data.Count; i++)
            }//if (SqliteDataAccess.m_serial_code_data.Count > 0)

            return StrResultNumber;
        }

        public static void SerialCodeDataInit(String StrGUID)
        {
            String SQL = "";
            SqliteDataAccess.m_serial_code_data.Clear();
            SqliteDataAccess.m_serial_code_data = SqliteDataAccess.serial_code_dataLoad();
            if (SqliteDataAccess.m_serial_code_data.Count == 0)
            {
                SQL = String.Format("INSERT INTO serial_code_data (serial_type,serial_name,code_first_char,code_split_char,code_num_len,code_str,code_num,serial_owner,updated_time) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    "ORDER_DATA", "ORDER_DATA", "[YEAR][MONTH][DAY]", "-", "4", DateTime.Now.ToString("yyyMMdd"), 0, StrGUID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                SQL = String.Format("INSERT INTO serial_code_data (serial_type,serial_name,code_first_char,code_split_char,code_num_len,code_str,code_num,serial_owner,updated_time) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    "DAILY_REPORT", "DAILY_REPORT", "DR-[YEAR][MONTH]", "", "3", "DR-" + DateTime.Now.ToString("yyyMM"), 0, StrGUID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                SQL = String.Format("INSERT INTO serial_code_data (serial_type,serial_name,code_first_char,code_split_char,code_num_len,code_str,code_num,serial_owner,updated_time) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    "CLASS_REPORT", "CLASS_REPORT", "CR-[YEAR][MONTH]", "", "3", "CR-" + DateTime.Now.ToString("yyyMM"), 0, StrGUID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                SQL = String.Format("INSERT INTO serial_code_data (serial_type,serial_name,code_first_char,code_split_char,code_num_len,code_str,code_num,serial_owner,updated_time) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                "EXPENSE_DATA", "EXPENSE_DATA", "[YEAR][MONTH][DAY]", "", "4", DateTime.Now.ToString("yyyMMdd"), 0, StrGUID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                SqliteDataAccess.m_serial_code_data = SqliteDataAccess.serial_code_dataLoad();
            }
        }

        public static void PosExpenseNumberCreate(String StrGUID)
        {
            String SQL = "";
            m_StrPosExpenseNumber = "";

            //---
            //產生收支號的主流程
            int intErrorTestCount = 0;
            do
            {
                String StrPosExpenseNumberBuf = SerialCodeDataGet(StrGUID, 3);
                SQL = String.Format("SELECT expense_no FROM expense_data WHERE expense_no='{0}' LIMIT 0,1;", StrPosExpenseNumberBuf);
                DataTable expense_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
                if ((expense_dataDataTable != null) && (expense_dataDataTable.Rows.Count > 0))
                {
                    expense_dataDataTable.Clear();
                }
                else
                {
                    m_StrPosExpenseNumber = StrPosExpenseNumberBuf;//紀錄產生的新訂單號
                    break;//跳離do-while
                }

                intErrorTestCount++;
            } while (intErrorTestCount <= m_intExpenseNumberTestMaxCount);
            //---產生收支號的主流程

        }

        public static void PosReportNumberCreate(String StrGUID, int type)
        {
            String SQL = "";
            m_StrPosReportNumber = "";

            //---
            //產生報表號的主流程
            int intErrorTestCount = 0;
            do
            {
                String StrResult = "";//String StrPosReportNumberBuf = SerialCodeDataGet(StrGUID, type);
                if (type == 1)//關帳
                {
                    //StrResult = (SysParmUI.m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetDailyNo() : SerialCodeDataGet(StrGUID, 1);
                    StrResult = (m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetDailyNo() : SerialCodeDataGet(StrGUID, 1);
                }
                else//交班
                {
                    //StrResult = (SysParmUI.m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetClassNo() : SerialCodeDataGet(StrGUID, 2);
                    StrResult = (m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetClassNo() : SerialCodeDataGet(StrGUID, 2);
                }

                if (StrResult.Length == 0)
                {
                    intErrorTestCount++;
                    if (intErrorTestCount <= m_intReportNumberTestMaxCount)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                SQL = String.Format("SELECT report_no FROM daily_report WHERE report_no='{0}' LIMIT 0,1;", StrResult);
                DataTable daily_reportDataTable = SQLDataTableModel.GetDataTable(SQL);
                if ((daily_reportDataTable != null) && (daily_reportDataTable.Rows.Count > 0))
                {
                    daily_reportDataTable.Clear();
                }
                else
                {
                    m_StrPosReportNumber = StrResult;//紀錄產生的新訂單號
                    break;//跳離do-while
                }

                intErrorTestCount++;
            } while (intErrorTestCount <= m_intReportNumberTestMaxCount);
            //---產生報表號的主流程

        }

        public static String PosOrderNumberCreate(String StrGUID)
        {
            String StrResult = "";
            int intclass_sid = 0;
            String Strclass_name = "";
            String SQL = "";

            //---
            //產生訂單號的主流程
            int intErrorTestCount = 0;
            do
            {
                //StrResult = (SysParmUI.m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetOrderNo() : SerialCodeDataGet(StrGUID, 0);
                StrResult = (m_pos_serial_paramBuf.order_no_from == "S") ? HttpGetOrderNo() : SerialCodeDataGet(StrGUID, 0);
                if (StrResult.Length == 0)
                {
                    intErrorTestCount++;
                    if (intErrorTestCount <= m_intOrderNumberTestMaxCount)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                SQL = String.Format("SELECT order_no FROM order_data WHERE order_no='{0}' LIMIT 0,1;", StrResult);
                DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
                if ((order_dataDataTable != null) && (order_dataDataTable.Rows.Count > 0))
                {
                    order_dataDataTable.Clear();
                }
                else
                {
                    //停用直接賦予值 因為有可能取號是為了別的設備 m_StrPosOrderNumber = StrResult;//紀錄產生的新訂單號

                    SQL = String.Format("UPDATE terminal_data SET last_order_no = '{0}'", StrResult);
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                    //---
                    //新增一筆order_data資料
                    String Strorder_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    NowClassDataGet();//取得目前班別
                    intclass_sid = m_intClassSid;
                    Strclass_name = m_StrClassName;

                    //訂單狀態(order_state) 0:訂購中，1:已結帳 2: 暫存中
                    //點單模式(order_mode) L:本機點單，O:線上點單 S:自助點單
                    //取號來源(order_no_from) (C:Cloud ，L:Local，S:主定的主機)
                    //付款狀態(paid_flag) Y: 已付款
                    //實收現金金額(cash_fee) - 沒有在使用 ，有在使用的是 order_payment_data 的 received_fee [order_payment_data 的 amount 是紀錄該筆的支付金額]
                    //找零金額(change_fee) - 沒有在使用 ，有在使用的是 order_payment_data 的 change_fee
                    //paid_time
                    //paid_info
                    //class_sid
                    //class_name - 班別
                    //user_sid - 從user_data&登入比對取得
                    //employee_no - 從user_data&登入比對取得
                    //call_num - 取餐號
                    //business_day - 營業日 [terminnal_data.business_day]

                    //labmain028.Text = Strclass_name; //UI顯示班別資訊
                    String business_day = Convert.ToDateTime(SqliteDataAccess.m_terminal_data[0].business_day).ToString("yyyy-MM-dd");

                    SQL = String.Format("INSERT INTO order_data (order_open_time,order_no, order_no_from, order_time,terminal_sid,class_sid,user_sid,created_time,updated_time,pos_no,order_state,order_mode,paid_flag,class_name,order_type,order_type_name,order_type_code,business_day,employee_no,upload_time,cancel_time,cancel_upload_time,paid_time) VALUES ('{1}','{0}', 'L', '{1}','{4}',{2},{3},'{1}','{1}','{5}',0,'L','N','{6}','{7}','{8}','{9}','{10}','{11}','1970-01-01','1970-01-01','1970-01-01','1970-01-01');", StrResult, Strorder_time, intclass_sid, m_StrUserSID, SqliteDataAccess.m_terminal_data[0].SID, SqliteDataAccess.m_terminal_data[0].pos_no, Strclass_name, m_intOrderTypeIdSelected, m_StrOrderTypeNameSelected, m_StrOrderTypeCodeSelected, business_day, m_StrEmployeeNo);
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    //---新增一筆order_data資料

                    break;//跳離do-while
                }

                intErrorTestCount++;
            } while (intErrorTestCount <= m_intOrderNumberTestMaxCount);
            //---產生訂單號的主流程

            return StrResult;
        }

        //---新增建立系統號碼字串函數

        public static String m_StrHasDaily = "";
        public static DateTime m_ClassLastTime = DateTime.Now;//最後一次執行交班時間
        public static DateTime m_DailyLastTime = DateTime.Now;//最後一次執行關帳時間

        public static DateTime m_ClassOrderFirstTime = DateTime.Now;
        public static DateTime m_ClassOrderLastTime = DateTime.Now;

        public static DateTime m_DailyOrderFirstTime = DateTime.Now;
        public static DateTime m_DailyOrderLastTime = DateTime.Now;

        public static void GetClassDailyLastTime()//抓取 最後一次執行交班時間 & 最後一次執行關帳時間
        {
            /*
            SELECT 
	            SUM(class_last_time) AS class_last_time,
	            SUM(daily_last_time) AS daily_last_time  
            FROM 
	            (
	            SELECT 
		            STRFTIME('%s',MAX(report_time)) AS class_last_time,
		            0 AS daily_last_time 
		            FROM daily_report 
		            WHERE report_type='C' 
	            UNION 
	            SELECT 
		            0 AS class_last_time,
		            STRFTIME('%s',MAX(report_time)) AS daily_last_time 
		            FROM daily_report 
		            WHERE report_type='D'
	            ); 
            */
            String SQL = "SELECT SUM(class_last_time) AS class_last_time,SUM(daily_last_time) AS daily_last_time  FROM (SELECT STRFTIME('%s',MAX(report_time)) AS class_last_time,0 AS daily_last_time FROM daily_report WHERE report_type='C' UNION SELECT 0 AS class_last_time,STRFTIME('%s',MAX(report_time)) AS daily_last_time FROM daily_report WHERE report_type='D')";
            DataTable dt = SQLDataTableModel.GetDataTable(SQL);

            // 紀錄最後的交班時間及關帳時間
            m_ClassLastTime = TimeConvert.UnixTimeStampToDateTime(Convert.ToDouble(dt.Rows[0][0].ToString()), false);
            m_DailyLastTime = TimeConvert.UnixTimeStampToDateTime(Convert.ToDouble(dt.Rows[0][1].ToString()), false);
            // 若交班的最後時間，小於關帳時間，就使用關帳時間做為期使時間
            if (DateTime.Compare(m_ClassLastTime, m_DailyLastTime) < 0)
            {
                m_ClassLastTime = m_DailyLastTime;
            }

            SQL = "SELECT business_day FROM terminal_data LIMIT 0,1";
            dt = SQLDataTableModel.GetDataTable(SQL);
            m_StrHasDaily = Convert.ToDateTime(dt.Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff");

            /*
            SQL = String.Format("SELECT MIN(order_time) AS order_first_time,MAX(order_time) AS order_last_time FROM order_data WHERE order_time >='{0}' AND order_time <='{1}' AND daily_close_flag != 'Y'", m_DailyLastTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            dt = SQLDataTableModel.GetDataTable(SQL);
            if((dt!=null)&&(dt.Rows.Count > 0))
            {
                m_DailyOrderFirstTime = (dt.Rows[0][0].ToString().Length>0)? Convert.ToDateTime(dt.Rows[0][0].ToString()) : DateTime.Now;
                m_DailyOrderLastTime = (dt.Rows[0][1].ToString().Length > 0)? Convert.ToDateTime(dt.Rows[0][1].ToString()) : DateTime.Now;
            }

            SQL = String.Format("SELECT MIN(order_time) AS order_first_time,MAX(order_time) AS order_last_time FROM order_data WHERE order_time >='{0}' AND order_time <='{1}' AND class_close_flag != 'Y'", m_ClassLastTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            dt = SQLDataTableModel.GetDataTable(SQL);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                m_ClassOrderFirstTime = (dt.Rows[0][0].ToString().Length > 0) ? Convert.ToDateTime(dt.Rows[0][0].ToString()) : DateTime.Now;
                m_ClassOrderLastTime = (dt.Rows[0][1].ToString().Length > 0) ? Convert.ToDateTime(dt.Rows[0][1].ToString()) : DateTime.Now;
            }
            */
            m_DailyOrderFirstTime = m_DailyLastTime;
            m_DailyOrderLastTime = DateTime.Now;
            m_ClassOrderFirstTime = m_ClassLastTime;
            m_ClassOrderLastTime = DateTime.Now;
        }


        public bool HttpObjectInit(ref bool blnServerMode)//Http 服務相關元件初始化
        {
            bool blnResult = true;
            //SysParmUI.SysParmUIMemberVarSet();//讀取 取號方式(Server/Local 方式) & KDS參數
            //if (SysParmUI.m_pos_serial_paramBuf.terminal_server_flag == "Y")//啟動服務
            if (m_pos_serial_paramBuf.terminal_server_flag == "Y")//啟動服務
            {
                blnServerMode = true;
                //blnResult = HttpServerThread.Start(Convert.ToInt32(SysParmUI.m_pos_serial_paramBuf.terminal_server_port));
                blnResult = HttpServerThread.Start(Convert.ToInt32(m_pos_serial_paramBuf.terminal_server_port));
            }
            else
            {
                blnServerMode = false;
                //if (SysParmUI.m_pos_serial_paramBuf.order_no_from == "S")//主機取號
                if (m_pos_serial_paramBuf.order_no_from == "S")//主機取號
                {
                    /*
                    //單純主機IP&PORT連線測試
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        tcpClient.Connect(SysParmUI.m_pos_serial_paramBuf.serial_server_name, SysParmUI.m_pos_serial_paramBuf.serial_server_port);
                        tcpClient.Close();
                        blnResult = true;
                    }
                    catch
                    {
                        blnResult = false;
                    }
                    tcpClient = null;
                    */

                    blnResult = MainPage.HttpTestConnection();
                }
            }
            return blnResult;
        }

        public static String HttpGetOrderNo()
        {
            String StrResult = "";
            //String StrDomain = $"http://{SysParmUI.m_pos_serial_paramBuf.serial_server_name}:{SysParmUI.m_pos_serial_paramBuf.serial_server_port}/";
            String StrDomain = $"http://{m_pos_serial_paramBuf.serial_server_name}:{m_pos_serial_paramBuf.serial_server_port}/";
            String StrBuf = HttpsFun.RESTfulAPI_get(StrDomain, "getorderno", "", "", "");
            getorderno getordernoBuf = JsonClassConvert.getorderno2Class(StrBuf);
            if (getordernoBuf != null)
            {
                StrResult = getordernoBuf.orderno;
            }
            return StrResult;
        }

        public static String HttpGetDailyNo()
        {
            String StrResult = "";
            //String StrDomain = $"http://{SysParmUI.m_pos_serial_paramBuf.serial_server_name}:{SysParmUI.m_pos_serial_paramBuf.serial_server_port}/";
            String StrDomain = $"http://{m_pos_serial_paramBuf.serial_server_name}:{m_pos_serial_paramBuf.serial_server_port}/";
            String StrBuf = HttpsFun.RESTfulAPI_get(StrDomain, "getdailyno", "", "", "");
            getdailyno getdailynoBuf = JsonClassConvert.getdailyno2Class(StrBuf);
            if (getdailynoBuf != null)
            {
                StrResult = getdailynoBuf.dailyno;
            }
            return StrResult;
        }

        public static String HttpGetClassNo()
        {
            String StrResult = "";
            //String StrDomain = $"http://{SysParmUI.m_pos_serial_paramBuf.serial_server_name}:{SysParmUI.m_pos_serial_paramBuf.serial_server_port}/";
            String StrDomain = $"http://{m_pos_serial_paramBuf.serial_server_name}:{m_pos_serial_paramBuf.serial_server_port}/";
            String StrBuf = HttpsFun.RESTfulAPI_get(StrDomain, "getclassno", "", "", "");
            getclassno getclassnoBuf = JsonClassConvert.getclassno2Class(StrBuf);
            if (getclassnoBuf != null)
            {
                StrResult = getclassnoBuf.classno;
            }
            return StrResult;
        }

        public static bool HttpTestConnection()
        {
            bool blnResult = false;
            //String StrDomain = $"http://{SysParmUI.m_pos_serial_paramBuf.serial_server_name}:{SysParmUI.m_pos_serial_paramBuf.serial_server_port}/";
            String StrDomain = $"http://{m_pos_serial_paramBuf.serial_server_name} : {m_pos_serial_paramBuf.serial_server_port}/";
            String StrBuf = HttpsFun.RESTfulAPI_get(StrDomain, "testconnection", "", "", "");
            testconnection testconnectionBuf = JsonClassConvert.testconnection2Class(StrBuf);
            if ((testconnectionBuf != null) && (testconnectionBuf.servername.Length > 0))
            {
                blnResult = true;
            }
            return blnResult;
        }


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