using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{

    public class VTEAMQrorderAPI
    {
        //c# httpwebrequest basic authentication
        //https://stackoverflow.com/questions/4334521/httpwebrequest-using-basic-authentication
        //https://zh.wikipedia.org/zh-tw/ISO/IEC_8859-1
        private static String username = "";//terminal_sid
        private static String password = "";//api_token

        public static int m_intQrorderButtonIndex =-1;
        public static get_qrorder_params m_get_qrorder_params = null;
        public static get_print_queue_data m_get_print_queue_data = null;
        public static bool get_qrorder_params()//取得掃碼點單模組參數
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }
            if ((SqliteDataAccess.m_terminal_data != null) && (SqliteDataAccess.m_terminal_data.Count > 0))
            {
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS
                String StrResult = HttpsFun.RESTfulAPI_get(StrDomain, "/api/qrorder/get/qr_order_params","", "Authorization", "Basic " + encoded);
                m_get_qrorder_params = JsonClassConvert.get_qrorder_params2Class(StrResult);
                if(m_get_qrorder_params!=null)
                {
                    blnResult = true;
                    if((m_get_qrorder_params.data!=null) && (m_get_qrorder_params.data.terminal_sid!=null) && (m_get_qrorder_params.data.terminal_sid == SqliteDataAccess.m_terminal_data[0].SID))
                    {
                        SQLDataTableModel.m_blnVTEAMQrorderOpen = true;//同一台設備
                    }
                    else
                    {
                        SQLDataTableModel.m_blnVTEAMQrorderOpen = false;//不同台設備
                    }
                }
                else
                {
                    SQLDataTableModel.m_blnVTEAMQrorderOpen = false;//無法比對
                }
            }

            return blnResult;
        }

        public static bool get_print_queue_data()//取得待列印的資料
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }
            if ((SqliteDataAccess.m_terminal_data!=null) && (SqliteDataAccess.m_terminal_data.Count>0))
            {
                m_get_print_queue_data = null;
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS
                String StrResult = HttpsFun.RESTfulAPI_get(StrDomain, "/api/qrorder/get/print_queue_data","", "Authorization", "Basic " + encoded);
                m_get_print_queue_data = JsonClassConvert.get_print_queue_data2Class(StrResult);
                if(m_get_print_queue_data!=null)
                {
                    blnResult = true;
                }
            }
            
            return blnResult;
        }

        public static bool update_print_queue_data(ArrayList ALQueueSidState)//列印排程資料狀態變更
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }
            if ((SqliteDataAccess.m_terminal_data != null) && (SqliteDataAccess.m_terminal_data.Count > 0))
            {
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS
                List<update_print_queue_data> ListDataBuf= new List<update_print_queue_data>();
                for(int i=0;i< ALQueueSidState.Count;i++)
                {
                    string[] strs = ALQueueSidState[i].ToString().Split(';');
                    update_print_queue_data update_print_queue_dataBuf = new update_print_queue_data();
                    update_print_queue_dataBuf.print_state = strs[1];
                    update_print_queue_dataBuf.print_sid = strs[0];
                    ListDataBuf.Add(update_print_queue_dataBuf);
                }

                String StrInput = JsonClassConvert.update_print_queue_data2String(ListDataBuf);
                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain, "/api/qrorder/update/print_queue_data", StrInput, "Authorization", "Basic " + encoded);
                blnResult = true;
            }

            return blnResult;
        }

        public static get_qrorder_order_list m_get_qrorder_order_list = new get_qrorder_order_list();
        public static bool get_order_list()//讀取待結帳的掃碼訂單清單
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }
            if ((SqliteDataAccess.m_terminal_data != null) && (SqliteDataAccess.m_terminal_data.Count > 0))
            {
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS
                String StrResult = HttpsFun.RESTfulAPI_get(StrDomain, "/api/qrorder/get/order_list", "", "Authorization", "Basic " + encoded);

                m_get_qrorder_order_list = JsonClassConvert.get_qrorder_order_list2Class(StrResult);
                if((m_get_qrorder_order_list!=null) && (m_get_qrorder_order_list.data!=null))
                {
                    blnResult = true;
                }
            }

            return blnResult;
        }

        public static get_qrorder_order_data m_get_qrorder_order_data =new get_qrorder_order_data();
        public static bool get_order_data(String StrTransaction_id)//讀取指定訂單完整資料
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }
            if ((SqliteDataAccess.m_terminal_data != null) && (SqliteDataAccess.m_terminal_data.Count > 0))
            {
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS
                String StrInput = StrTransaction_id;
                String StrResult = HttpsFun.RESTfulAPI_get(StrDomain, "/api/qrorder/get/order_data", StrInput, "Authorization", "Basic " + encoded);
                m_get_qrorder_order_data = JsonClassConvert.get_qrorder_order_data2Class(StrResult);
                if((m_get_qrorder_order_data!=null) && (m_get_qrorder_order_data.data != null))
                {
                    blnResult = true;
                }
            }

            return blnResult;
        }
        public static bool update_order_data(String StrTransactionID)//修改指定訂單的狀態
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if ((SqliteDataAccess.m_terminal_data != null) && (SqliteDataAccess.m_terminal_data.Count > 0))
            {
                String username = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                password = SqliteDataAccess.m_terminal_data[0].api_token_id;//api_token
                String encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                String StrDomain = HttpsFun.setDomainMode(1);//POS

                update_order_data update_order_dataBuf = new update_order_data();
                update_order_dataBuf.state_flag = "finish";
                update_order_dataBuf.transaction_id = StrTransactionID;
                String StrInput = JsonClassConvert.update_order_data2String(update_order_dataBuf);
                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain, "/api/qrorder/update/order_data", StrInput, "Authorization", "Basic " + encoded);
                blnResult = true;
            }

            return blnResult;
        }
    }//public class VTEAMQrorderAPI
}
