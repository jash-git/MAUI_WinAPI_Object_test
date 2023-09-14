using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class UbereatsAPI
    {
        private static int m_intLimitTime = 1;//分鐘
        private static oauthInput m_oauthInput = new oauthInput();
        public static void varInit()
        {
            m_oauthInput.client_secret = "";
            m_oauthInput.client_id = "";
            try
            {
                m_oauthInput.client_id = SqliteDataAccess.m_terminal_data[0].client_id;
                m_oauthInput.client_secret = SqliteDataAccess.m_terminal_data[0].client_secret;
            }
            catch
            {
                m_oauthInput.client_secret = "";
                m_oauthInput.client_id = "";
            }
        }

        private static oauthResult m_oauthResult = new oauthResult();
        public static String m_Straccess_token = "";
        public static DateTime m_DTexpires_time = DateTime.Now;
        public static bool Authentication()
        {
            bool blnResult = false;

            if ((m_Straccess_token.Length == 0) || (ValidityCalculate() < m_intLimitTime))
            {
                String StrData = JsonClassConvert.oauthInput2String(m_oauthInput);
                StrData = StrData.Substring(0, StrData.Length - 1);
                StrData += @",""scope"":""vt.takeaways""}";

                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain, "/api/oauth/token", StrData, "", "");//String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain, "/api/oauth", StrData, "", "");
                m_oauthResult = JsonClassConvert.oauthResult2Class(StrResult);
                m_Straccess_token = "";
                m_DTexpires_time = DateTime.Now;
                if ((m_oauthResult != null) && (m_oauthResult.status == "OK"))
                {
                    blnResult = true;
                    m_Straccess_token = m_oauthResult.access_token;
                    m_DTexpires_time = TimeConvert.UnixTimeStampToDateTime(Convert.ToDouble(m_oauthResult.expires_unixtime));
                }
                else
                {
                    blnResult = false;
                }
            }
            else
            {
                blnResult = true;
            }

            return blnResult;
        }

        private static int ValidityCalculate()//回復還有幾分鐘失效
        {
            int intResult = 0;
            TimeSpan ts1 = new TimeSpan(m_DTexpires_time.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            if (ts.TotalSeconds > 0)
            {
                intResult = (int)(ts.TotalSeconds / 60);
            }
            else
            {
                intResult = 0;
            }
            return intResult;
        }

        public static Ubereats_ordersnew m_Ubereats_ordersnew = new Ubereats_ordersnew();
        public static bool ordersnew()////抓取資料&寫進DB
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if (Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String[] StrHeaderName = new String[] { "Authorization" };
                String[] StrHeaderValue = new String[] { "Bearer " + m_Straccess_token };
                String StrOutput = HttpsFun.RESTfulAPI_get(StrDomain, $"/api/takeaways/uber_eats/orders/new", "", StrHeaderName, StrHeaderValue);//String StrOutput = HttpsFun.RESTfulAPI_get(StrDomain, $"/api/vtstore/orders/new", "", StrHeaderName, StrHeaderValue);
                m_Ubereats_ordersnew = JsonClassConvert.Ubereats_ordersnew2Class(StrOutput);
                if ((m_Ubereats_ordersnew != null) && (m_Ubereats_ordersnew.status == "OK"))
                {
                    String SQL = "";
                    //---
                    //清空還未處理的訂單
                    SQL = "DELETE FROM takeaways_order_data WHERE platform_sid='uber_eats' AND (order_state = 'CREATED' OR order_state = 'ACCEPTED')";
                    SQLDataTableModel.SQLiteInsertUpdateDelete("Takeaways", SQL);
                    //---清空還未處理的訂單

                    //---
                    //新增所有新訂單
                    for (int i = 0; i < m_Ubereats_ordersnew.data.Count; i++)
                    {
                        String SID = SQLDataTableModel.UUID2SID(64);//UUID
                        String platform_sid = "uber_eats";
                        String external_order_id = m_Ubereats_ordersnew.data[i].external_order_id;
                        String order_no = m_Ubereats_ordersnew.data[i].order_no;
                        double order_time = m_Ubereats_ordersnew.data[i].order_time;
                        String order_type = m_Ubereats_ordersnew.data[i].order_type;
                        String order_state = m_Ubereats_ordersnew.data[i].order_state;
                        String call_num = (m_Ubereats_ordersnew.data[i].call_num != null) ? m_Ubereats_ordersnew.data[i].call_num : "";
                        int item_count = m_Ubereats_ordersnew.data[i].item_count;
                        int subtotal = m_Ubereats_ordersnew.data[i].subtotal;
                        int promotion_fee = m_Ubereats_ordersnew.data[i].promotion_fee;
                        int discount_fee = m_Ubereats_ordersnew.data[i].delivery_fee;
                        int delivery_fee = m_Ubereats_ordersnew.data[i].delivery_fee;
                        int service_fee = m_Ubereats_ordersnew.data[i].service_fee;
                        int amount = m_Ubereats_ordersnew.data[i].amount;
                        String payment_type = (m_Ubereats_ordersnew.data[i].payment_type != null) ? m_Ubereats_ordersnew.data[i].payment_type : "";
                        String cust_name = m_Ubereats_ordersnew.data[i].cust_name;//(m_VTSTORE_ordersnew.data[i].customer.full_name != null) ? m_VTSTORE_ordersnew.data[i].customer.full_name : "";
                        String cust_phone = m_Ubereats_ordersnew.data[i].cust_phone;//(m_VTSTORE_ordersnew.data[i].customer.mobile_phone != null) ? m_VTSTORE_ordersnew.data[i].customer.mobile_phone : "";
                        String cust_tax_number = m_Ubereats_ordersnew.data[i].cust_tax_number;//(m_VTSTORE_ordersnew.data[i].customer.tax_number != null) ? m_VTSTORE_ordersnew.data[i].customer.tax_number : "";
                        double cust_reserve_time = m_Ubereats_ordersnew.data[i].cust_reserve_time;
                        String delivery_city_name = ((m_Ubereats_ordersnew.data[i].delivery != null) && (m_Ubereats_ordersnew.data[i].delivery.city != null)) ? m_Ubereats_ordersnew.data[i].delivery.city : "";
                        String delivery_district_name = ((m_Ubereats_ordersnew.data[i].delivery != null) && (m_Ubereats_ordersnew.data[i].delivery.district != null)) ? m_Ubereats_ordersnew.data[i].delivery.district : "";
                        String delivery_address = ((m_Ubereats_ordersnew.data[i].delivery != null) && (m_Ubereats_ordersnew.data[i].delivery.address != null)) ? m_Ubereats_ordersnew.data[i].delivery.address : "";
                        String remarks = (m_Ubereats_ordersnew.data[i].remarks != null) ? m_Ubereats_ordersnew.data[i].remarks : "";
                        double estimated_read_time = 0;//預計完成時間
                        String original_data = JsonClassConvert.UBEON_Datum2String(m_Ubereats_ordersnew.data[i]);
                        String sync_state = "N";
                        String sync_msg = "";
                        String pos_order_no = "";
                        String created_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        String updated_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        String pre_order = (m_Ubereats_ordersnew.data[i].pre_order == "Y") ? "Y" : "N";

                        SQL = String.Format("INSERT INTO takeaways_order_data (SID,platform_sid,external_order_id,order_no,order_time,order_type,order_state,call_num,item_count,subtotal,promotion_fee,discount_fee,delivery_fee,service_fee,amount,payment_type,cust_name,cust_phone,cust_tax_number,cust_reserve_time,delivery_city_name,delivery_district_name,delivery_address,remarks,estimated_read_time,original_data,sync_state,sync_msg,pos_order_no,created_time,updated_time,pre_order) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}')", SID, platform_sid, external_order_id, order_no, order_time, order_type, order_state, call_num, item_count, subtotal, promotion_fee, discount_fee, delivery_fee, service_fee, amount, payment_type, cust_name, cust_phone, cust_tax_number, cust_reserve_time, delivery_city_name, delivery_district_name, delivery_address, remarks, estimated_read_time, original_data, sync_state, sync_msg, pos_order_no, created_time, updated_time, pre_order);
                        SQLDataTableModel.SQLiteInsertUpdateDelete("Takeaways", SQL);
                    }
                    //---新增所有新訂單
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }
            }
            else
            {
                blnResult = false;
            }

            return blnResult;
        }

        public static VTSTORE_change_state m_VTSTORE_change_state = new VTSTORE_change_state();
        public static bool change_state(int intIndex, String Strexternal_order_id, String Straction_state, String Strstate_memo, String Strstore_reserve_time = "")//訂單狀態異動
        {
            bool blnResult = false;
            /*
            accept : 接收訂單 
            import : 轉入訂單 [只有UberEats;Foodpanda 才有]
            deny : 婉拒訂單 
            finish : 完成訂單 
            invalid : 註銷(作廢)             
            */
            String Strorder_state = "";
            switch (Straction_state)
            {
                case "accept":
                    Strorder_state = "ACCEPTED";//待處理訂單; ACCEPTED:
                    break;
                case "import":
                    Strorder_state = "IMPORT";
                    break;
                case "deny":
                    Strorder_state = "DENIED";//DENIED:店家取消訂單
                    break;
                case "finish":
                    Strorder_state = "FINISHED";//FINISHED:已完成
                    break;
                case "invalid":
                    Strorder_state = "CANCELED";//CANCELED:店家作廢訂單
                    break;
            }
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if (Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String[] StrHeaderName = new String[] { "Authorization" };
                String[] StrHeaderValue = new String[] { "Bearer " + m_Straccess_token };

                VTSTORE_change_input VTSTORE_change_inputBuf = new VTSTORE_change_input();
                VTSTORE_change_inputBuf.receiver_id = SqliteDataAccess.m_terminal_data[0].SID;
                VTSTORE_change_inputBuf.store_completion_time = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime((Strstore_reserve_time.Length > 0) ? Strstore_reserve_time : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                VTSTORE_change_inputBuf.reason = Strstate_memo;
                String StrInput = JsonClassConvert.VTSTORE_change_input2String(VTSTORE_change_inputBuf);
                String StrOutput = HttpsFun.RESTfulAPI_postBody(StrDomain, $"/api/takeaways/uber_eats/orders/{Strexternal_order_id}/action/{Straction_state}", StrInput, StrHeaderName, StrHeaderValue);//String StrOutput = HttpsFun.RESTfulAPI_postBody(StrDomain, $"/api/vtstore/orders/change_state", StrInputName, StrInputValue, StrHeaderName, StrHeaderValue);
                m_VTSTORE_change_state = JsonClassConvert.VTSTORE_change_state2Class(StrOutput);

                if ((m_VTSTORE_change_state != null) && (m_VTSTORE_change_state.status == "OK"))
                {
                    blnResult = true;

                    //---
                    //更新takeaways_order 訂單狀態
                    if ( (Straction_state== "accept") || (Straction_state == "import"))
                    {
                        Strorder_state = "FINISHED";
                    }
                    String StrExternalOrderId = "";//String StrExternalOrderId = TakeawayOrderList.m_takeaways_order_dataDataTable.Rows[intIndex]["external_order_id"].ToString();

                    String SQL = String.Format("UPDATE takeaways_order_data SET order_state='{2}', updated_time = '{1}' WHERE external_order_id='{0}'", StrExternalOrderId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Strorder_state);
                    SQLDataTableModel.SQLiteInsertUpdateDelete("Takeaways", SQL);
                    //---更新takeaways_order 訂單狀態
                }
                else
                {
                    blnResult = false;
                }
            }
            else
            {
                blnResult = false;
            }
            return blnResult;
        }

        public static VTSTORE_ordersinfo m_VTSTORE_ordersinfo = new VTSTORE_ordersinfo();
        public static bool ordersinfo(String Strorder_no)//訂單資料查詢
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if (Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String[] StrHeaderName = new String[] { "Authorization" };
                String[] StrHeaderValue = new String[] { "Bearer " + m_Straccess_token };
                String StrOutput = HttpsFun.RESTfulAPI_get(StrDomain, $"/api/vtstore/orders/info/{Strorder_no} ", "", StrHeaderName, StrHeaderValue);
                m_VTSTORE_ordersinfo = JsonClassConvert.VTSTORE_ordersinfo2Class(StrOutput);
                if ((m_VTSTORE_ordersinfo != null) && (m_VTSTORE_ordersinfo.status == "OK"))
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }
            }
            else
            {
                blnResult = false;
            }
            return blnResult;
        }
    }
}
