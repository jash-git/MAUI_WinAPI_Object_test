using Jint.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;


namespace VPOS
{
    public class SyncDBData//透過WEB API依序同步Cloude DB(MySQL) 到 SQLite
    {
        public static int m_intStep = 0;
        public static float m_intAllStep = 33;
        public static String m_StrEncoded = "";

        public static String JsonDataModified(String StrInput)
        {
            String StrResult = StrInput;

            StrResult = StrResult.Replace("\"set_sid\":null", "\"set_sid\":0");
            StrResult = StrResult.Replace("\"attribute_sid\":null", "\"attribute_sid\":0");
            StrResult = StrResult.Replace("\"product_sid\":null", "\"product_sid\":0");
            StrResult = StrResult.Replace("\"company_sid\":null", "\"company_sid\":0");
            StrResult = StrResult.Replace("\"category_sid\":null", "\"category_sid\":0");
            StrResult = StrResult.Replace("\"unit_sid\":null", "\"unit_sid\":0");
            StrResult = StrResult.Replace("\"tax_sid\":null", "\"tax_sid\":0");
            StrResult = StrResult.Replace("\"sort\":null", "\"sort\":0");
            StrResult = StrResult.Replace("\"stop_unix_time\":null", "\"stop_unix_time\":0");
            StrResult = StrResult.Replace("\"del_unix_time\":null", "\"del_unix_time\":0");
            StrResult = StrResult.Replace("\"condiment_update_unix_time\":null", "\"condiment_update_unix_time\":0");
            StrResult = StrResult.Replace("\"category_update_unix_time\":null", "\"category_update_unix_time\":0");
            StrResult = StrResult.Replace("\"price_update_unix_time\":null", "\"price_update_unix_time\":0");
            StrResult = StrResult.Replace("\"created_unix_time\":null", "\"created_unix_time\":0");
            StrResult = StrResult.Replace("\"updated_unix_time\":null", "\"updated_unix_time\":0");
            StrResult = StrResult.Replace("\"min_count\":null", "\"min_count\":0");
            StrResult = StrResult.Replace("\"max_count\":null", "\"max_count\":0");
            
            //StrResult = StrResult.Replace("null", "\"\"");//替換字串用
            
            return StrResult;
        }

        public static void Iint(String StrUserName, String StrPassword)
        {
            m_intStep = 0;
            m_StrEncoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(StrUserName + ":" + StrPassword));
        }

        public static void DBStructRegulating()//資料庫(DB) 新增資料表 / 新增(補)欄位 / 調整欄位資料型
        {
            //---
            //新增印表範本資料表 20221230
            String CreateTableString = SQLDataTableModel.VPOSInitialTableSyntax("printer_template");
            String StrDBFileName = LogFile.m_StrSysPath + "vpos.db";
            SQLDataTableModel.CreateSQLiteTable(StrDBFileName, CreateTableString);
            //---新增印表範本資料表 20221230

            SQLDataTableModel.DBColumnsPadding("terminal_data", "params", "TEXT", "null", false);//新增 終端設備資料表 params at 20221020
            SQLDataTableModel.DBColumnsPadding("order_data", "guests_num", "INT","0");
            SQLDataTableModel.DBColumnsPadding("basic_params", "source_type", "CHAR(10)");
            SQLDataTableModel.DBColumnsPadding("company", "def_params", "TEXT");//店家資料同步 20220621->新增回傳[def_params]欄位
            SQLDataTableModel.DBColumnsPadding("order_type_data", "params", "TEXT");//訂單類型 20220621->增加回傳[params]欄位
            SQLDataTableModel.DBColumnsPadding("order_coupon_data", "coupon_quantity", "INT", "0");//訂單優惠券欄位新增 20230207
            SQLDataTableModel.DBColumnsPadding("order_coupon_data", "coupon_val", "decimal(10, 2)","0");//訂單優惠券欄位新增 20230207
            SQLDataTableModel.DBColumnsPadding("daily_report", "category_sale_info", "TEXT");//報表 20230214->新增回傳[category_sale_info]欄位
            SQLDataTableModel.DBColumnsPadding("printer_data", "template_type", "varchar(20)");//報表 20230215->新增回傳[template_type]欄位
            SQLDataTableModel.DBColumnsPadding("printer_data", "template_sid", "varchar(64)");//報表 20230215->新增回傳[template_sid]欄位

            SQLDataTableModel.DBColumnsModifying("condiment_data", "condiment_code", "varchar(64)");//20230217
            SQLDataTableModel.DBColumnsModifying("condiment_data", "condiment_name", "varchar(50)");//20230217

            //SQLDataTableModel.DBColumnsPadding("daily_report", "easycard_checkout_info", "TEXT");//20230426
            SQLDataTableModel.DBColumnsPadding("daily_report", "promotions_info", "TEXT");//20230426
            //SQLDataTableModel.DBColumnsPadding("daily_report", "nccc_checkout_info", "TEXT");//20230509
            SQLDataTableModel.DBColumnsPadding("daily_report", "checkout_info", "TEXT");//20230511

            SQLDataTableModel.DBColumnsPadding("company_invoice_params", "booklet", "INT", "0");//20230524

            SQLDataTableModel.DBColumnsPadding("product_category", "display_flag", "CHAR(1)", "N", true);//20230627 ~ 商品類別依據設定[是否顯示]，決定POS端是否出現該類別
            SQLDataTableModel.DBColumnsPadding("product_data", "display_flag", "CHAR(1)", "N", true);//20230627 ~ 商品資料依據設定[是否顯示]，決定POS端是否出現        
        }


        //public static void main(object arg)//主函數
        //{
        //    ThreadWait d = (ThreadWait)arg;
        //    //Thread.Sleep(500);
        //    d.m_StrInput = String.Format("更新資料:{0}%;{1}", "組織資料",(int)(m_intStep/ m_intAllStep*100));//company

        //    //---
        //    //do something
        //    HttpsFun.setDomainMode(1);//POS
        //    get_company();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "基本共用參數", (int)(m_intStep / m_intAllStep * 100));//basic_params
        //    get_basic_params();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "終端功能清單", (int)(m_intStep / m_intAllStep * 100));//func_main
        //    get_terminal_func_main();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "終端角色", (int)(m_intStep / m_intAllStep * 100));//role_data
        //    get_terminal_roles();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "訂單類型資料", (int)(m_intStep / m_intAllStep * 100));//order_type_data
        //    get_order_type_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "支付模組參數", (int)(m_intStep / m_intAllStep * 100));//payment_module_params
        //    get_payment_module_params();
        //    get_easy_card_blacklist_info();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "組織支出類型", (int)(m_intStep / m_intAllStep * 100));//company_payment_type
        //    get_company_payment_type();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "電子發票平台", (int)(m_intStep / m_intAllStep * 100));//invoice_platform
        //    get_invoice_platform();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "組織發票參數", (int)(m_intStep / m_intAllStep * 100));//company_invoice_params
        //    get_company_invoice_params();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "組織自訂參數", (int)(m_intStep / m_intAllStep * 100));//company_customized_params
        //    get_company_customized_params();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "外賣平台", (int)(m_intStep / m_intAllStep * 100));//takeaways_platform
        //    get_takeaways_platform();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "外賣餐數", (int)(m_intStep / m_intAllStep * 100));//takeaways_params
        //    get_takeaways_params();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "會員平台參數", (int)(m_intStep / m_intAllStep * 100));//member_platform_params
        //    get_member_platform_params();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "終端使用者參數", (int)(m_intStep / m_intAllStep * 100));//user_data
        //    get_user_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "終端班別參數", (int)(m_intStep / m_intAllStep * 100));//class_data
        //    get_class_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "稅率資料", (int)(m_intStep / m_intAllStep * 100));//tax_data
        //    get_tax_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "商品單位", (int)(m_intStep / m_intAllStep * 100));//product_unit
        //    get_product_unit();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "價格類型", (int)(m_intStep / m_intAllStep * 100));//price_type
        //    get_price_type();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "配料群組", (int)(m_intStep / m_intAllStep * 100));//condiment_group
        //    get_condiment_group();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "配料資料", (int)(m_intStep / m_intAllStep * 100));//condiment_data
        //    get_condiment_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "商品資料", (int)(m_intStep / m_intAllStep * 100));//product_data
        //    get_product_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "商品類別", (int)(m_intStep / m_intAllStep * 100));//product_category
        //    get_product_category();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "產品規格數據", (int)(m_intStep / m_intAllStep * 100));//product_spec_data
        //    get_product_spec_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "套餐元素資料", (int)(m_intStep / m_intAllStep * 100));//set_attribute_data
        //    get_set_attribute_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "促銷活動", (int)(m_intStep / m_intAllStep * 100));//promotion_data
        //    get_promotion_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "桌位資料", (int)(m_intStep / m_intAllStep * 100));//store_table_data
        //    get_store_table_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "配方資料", (int)(m_intStep / m_intAllStep * 100));//formula_data
        //    get_formula_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "印表設備資料", (int)(m_intStep / m_intAllStep * 100));//printer_data
        //    get_printer_template();
        //    get_printer_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "印表群組資料", (int)(m_intStep / m_intAllStep * 100));//printer_group_data
        //    get_printer_group_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "包材種類", (int)(m_intStep / m_intAllStep * 100));//packaging_type
        //    get_packaging_type();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "包材資料", (int)(m_intStep / m_intAllStep * 100));//packaging_data
        //    get_packaging_data();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "優惠類型參數", (int)(m_intStep / m_intAllStep * 100));//discount_param
        //    get_discount_param();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "優惠快速鍵資料", (int)(m_intStep / m_intAllStep * 100));//discount_hotkey
        //    get_discount_hotkey();
        //    //d.Refresh();

        //    d.m_StrInput = String.Format("更新資料:{0};{1}", "支付科目資料", (int)(m_intStep / m_intAllStep * 100));//account_data
        //    get_account_data();
        //    //d.Refresh();
        //    //---do something

        //    String SQL = "";
        //    SQL = "DELETE FROM func_main WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM role_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM order_type_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    //SQL = "DELETE FROM payment_module WHERE del_flag='Y'";
        //    //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM payment_module_params WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM company_payment_type WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM member_platform_params WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM user_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM class_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM tax_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM product_unit WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM price_type WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM condiment_group WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM condiment_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM product_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM product_category WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM product_spec_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM promotion_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM store_table_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM printer_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM printer_group_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM packaging_type WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM packaging_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM discount_param WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM discount_hotkey WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
        //    SQL = "DELETE FROM account_data WHERE del_flag='Y'";
        //    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

        //    /*
        //     還有欄位要增加,先更新函數列表上面有備註
        //     1.8.5.8
        //     1.8.5.9
        //     */

        //    d.Invoke(new Action(d.Close));
        //}


        public static void get_account_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM account_data";
            DataTable account_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((account_dataDataTable != null) && (account_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = account_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/account_data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_account_data get_account_dataBuf = JsonClassConvert.get_account_data2Class(StrOutput);
            if ((get_account_dataBuf != null) && (get_account_dataBuf.data != null))
            {
                if (get_account_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_account_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO account_data (SID,account_code,account_name,type,sort,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ON CONFLICT(SID) DO UPDATE SET account_code='{1}',account_name='{2}',type='{3}',sort='{4}',stop_flag='{5}',del_flag='{6}',stop_time='{7}',del_time='{8}',created_time='{9}',updated_time='{10}'",
                            get_account_dataBuf.data[i].SID,
                            get_account_dataBuf.data[i].account_code,
                            get_account_dataBuf.data[i].account_name,
                            get_account_dataBuf.data[i].type,
                            get_account_dataBuf.data[i].sort,
                            get_account_dataBuf.data[i].stop_flag,
                            get_account_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_account_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_account_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_account_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_account_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_discount_hotkey()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM discount_hotkey";
            DataTable discount_hotkeyDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((discount_hotkeyDataTable != null) && (discount_hotkeyDataTable.Rows.Count > 0))
            {
                String StrMAXDate = discount_hotkeyDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/discount_area/hotkeys", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_discount_hotkey get_discount_hotkeyBuf = JsonClassConvert.get_discount_hotkey2Class(StrOutput);
            if ((get_discount_hotkeyBuf != null) && (get_discount_hotkeyBuf.data != null))
            {
                if (get_discount_hotkeyBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_discount_hotkeyBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO discount_hotkey (SID,hotkey_name,hotkey_code,discount_code,val_mode,val,round_calc_type,stop_flag,del_flag,sort,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ON CONFLICT(SID) DO UPDATE SET hotkey_name='{1}',hotkey_code='{2}',discount_code='{3}',val_mode='{4}',val='{5}',round_calc_type='{6}',stop_flag='{7}',del_flag='{8}',sort='{9}',stop_time='{10}',del_time='{11}',created_time='{12}',updated_time='{13}'",
                            get_discount_hotkeyBuf.data[i].hotkey_sid,
                            get_discount_hotkeyBuf.data[i].hotkey_name,
                            get_discount_hotkeyBuf.data[i].hotkey_code,
                            get_discount_hotkeyBuf.data[i].discount_code,
                            get_discount_hotkeyBuf.data[i].val_mode,
                            get_discount_hotkeyBuf.data[i].val,
                            get_discount_hotkeyBuf.data[i].round_calc_type,
                            get_discount_hotkeyBuf.data[i].stop_flag,
                            get_discount_hotkeyBuf.data[i].del_flag,
                            get_discount_hotkeyBuf.data[i].sort,
                        TimeConvert.UnixTimeStampToDateTime(get_discount_hotkeyBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_discount_hotkeyBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_discount_hotkeyBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_discount_hotkeyBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_discount_param()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM discount_param";
            DataTable discount_paramDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((discount_paramDataTable != null) && (discount_paramDataTable.Rows.Count > 0))
            {
                String StrMAXDate = discount_paramDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/discount_area/params", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_discount_param get_discount_paramBuf = JsonClassConvert.get_discount_param2Class(StrOutput);
            if ((get_discount_paramBuf != null) && (get_discount_paramBuf.data != null))
            {
                if (get_discount_paramBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_discount_paramBuf.data.Count; i++)
                    {
                        //---
                        //更新discount_param資料表資料
                        SQL = String.Format("INSERT INTO discount_param (SID,discount_code,filter_type,round_calc_type,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ON CONFLICT(SID) DO UPDATE SET discount_code='{1}',filter_type='{2}',round_calc_type='{3}',del_flag='{4}',del_time='{5}',created_time='{6}',updated_time='{7}'",
                            get_discount_paramBuf.data[i].param_sid,
                            get_discount_paramBuf.data[i].discount_code,
                            get_discount_paramBuf.data[i].filter_type,
                            get_discount_paramBuf.data[i].round_calc_type,
                            get_discount_paramBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_discount_paramBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_discount_paramBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_discount_paramBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        //---更新discount_param資料表資料

                        //---
                        //更新discount_product_relation資料表
                        SQL = $"DELETE FROM discount_product_relation WHERE discount_param_sid='{get_discount_paramBuf.data[i].param_sid}'";
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);//刪除就有關聯資料
                        
                        for(int j=0; j<get_discount_paramBuf.data[i].product_relation.Count;j++)
                        {
                            SQL = $"INSERT INTO discount_product_relation (discount_param_sid,product_sid) VALUES ('{get_discount_paramBuf.data[i].param_sid}', '{get_discount_paramBuf.data[i].product_relation[j].product_sid}')";
                            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        }
                        //---更新discount_product_relation資料表

                    }
                }
            }

            m_intStep++;
        }

        public static void get_packaging_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM packaging_data";
            DataTable packaging_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((packaging_dataDataTable != null) && (packaging_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = packaging_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/packaging/data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_packaging_data get_packaging_dataBuf = JsonClassConvert.get_packaging_data2Class(StrOutput);
            if ((get_packaging_dataBuf != null) && (get_packaging_dataBuf.data != null))
            {
                if (get_packaging_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_packaging_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO packaging_data (SID,packaging_type_sid,code,name,price,memo,sort,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ON CONFLICT(SID) DO UPDATE SET packaging_type_sid='{1}',code='{2}',name='{3}',price='{4}',memo='{5}',sort='{6}',del_flag='{7}',del_time='{8}',created_time='{9}',updated_time='{10}'",
                            get_packaging_dataBuf.data[i].SID,
                            get_packaging_dataBuf.data[i].packaging_type_sid,
                            get_packaging_dataBuf.data[i].code,
                            get_packaging_dataBuf.data[i].name,
                            get_packaging_dataBuf.data[i].price.ToString(),
                            get_packaging_dataBuf.data[i].memo,
                            get_packaging_dataBuf.data[i].sort,
                            get_packaging_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_packaging_type()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM packaging_type";
            DataTable packaging_typeDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((packaging_typeDataTable != null) && (packaging_typeDataTable.Rows.Count > 0))
            {
                String StrMAXDate = packaging_typeDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/packaging/type", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_packaging_type get_packaging_typeBuf = JsonClassConvert.get_packaging_type2Class(StrOutput);
            if ((get_packaging_typeBuf != null) && (get_packaging_typeBuf.data != null))
            {
                if (get_packaging_typeBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_packaging_typeBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO packaging_type (SID,name,sort,show_flag,required_flag,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ON CONFLICT(SID) DO UPDATE SET name='{1}',sort='{2}',show_flag='{3}',required_flag='{4}',del_flag='{5}',del_time='{6}',created_time='{7}',updated_time='{8}'",
                            get_packaging_typeBuf.data[i].SID,
                            get_packaging_typeBuf.data[i].name,
                            get_packaging_typeBuf.data[i].sort,
                            get_packaging_typeBuf.data[i].show_flag,
                            get_packaging_typeBuf.data[i].required_flag,
                            get_packaging_typeBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_typeBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_typeBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_packaging_typeBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_printer_group_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM printer_group_data";
            DataTable printer_group_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((printer_group_dataDataTable != null) && (printer_group_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = printer_group_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/printer/group", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_printer_group_data get_printer_group_dataBuf = JsonClassConvert.get_printer_group_data2Class(StrOutput);
            if ((get_printer_group_dataBuf != null) && (get_printer_group_dataBuf.data != null))
            {
                if (get_printer_group_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_printer_group_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO printer_group_data (SID,printer_group_name,printer_sid,order_type_sid,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ON CONFLICT(SID) DO UPDATE SET printer_group_name='{1}',printer_sid='{2}',order_type_sid='{3}',stop_flag='{4}',del_flag='{5}',stop_time='{6}',del_time='{7}',created_time='{8}',updated_time='{9}';",
                            get_printer_group_dataBuf.data[i].printer_group_sid,
                            get_printer_group_dataBuf.data[i].printer_group_name,
                            get_printer_group_dataBuf.data[i].printer_sid,
                            get_printer_group_dataBuf.data[i].order_type_sid,
                            get_printer_group_dataBuf.data[i].stop_flag,
                            get_printer_group_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_printer_group_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_group_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_group_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_group_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL += String.Format("DELETE FROM printer_group_relation WHERE printer_group_sid='{0}';", get_printer_group_dataBuf.data[i].printer_group_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        if ((get_printer_group_dataBuf.data[i].del_flag != "Y") && (get_printer_group_dataBuf.data[i].relation_data != null) && (get_printer_group_dataBuf.data[i].relation_data.Count > 0))
                        {
                            SQL = "";
                            for(int j = 0; j < get_printer_group_dataBuf.data[i].relation_data.Count;j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO printer_group_relation(printer_group_sid, product_sid) VALUES('{0}', '{1}')", get_printer_group_dataBuf.data[i].relation_data[j].printer_group_sid, get_printer_group_dataBuf.data[i].relation_data[j].product_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}', '{1}')", get_printer_group_dataBuf.data[i].relation_data[j].printer_group_sid, get_printer_group_dataBuf.data[i].relation_data[j].product_sid);
                                }
                            }
                            if (SQL.Length > 0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }

                        SQL = String.Format("DELETE FROM printer_group_order_type_relation WHERE printer_group_sid='{0}'", get_printer_group_dataBuf.data[i].printer_group_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        if ((get_printer_group_dataBuf.data[i].del_flag != "Y") && (get_printer_group_dataBuf.data[i].order_type_relation != null) && (get_printer_group_dataBuf.data[i].order_type_relation.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_printer_group_dataBuf.data[i].order_type_relation.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO printer_group_order_type_relation(printer_group_sid, order_type_sid) VALUES('{0}', '{1}')", get_printer_group_dataBuf.data[i].printer_group_sid, get_printer_group_dataBuf.data[i].order_type_relation[j].order_type_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}', '{1}')", get_printer_group_dataBuf.data[i].printer_group_sid, get_printer_group_dataBuf.data[i].order_type_relation[j].order_type_sid);
                                }
                            }
                            if(SQL.Length>0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }
                    }
                }
            }

            m_intStep++;
        }

        public static void get_printer_template()
        {
            String StrParams = "";
            String SQL = "";

            //---
            //清空資料表
            SQL = "DELETE FROM printer_template";
            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            //---清空資料表

            SQL = "SELECT MAX(updated_time) FROM printer_template";
            DataTable printer_templateDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((printer_templateDataTable != null) && (printer_templateDataTable.Rows.Count > 0))
            {
                String StrMAXDate = printer_templateDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/printer/template", StrParams, "Authorization", "Basic " + m_StrEncoded);

            get_printer_template get_printer_templateBuf = JsonClassConvert.get_printer_template2Class(StrOutput);

            if ((get_printer_templateBuf != null) && (get_printer_templateBuf.data != null))
            {
                if (get_printer_templateBuf.data.Count > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("SID", typeof(string));
                    table.Columns.Add("template_name", typeof(string));
                    table.Columns.Add("template_value", typeof(string));
                    table.Columns.Add("template_type", typeof(string));
                    table.Columns.Add("include_command", typeof(string));
                    table.Columns.Add("created_time", typeof(string));
                    table.Columns.Add("updated_time", typeof(string));
                    for (int i = 0; i < get_printer_templateBuf.data.Count; i++)
                    {
                        /*
                        SQL = String.Format("INSERT INTO printer_template (SID,template_name,template_value,template_type,include_command,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ON CONFLICT(SID) DO UPDATE SET template_name='{1}',template_value='{2}',template_type='{3}',include_command='{4}',created_time='{5}',updated_time='{6}';",
                        get_printer_templateBuf.data[i].template_sid,
                        get_printer_templateBuf.data[i].template_name,
                        get_printer_templateBuf.data[i].template_value,
                        get_printer_templateBuf.data[i].template_type,
                        get_printer_templateBuf.data[i].include_command,
                        TimeConvert.UnixTimeStampToDateTime(get_printer_templateBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_templateBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        */
                        String Strtemplate_value = get_printer_templateBuf.data[i].template_value;
                        //Strtemplate_value = Strtemplate_value.Replace("\\r\\n", "\r\n");
                        Strtemplate_value = Strtemplate_value.Replace("\\t", "\t");
                        Strtemplate_value = Strtemplate_value.Replace("{\"code\":\"", "");
                        Strtemplate_value = Strtemplate_value.Replace("\",\"lang\":\"javascript\",\"theme\":\"twilight\"}", "");
                        
                        table.Rows.Add(get_printer_templateBuf.data[i].template_sid,
                        get_printer_templateBuf.data[i].template_name,
                        Strtemplate_value,
                        get_printer_templateBuf.data[i].template_type,
                        get_printer_templateBuf.data[i].include_command,
                        TimeConvert.UnixTimeStampToDateTime(get_printer_templateBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_templateBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    SQL = "INSERT INTO printer_template (SID,template_name,template_value,template_type,include_command,created_time,updated_time) VALUES(@SID,@template_name,@template_value,@template_type,@include_command,@created_time,@updated_time) ON CONFLICT(SID) DO UPDATE SET template_name=@template_name,template_value=@template_value,template_type=@template_type,include_command=@include_command,created_time=@created_time,updated_time=@updated_time;";
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL, table);
                }
            }

        }
        public static void get_printer_data()
        {
            String StrParams = "";
            String SQL = "";

            //---
            //清空資料表
            //SQL = "DELETE FROM printer_data";
            //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            //---清空資料表

            SQL = "SELECT MAX(updated_time) FROM printer_data";
            DataTable printer_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((printer_dataDataTable != null) && (printer_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = printer_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/printer/data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_printer_data get_printer_dataBuf = JsonClassConvert.get_printer_data2Class(StrOutput);
            if ((get_printer_dataBuf != null) && (get_printer_dataBuf.data != null))
            {
                if (get_printer_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_printer_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO printer_data (SID,printer_code,printer_name,output_type,stop_flag,del_flag,stop_time,del_time,created_time,updated_time,template_type,template_sid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ON CONFLICT(SID) DO UPDATE SET printer_code='{1}',printer_name='{2}',output_type='{3}',stop_flag='{4}',del_flag='{5}',stop_time='{6}',del_time='{7}',created_time='{8}',updated_time='{9}',template_type='{10}',template_sid='{11}';",
                            get_printer_dataBuf.data[i].printer_sid,
                            get_printer_dataBuf.data[i].printer_code,
                            get_printer_dataBuf.data[i].printer_name,
                            get_printer_dataBuf.data[i].output_type,
                            get_printer_dataBuf.data[i].stop_flag,
                            get_printer_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_printer_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_printer_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        get_printer_dataBuf.data[i].template_type,
                        get_printer_dataBuf.data[i].template_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            //---
            //手動增加 CASH_BOX 資料
            SQL = "SELECT SID,template_type  FROM printer_template WHERE template_type='CASH_BOX'";
            DataTable printer_template = SQLDataTableModel.GetDataTable(SQL);
            String Strtemplate_sid = "";
            String Strtemplate_type = "";
            if ((printer_template != null) && (printer_template.Rows.Count > 0))
            {
                Strtemplate_sid = printer_template.Rows[0][0].ToString();
                Strtemplate_type = printer_template.Rows[0][1].ToString();
            }

            SQL = String.Format("INSERT INTO printer_data (SID,printer_code,printer_name,output_type,stop_flag,del_flag,stop_time,del_time,created_time,updated_time,template_sid,template_type) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ON CONFLICT(SID) DO UPDATE SET printer_code='{1}',printer_name='{2}',output_type='{3}',stop_flag='{4}',del_flag='{5}',stop_time='{6}',del_time='{7}',created_time='{8}',updated_time='{9}',template_sid='{10}',template_type='{11}';",
                0,
                "CASH_BOX",
                "錢箱",
                "X",
                "N",
                "N",
                "1970-01-01 08:00:00.000",
                "1970-01-01 08:00:00.000",
                "1970-01-01 08:00:00.000",
                "1970-01-01 08:00:00.000",
                Strtemplate_sid,
                Strtemplate_type);
            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            //---手動增加 CASH_BOX 資料
            m_intStep++;
        }

        public static void get_formula_data()
        {
            String SQL = "";
            
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/formula/data", "", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            StrOutput = StrOutput.Replace("{\"status\":\"OK\",\"message\":\"\",\"data\":","");
            StrOutput = StrOutput.Substring(0, StrOutput.Length - 1);
            StrOutput=System.Text.RegularExpressions.Regex.Unescape(StrOutput);// \u -> 中文
            if ( (StrOutput.Length>0) && (StrOutput.Contains("formula_data")))
            {
                SQL = "DELETE FROM formula_data";
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                SQL = String.Format("INSERT INTO formula_data (SID,formula_params,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}')",
                    1, StrOutput, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            }
            m_intStep++;
        }

        public static void get_store_table_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM store_table_data";
            DataTable store_table_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((store_table_dataDataTable != null) && (store_table_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = store_table_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/store_table/data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_store_table_data get_store_table_dataBuf = JsonClassConvert.get_store_table_data2Class(StrOutput);
            if ((get_store_table_dataBuf != null) && (get_store_table_dataBuf.data != null))
            {
                if (get_store_table_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_store_table_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO store_table_data (SID,table_code,table_name,table_capacity,table_sort,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ON CONFLICT(SID) DO UPDATE SET table_code='{1}',table_name='{2}',table_capacity='{3}',table_sort='{4}',stop_flag='{5}',del_flag='{6}',stop_time='{7}',del_time='{8}',created_time='{9}',updated_time='{10}'",
                            get_store_table_dataBuf.data[i].table_sid,
                            get_store_table_dataBuf.data[i].table_code,
                            get_store_table_dataBuf.data[i].table_name,
                            get_store_table_dataBuf.data[i].table_capacity,
                            get_store_table_dataBuf.data[i].table_sort,
                            get_store_table_dataBuf.data[i].stop_flag,
                            get_store_table_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_store_table_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_store_table_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_store_table_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_store_table_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_promotion_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM promotion_data";
            DataTable get_promotion_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((get_promotion_dataDataTable != null) && (get_promotion_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = get_promotion_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/promotions/all_data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = StrOutput.Replace("\"del_time\":null", "\"del_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_promotion_data get_promotion_dataBuf = JsonClassConvert.get_promotion_data2Class(StrOutput);
            if ((get_promotion_dataBuf != null) && (get_promotion_dataBuf.data != null))
            {
                if (get_promotion_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_promotion_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO promotion_data (SID,company_sid,promotion_name,promotion_start_time,promotion_end_time,promotion_sort,coexist_flag,promotion_type,promotion_data,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') ON CONFLICT(SID,company_sid) DO UPDATE SET promotion_name='{2}',promotion_start_time='{3}',promotion_end_time='{4}',promotion_sort='{5}',coexist_flag='{6}',promotion_type='{7}',promotion_data='{8}',stop_flag='{9}',del_flag='{10}',stop_time='{11}',del_time='{12}',created_time='{13}',updated_time='{14}';",
                            get_promotion_dataBuf.data[i].promotion_sid,
                            get_promotion_dataBuf.data[i].company_sid,
                            get_promotion_dataBuf.data[i].promotion_name,
                            get_promotion_dataBuf.data[i].promotion_start_time,
                            get_promotion_dataBuf.data[i].promotion_end_time,
                            get_promotion_dataBuf.data[i].promotion_sort,
                            get_promotion_dataBuf.data[i].coexist_flag,
                            get_promotion_dataBuf.data[i].promotion_type,
                            get_promotion_dataBuf.data[i].promotion_data,
                            get_promotion_dataBuf.data[i].stop_flag,
                            get_promotion_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_promotion_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_promotion_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_promotion_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_promotion_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL += String.Format("DELETE FROM product_promotion_relation WHERE promotion_sid = '{0}';", get_promotion_dataBuf.data[i].promotion_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        if ((get_promotion_dataBuf.data[i].del_flag != "Y") && (get_promotion_dataBuf.data[i].product_list != null) && (get_promotion_dataBuf.data[i].product_list.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_promotion_dataBuf.data[i].product_list.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO product_promotion_relation (promotion_sid ,product_sid) VALUES ('{0}','{1}')", get_promotion_dataBuf.data[i].product_list[j].promotion_sid, get_promotion_dataBuf.data[i].product_list[j].product_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}')", get_promotion_dataBuf.data[i].product_list[j].promotion_sid, get_promotion_dataBuf.data[i].product_list[j].product_sid);
                                }
                            }

                            if(SQL.Length>0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }                 
                        }

                        SQL = String.Format("DELETE FROM promotion_order_type_relation WHERE promotion_sid = '{0}'", get_promotion_dataBuf.data[i].promotion_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        if ((get_promotion_dataBuf.data[i].del_flag != "Y") && (get_promotion_dataBuf.data[i].assign_order_type != null) && (get_promotion_dataBuf.data[i].assign_order_type.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_promotion_dataBuf.data[i].assign_order_type.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO promotion_order_type_relation (promotion_sid ,order_type_sid) VALUES ('{0}','{1}')", get_promotion_dataBuf.data[i].promotion_sid, get_promotion_dataBuf.data[i].assign_order_type[j].order_type_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}')", get_promotion_dataBuf.data[i].promotion_sid, get_promotion_dataBuf.data[i].assign_order_type[j].order_type_sid);
                                }
                            }

                            if (SQL.Length > 0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }
                    }
                }
            }

            m_intStep++;
        }

        public static void get_set_attribute_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM set_attribute_data";
            DataTable set_attribute_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((set_attribute_dataDataTable != null) && (set_attribute_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = set_attribute_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/set_attribute", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_set_attribute_data get_set_attribute_dataBuf = JsonClassConvert.get_set_attribute_data2Class(StrOutput);
            if ((get_set_attribute_dataBuf != null) && (get_set_attribute_dataBuf.data != null))
            {
                if (get_set_attribute_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_set_attribute_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO set_attribute_data (SID,set_sid,attribute_name,main_price_type,main_price,main_max_price,sub_price_type,sub_price,sub_max_price,required_flag,limit_count,repeat_flag,sort,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') ON CONFLICT(SID, set_sid) DO UPDATE SET attribute_name='{2}',main_price_type='{3}',main_price='{4}',main_max_price='{5}',sub_price_type='{6}',sub_price='{7}',sub_max_price='{8}',required_flag='{9}',limit_count='{10}',repeat_flag='{11}',sort='{12}',created_time='{13}',updated_time='{14}';",
                            get_set_attribute_dataBuf.data[i].attribute_sid,
                            get_set_attribute_dataBuf.data[i].set_sid,
                            get_set_attribute_dataBuf.data[i].attribute_name,
                            get_set_attribute_dataBuf.data[i].attribute_main_price_type,
                            get_set_attribute_dataBuf.data[i].attribute_main_price,
                            get_set_attribute_dataBuf.data[i].attribute_main_max_price,
                            get_set_attribute_dataBuf.data[i].attribute_price_type,
                            get_set_attribute_dataBuf.data[i].attribute_price,
                            get_set_attribute_dataBuf.data[i].attribute_max_price,
                            get_set_attribute_dataBuf.data[i].required_flag,
                            get_set_attribute_dataBuf.data[i].attribute_count, 
                            get_set_attribute_dataBuf.data[i].repeat_flag,
                            get_set_attribute_dataBuf.data[i].sort,
                        TimeConvert.UnixTimeStampToDateTime(get_set_attribute_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_set_attribute_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL += String.Format("DELETE FROM product_set_relation WHERE (attribute_sid = '{0}') AND (set_sid='{1}');", get_set_attribute_dataBuf.data[i].attribute_sid, get_set_attribute_dataBuf.data[i].set_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                    }

                    for (int i = 0; i < get_set_attribute_dataBuf.data.Count; i++)
                    {
                        if ((get_set_attribute_dataBuf.data[i].product_set_relation != null) && (get_set_attribute_dataBuf.data[i].product_set_relation.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_set_attribute_dataBuf.data[i].product_set_relation.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO product_set_relation (set_sid,attribute_sid,category_sid,product_sid,main_flag,default_flag) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].set_sid,
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].attribute_sid,
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].category_sid,
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].product_sid,
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].main_flag,
                                        get_set_attribute_dataBuf.data[i].product_set_relation[j].default_flag);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].set_sid,
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].attribute_sid,
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].category_sid,
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].product_sid,
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].main_flag,
                                                                            get_set_attribute_dataBuf.data[i].product_set_relation[j].default_flag);
                                }
                            }
                            if(SQL.Length>0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }

                    }
                }
            }

            m_intStep++;
        }

        public static void get_product_spec_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM product_spec_data";
            DataTable product_spec_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((product_spec_dataDataTable != null) && (product_spec_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = product_spec_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/spec", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_product_spec_data get_product_spec_dataBuf = JsonClassConvert.get_product_spec_data2Class(StrOutput);
            if ((get_product_spec_dataBuf != null) && (get_product_spec_dataBuf.data != null))
            {
                if (get_product_spec_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_product_spec_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO product_spec_data (SID,spec_name,init_product_sid,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ON CONFLICT(SID) DO UPDATE SET spec_name='{1}',init_product_sid='{2}',del_flag='{3}',del_time='{4}',created_time='{5}',updated_time='{6}';",
                            get_product_spec_dataBuf.data[i].spec_sid,
                            get_product_spec_dataBuf.data[i].spec_name,
                            get_product_spec_dataBuf.data[i].init_product_sid,
                            get_product_spec_dataBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_product_spec_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_product_spec_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_product_spec_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL += String.Format("DELETE FROM product_spec_relation WHERE spec_sid = '{0}';", get_product_spec_dataBuf.data[i].spec_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }

                    for (int i = 0; i < get_product_spec_dataBuf.data.Count; i++)
                    {
                        SQL = "";
                        if ((get_product_spec_dataBuf.data[i].del_flag != "Y") && (get_product_spec_dataBuf.data[i].spec_relation_data != null) && (get_product_spec_dataBuf.data[i].spec_relation_data.Count > 0))
                        {
                            for (int j = 0; j < get_product_spec_dataBuf.data[i].spec_relation_data.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO product_spec_relation (spec_sid,product_sid,alias_name,sort) VALUES('{0}','{1}','{2}','{3}')",
                                        get_product_spec_dataBuf.data[i].spec_sid,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].product_sid,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].alias_name,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].sort);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}','{2}','{3}')",
                                        get_product_spec_dataBuf.data[i].spec_sid,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].product_sid,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].alias_name,
                                        get_product_spec_dataBuf.data[i].spec_relation_data[j].sort);
                                }            
                            }
                        }
                        if(SQL.Length>0)
                        {
                            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        }
                    }
                }
            }

            m_intStep++;
        }

        public static void get_product_category()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM product_category";
            DataTable product_categoryDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((product_categoryDataTable != null) && (product_categoryDataTable.Rows.Count > 0))
            {
                String StrMAXDate = product_categoryDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/category", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_products_category get_products_categoryBuf = JsonClassConvert.get_products_category2Class(StrOutput);
            if ((get_products_categoryBuf != null) && (get_products_categoryBuf.data != null))
            {
                if (get_products_categoryBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_products_categoryBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO product_category (SID,company_sid,category_code,category_name,sort,stop_flag,del_flag,stop_time,del_time,created_time,updated_time,display_flag) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ON CONFLICT(SID,company_sid) DO UPDATE SET category_code='{2}',category_name='{3}',sort='{4}',stop_flag='{5}',del_flag='{6}',stop_time='{7}',del_time='{8}',created_time='{9}',updated_time='{10}',display_flag='{11}';",
                            get_products_categoryBuf.data[i].category_sid,
                            get_products_categoryBuf.data[i].company_sid,
                            get_products_categoryBuf.data[i].category_code,
                            get_products_categoryBuf.data[i].category_name,
                            get_products_categoryBuf.data[i].sort,
                            get_products_categoryBuf.data[i].stop_flag,
                            get_products_categoryBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_products_categoryBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_categoryBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_categoryBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_categoryBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            get_products_categoryBuf.data[i].display_flag);
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL += String.Format("DELETE FROM product_category_relation WHERE category_sid = '{0}';", get_products_categoryBuf.data[i].category_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                    }

                    for (int i = 0; i < get_products_categoryBuf.data.Count; i++)
                    {
                        if ((get_products_categoryBuf.data[i].del_flag != "Y") && (get_products_categoryBuf.data[i].product_relation_data != null) && (get_products_categoryBuf.data[i].product_relation_data.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_products_categoryBuf.data[i].product_relation_data.Count; j++)
                            {
                                if(j==0)
                                {
                                    SQL = String.Format("INSERT INTO product_category_relation (category_sid,product_sid) VALUES('{0}','{1}')", get_products_categoryBuf.data[i].category_sid, get_products_categoryBuf.data[i].product_relation_data[j].product_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}')", get_products_categoryBuf.data[i].category_sid, get_products_categoryBuf.data[i].product_relation_data[j].product_sid);
                                }
                            }
                            if(SQL.Length>0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }               
                        }

                    }
                }
            }

            m_intStep++;
        }

        public static void get_product_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM product_data";
            DataTable product_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((product_dataDataTable != null) && (product_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = product_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/data", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_products_data get_products_dataBuf = JsonClassConvert.get_products_data2Class(StrOutput);
            if ((get_products_dataBuf != null) && (get_products_dataBuf.data != null))
            {
                if (get_products_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_products_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO product_data (SID,company_sid,product_code,barcode,product_name,product_shortname,product_type,price_mode,product_price,unit_sid,tax_sid,sort,memo,stop_flag,del_flag,stop_time,del_time,condiment_update_time,category_update_time,price_update_time,created_time,updated_time,display_flag) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}') ON CONFLICT(SID,company_sid) DO UPDATE SET product_code='{2}',barcode='{3}',product_name='{4}',product_shortname='{5}',product_type='{6}',price_mode='{7}',product_price='{8}',unit_sid='{9}',tax_sid='{10}',sort='{11}',memo='{12}',stop_flag='{13}',del_flag='{14}',stop_time='{15}',del_time='{16}',condiment_update_time='{17}',category_update_time='{18}',price_update_time='{19}',  created_time='{20}',updated_time='{21}',display_flag='{22}';",
                            get_products_dataBuf.data[i].product_sid,
                            get_products_dataBuf.data[i].company_sid,
                            get_products_dataBuf.data[i].product_code,
                            get_products_dataBuf.data[i].product_barcode,
                            get_products_dataBuf.data[i].product_name,
                            get_products_dataBuf.data[i].product_shortname,
                            get_products_dataBuf.data[i].product_type,
                            get_products_dataBuf.data[i].price_mode.ToString(),
                            get_products_dataBuf.data[i].product_price,
                            get_products_dataBuf.data[i].unit_sid,
                            get_products_dataBuf.data[i].tax_sid,
                            get_products_dataBuf.data[i].sort,
                            get_products_dataBuf.data[i].memo,
                            get_products_dataBuf.data[i].stop_flag,
                            get_products_dataBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].condiment_update_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].category_update_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].price_update_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            get_products_dataBuf.data[i].display_flag);
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        
                        SQL += String.Format("DELETE FROM product_condiment_relation WHERE product_sid = '{0}';", get_products_dataBuf.data[i].product_sid);
                        //SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        
                        SQL += String.Format("DELETE FROM product_price_type_relation WHERE product_sid = '{0}';", get_products_dataBuf.data[i].product_sid);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        
                    }

                    //*
                    for (int i = 0; i < get_products_dataBuf.data.Count; i++)
                    {
                        if ((get_products_dataBuf.data[i].del_flag != "Y") && (get_products_dataBuf.data[i].condiment_relation != null) && (get_products_dataBuf.data[i].condiment_relation.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_products_dataBuf.data[i].condiment_relation.Count; j++)
                            {
                                if (j == 0)
                                {
                                    SQL += String.Format("INSERT INTO product_condiment_relation (product_sid,condiment_sid) VALUES('{0}','{1}')", get_products_dataBuf.data[i].product_sid, get_products_dataBuf.data[i].condiment_relation[j].condiment_sid);
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}')", get_products_dataBuf.data[i].product_sid, get_products_dataBuf.data[i].condiment_relation[j].condiment_sid);
                                }
                            }

                            if (SQL.Length > 0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }

                        if ((get_products_dataBuf.data[i].del_flag != "Y") && (get_products_dataBuf.data[i].price_type_relation != null) && (get_products_dataBuf.data[i].price_type_relation.Count > 0))
                        {
                            SQL = "";
                            for (int j = 0; j < get_products_dataBuf.data[i].price_type_relation.Count; j++)
                            {
                                if (j == 0)
                                {
                                    SQL += String.Format("INSERT INTO product_price_type_relation (product_sid,price_type_sid,price,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}')",
                                        get_products_dataBuf.data[i].product_sid,
                                        get_products_dataBuf.data[i].price_type_relation[j].price_type_sid,
                                        get_products_dataBuf.data[i].price_type_relation[j].price.ToString(),
                                    TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                    TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].price_update_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                }
                                else
                                {
                                    SQL += String.Format(",('{0}','{1}','{2}','{3}','{4}')",
                                        get_products_dataBuf.data[i].product_sid,
                                        get_products_dataBuf.data[i].price_type_relation[j].price_type_sid,
                                        get_products_dataBuf.data[i].price_type_relation[j].price.ToString(),
                                    TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                    TimeConvert.UnixTimeStampToDateTime(get_products_dataBuf.data[i].price_update_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                }
                            }

                            if (SQL.Length > 0)
                            {
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }
                    }
                    //*/
                }
            }

            m_intStep++;
        }

        public static void get_condiment_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM condiment_data";
            DataTable condiment_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((condiment_dataDataTable != null) && (condiment_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = condiment_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/condiments/info", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = StrOutput.Replace("\"unit_sid\":null", "\"unit_sid\":0");
            StrOutput = StrOutput.Replace("\"stop_time\":null", "\"stop_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_condiment_data get_condiment_dataBuf = JsonClassConvert.get_condiment_data2Class(StrOutput);
            if ((get_condiment_dataBuf != null) && (get_condiment_dataBuf.data != null))
            {
                if (get_condiment_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_condiment_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO condiment_data (SID,company_sid,condiment_code,condiment_name,condiment_price,unit_sid,group_sid,sort,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ON CONFLICT(SID) DO UPDATE SET condiment_code='{2}',condiment_name='{3}',condiment_price='{4}',unit_sid='{5}',group_sid='{6}',sort='{7}',stop_flag='{8}',del_flag='{9}',stop_time='{10}',del_time='{11}',created_time='{12}',updated_time='{13}'",
                            get_condiment_dataBuf.data[i].condiment_sid,
                            get_condiment_dataBuf.data[i].company_sid,
                            get_condiment_dataBuf.data[i].condiment_code,
                            get_condiment_dataBuf.data[i].condiment_name,
                            get_condiment_dataBuf.data[i].condiment_price,
                            get_condiment_dataBuf.data[i].unit_sid,
                            get_condiment_dataBuf.data[i].group_sid,
                            get_condiment_dataBuf.data[i].sort,
                            get_condiment_dataBuf.data[i].stop_flag,
                            get_condiment_dataBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_condiment_dataBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_condiment_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_condiment_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_condiment_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_condiment_group()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM condiment_group";
            DataTable condiment_groupDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((condiment_groupDataTable != null) && (condiment_groupDataTable.Rows.Count > 0))
            {
                String StrMAXDate = condiment_groupDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/condiments/group", StrParams, "Authorization", "Basic " + m_StrEncoded);
            //StrOutput = StrOutput.Replace("\"min_count\":null", "\"min_count\":0");
            //StrOutput = StrOutput.Replace("\"max_count\":null", "\"max_count\":0");
            //StrOutput = StrOutput.Replace("\"sort\":null", "\"sort\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_condiment_group get_condiment_groupBuf = JsonClassConvert.get_condiment_group2Class(StrOutput);
            if ((get_condiment_groupBuf != null) && (get_condiment_groupBuf.data != null))
            {
                if (get_condiment_groupBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_condiment_groupBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO condiment_group (SID,company_sid,group_name,required_flag,single_flag,newline_flag,count_flag,min_count,max_count,sort,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ON CONFLICT(SID,company_sid) DO UPDATE SET group_name='{2}',required_flag='{3}',single_flag='{4}',newline_flag='{5}',count_flag='{6}',min_count='{7}',max_count='{8}',sort='{9}',del_flag='{10}',del_time='{11}',created_time='{12}',updated_time='{13}';",
                            get_condiment_groupBuf.data[i].group_sid,
                            get_condiment_groupBuf.data[i].company_sid,
                            get_condiment_groupBuf.data[i].group_name,
                            get_condiment_groupBuf.data[i].required_flag,
                            get_condiment_groupBuf.data[i].single_flag,
                            get_condiment_groupBuf.data[i].newline_flag,
                            get_condiment_groupBuf.data[i].count_flag,
                            get_condiment_groupBuf.data[i].min_count,
                            get_condiment_groupBuf.data[i].max_count,
                            get_condiment_groupBuf.data[i].sort,
                            get_condiment_groupBuf.data[i].del_flag,
                        TimeConvert.UnixTimeStampToDateTime(get_condiment_groupBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_condiment_groupBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        TimeConvert.UnixTimeStampToDateTime(get_condiment_groupBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_price_type()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM price_type";
            DataTable price_typeDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((price_typeDataTable != null) && (price_typeDataTable.Rows.Count > 0))
            {
                String StrMAXDate = price_typeDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/price/get_price_type", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_price_type get_price_typeBuf = JsonClassConvert.get_price_type2Class(StrOutput);
            if ((get_price_typeBuf != null) && (get_price_typeBuf.data != null))
            {
                if (get_price_typeBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_price_typeBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO price_type (price_type_sid,company_sid,price_type_name,del_flag,stop_flag,del_time,stop_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ON CONFLICT(company_sid,price_type_sid) DO UPDATE SET price_type_name='{2}',del_flag='{3}',stop_flag='{4}',del_time='{5}',stop_time='{6}',created_time='{7}',updated_time='{8}'",
                            get_price_typeBuf.data[i].price_type_sid,
                            get_price_typeBuf.data[i].company_sid,
                            get_price_typeBuf.data[i].price_type_name.ToString(),
                            get_price_typeBuf.data[i].del_flag,
                            get_price_typeBuf.data[i].stop_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_price_typeBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_price_typeBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_price_typeBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_price_typeBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_product_unit()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM product_unit";
            DataTable product_unitDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((product_unitDataTable != null) && (product_unitDataTable.Rows.Count > 0))
            {
                String StrMAXDate = product_unitDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/unit", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_product_unit get_product_unitBuf = JsonClassConvert.get_product_unit2Class(StrOutput);
            if ((get_product_unitBuf != null) && (get_product_unitBuf.data != null))
            {
                if (get_product_unitBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_product_unitBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO product_unit (SID,company_sid,unit_name,sort,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ON CONFLICT(SID,company_sid) DO UPDATE SET unit_name='{2}',sort='{3}',del_flag='{4}',del_time='{5}',created_time='{6}',updated_time='{7}'",
                            get_product_unitBuf.data[i].product_unit_sid,
                            get_product_unitBuf.data[i].company_sid,
                            get_product_unitBuf.data[i].unit_name,
                            get_product_unitBuf.data[i].sort,
                            get_product_unitBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_product_unitBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_product_unitBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_product_unitBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;

        }

        public static void get_tax_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM tax_data";
            DataTable tax_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((tax_dataDataTable != null) && (tax_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = tax_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/tax", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_tax_data get_tax_dataBuf = JsonClassConvert.get_tax_data2Class(StrOutput);
            if ((get_tax_dataBuf != null) && (get_tax_dataBuf.data != null))
            {
                if (get_tax_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_tax_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO tax_data (SID,company_sid,tax_name,tax_rate,tax_type,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ON CONFLICT(SID,company_sid) DO UPDATE SET tax_name='{2}',tax_rate='{3}',tax_type='{4}',del_flag='{5}',del_time='{6}',created_time='{7}',updated_time='{8}'",
                            get_tax_dataBuf.data[i].tax_sid,
                            get_tax_dataBuf.data[i].company_sid,
                            get_tax_dataBuf.data[i].tax_name,
                            get_tax_dataBuf.data[i].tax_rate,
                            get_tax_dataBuf.data[i].tax_type,
                            get_tax_dataBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_tax_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_tax_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_tax_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_class_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM class_data";
            DataTable class_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((class_dataDataTable != null) && (class_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = class_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/terminal/class", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_class_data get_class_dataBuf = JsonClassConvert.get_class_data2Class(StrOutput);
            if ((get_class_dataBuf != null) && (get_class_dataBuf.data != null))
            {
                if (get_class_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_class_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO class_data (SID,class_name,time_start,time_end,sort,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ON CONFLICT(SID) DO UPDATE SET class_name='{1}',time_start='{2}',time_end='{3}',sort='{4}',del_flag='{5}',del_time='{6}',created_time='{7}',updated_time='{8}'",
                            get_class_dataBuf.data[i].class_sid,
                            get_class_dataBuf.data[i].class_name,
                            get_class_dataBuf.data[i].time_start,
                            get_class_dataBuf.data[i].time_end,
                            get_class_dataBuf.data[i].sort,
                            get_class_dataBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_class_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_class_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_class_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_user_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM user_data";
            DataTable user_dataDataTable = SQLDataTableModel.GetDataTable(SQL);


            if ((user_dataDataTable != null) && (user_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = user_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/terminal/users", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_user_data get_user_dataBuf = JsonClassConvert.get_user_data2Class(StrOutput);
            if ((get_user_dataBuf != null) && (get_user_dataBuf.data != null))
            {
                if (get_user_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_user_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO user_data (SID,company_sid,role_sid,user_account,user_pwd,user_name,employee_no,job_title,tel,cellphone,state_flag,del_flag,state_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}') ON CONFLICT(SID) DO UPDATE SET company_sid='{1}',role_sid='{2}',user_account='{3}',user_pwd='{4}',user_name='{5}',employee_no='{6}',job_title='{7}',tel='{8}',cellphone='{9}',state_flag='{10}',del_flag='{11}',state_time='{12}',del_time='{13}',created_time='{14}',updated_time='{15}'",
                            get_user_dataBuf.data[i].user_sid,
                            get_user_dataBuf.data[i].company_sid,
                            get_user_dataBuf.data[i].role_sid,
                            get_user_dataBuf.data[i].user_account,
                            get_user_dataBuf.data[i].user_pwd,
                            get_user_dataBuf.data[i].user_name,
                            get_user_dataBuf.data[i].employee_no,
                            get_user_dataBuf.data[i].job_title,
                            get_user_dataBuf.data[i].tel,
                            get_user_dataBuf.data[i].cellphone,
                            get_user_dataBuf.data[i].state_flag,
                            get_user_dataBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_user_dataBuf.data[i].state_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_user_dataBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_user_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_user_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);//state_time, del_time, created_time, updated_time
                    }
                }
            }

            m_intStep++;
        }

        public static void get_member_platform_params()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM member_platform_params";
            DataTable member_platform_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((member_platform_paramsDataTable != null) && (member_platform_paramsDataTable.Rows.Count > 0))
            {
                String StrMAXDate = member_platform_paramsDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/member/platform_params", StrParams, "Authorization", "Basic " + m_StrEncoded);
            //StrOutput = StrOutput.Replace("\"del_unix_time\":null", "\"del_unix_time\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_member_platform_params get_member_platform_paramsBuf = JsonClassConvert.get_member_platform_params2Class(StrOutput);
            if ((get_member_platform_paramsBuf != null) && (get_member_platform_paramsBuf.data != null))
            {
                if (get_member_platform_paramsBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_member_platform_paramsBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO member_platform_params (SID,platform_type,params,sort,stop_flag,del_flag,stop_time,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ON CONFLICT(SID) DO UPDATE SET platform_type='{1}',params='{2}',sort='{3}',stop_flag='{4}',del_flag='{5}',stop_time='{6}',del_time='{7}',created_time='{8}',updated_time='{9}';",
                            get_member_platform_paramsBuf.data[i].SID,
                            get_member_platform_paramsBuf.data[i].platform_type,
                            get_member_platform_paramsBuf.data[i].@params,
                            get_member_platform_paramsBuf.data[i].sort,
                            get_member_platform_paramsBuf.data[i].stop_flag,
                            get_member_platform_paramsBuf.data[i].del_flag,
                            TimeConvert.UnixTimeStampToDateTime(get_member_platform_paramsBuf.data[i].stop_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_member_platform_paramsBuf.data[i].del_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_member_platform_paramsBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_member_platform_paramsBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_takeaways_params()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM takeaways_params";
            DataTable takeaways_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((takeaways_paramsDataTable != null) && (takeaways_paramsDataTable.Rows.Count > 0))
            {
                String StrMAXDate = takeaways_paramsDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/takeaways/params/sync", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_takeaways_params get_takeaways_paramsBuf = JsonClassConvert.get_takeaways_params2Class(StrOutput);
            if ((get_takeaways_paramsBuf != null) && (get_takeaways_paramsBuf.data != null))
            {
                if (get_takeaways_paramsBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_takeaways_paramsBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO takeaways_params (platform_sid,active_state,params,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}') ON CONFLICT(platform_sid) DO UPDATE SET active_state='{1}',params='{2}',created_time='{3}',updated_time='{4}'",
                            get_takeaways_paramsBuf.data[i].platform_sid,
                            get_takeaways_paramsBuf.data[i].active_state,
                            get_takeaways_paramsBuf.data[i].@params,
                            TimeConvert.UnixTimeStampToDateTime(get_takeaways_paramsBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_takeaways_paramsBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_takeaways_platform()
        {
            String SQL = "";
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/takeaways/platform", "", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_takeaways_platform get_takeaways_platformBuf = JsonClassConvert.get_takeaways_platform2Class(StrOutput);
            if ((get_takeaways_platformBuf != null) && (get_takeaways_platformBuf.data != null)) 
            {
                if (get_takeaways_platformBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_takeaways_platformBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO takeaways_platform (SID,platform_name,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}') ON CONFLICT(SID) DO UPDATE SET platform_name='{1}',created_time='{2}',updated_time='{3}'",
                            get_takeaways_platformBuf.data[i].platform_sid,
                            get_takeaways_platformBuf.data[i].platform_name,
                            TimeConvert.UnixTimeStampToDateTime(get_takeaways_platformBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_takeaways_platformBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_company_customized_params()
        {
            String SQL = "";
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/customized_params", "", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_company_customized_params get_company_customized_paramsBuf = JsonClassConvert.get_company_customized_params2Class(StrOutput);
            if ((get_company_customized_paramsBuf != null) && (get_company_customized_paramsBuf.data != null)) 
            {
                if (get_company_customized_paramsBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_company_customized_paramsBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO company_customized_params (customized_code,customized_name,active_state,params,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}') ON CONFLICT(customized_code) DO UPDATE SET customized_name='{1}',active_state='{2}',params='{3}',created_time='{4}',updated_time='{5}'",
                            get_company_customized_paramsBuf.data[i].customized_code,
                            get_company_customized_paramsBuf.data[i].customized_name,
                            get_company_customized_paramsBuf.data[i].active_state,
                            get_company_customized_paramsBuf.data[i].@params,
                            TimeConvert.UnixTimeStampToDateTime(get_company_customized_paramsBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_company_customized_paramsBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_company_invoice_params()
        {
            String SQL = "";
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/inv_params", "", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            StrOutput = StrOutput.Replace("null", "\"\"");
            get_company_invoice_params get_company_invoice_paramsBuf = JsonClassConvert.get_company_invoice_params2Class(StrOutput);
            if ((get_company_invoice_paramsBuf != null) && (get_company_invoice_paramsBuf.data != null))   
            {
                SQL = String.Format("INSERT INTO company_invoice_params (company_sid,platform_sid,env_type,active_state,branch_no,reg_id,qrcode_aes_key,inv_renew_count,auth_account,auth_password,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ON CONFLICT(company_sid) DO UPDATE SET platform_sid='{1}',env_type='{2}',active_state='{3}',branch_no='{4}',reg_id='{5}',qrcode_aes_key='{6}',inv_renew_count='{7}',auth_account='{8}',auth_password='{9}',created_time='{10}',updated_time='{11}'",
                    get_company_invoice_paramsBuf.data[0].company_sid,
                    get_company_invoice_paramsBuf.data[0].platform_sid,
                    get_company_invoice_paramsBuf.data[0].env_type,
                    get_company_invoice_paramsBuf.data[0].active_state,
                    get_company_invoice_paramsBuf.data[0].branch_no,
                    get_company_invoice_paramsBuf.data[0].reg_id,
                    get_company_invoice_paramsBuf.data[0].qrcode_aes_key,
                    get_company_invoice_paramsBuf.data[0].inv_renew_count,
                    get_company_invoice_paramsBuf.data[0].auth_account,
                    get_company_invoice_paramsBuf.data[0].auth_password,
                    TimeConvert.UnixTimeStampToDateTime(get_company_invoice_paramsBuf.data[0].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    TimeConvert.UnixTimeStampToDateTime(get_company_invoice_paramsBuf.data[0].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            }

            m_intStep++;
        }

        public static void get_invoice_platform()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM invoice_platform";
            DataTable invoice_platformDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((invoice_platformDataTable != null) && (invoice_platformDataTable.Rows.Count > 0))
            {
                String StrMAXDate = invoice_platformDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/invoice/get_inv_platform", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_invoice_platform get_invoice_platformBuf = JsonClassConvert.get_invoice_platform2Class(StrOutput);
            if ((get_invoice_platformBuf != null) && (get_invoice_platformBuf.data != null))
            {
                if (get_invoice_platformBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_invoice_platformBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO invoice_platform (SID,platform_name,inv_url_1,inv_url_2,inv_test_url_1,inv_test_url_2,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ON CONFLICT(SID) DO UPDATE SET platform_name='{1}',inv_url_1='{2}',inv_url_2='{3}',inv_test_url_1='{4}',inv_test_url_2='{5}',created_time='{6}',updated_time='{7}'",
                            get_invoice_platformBuf.data[i].SID,
                            get_invoice_platformBuf.data[i].platform_name,
                            get_invoice_platformBuf.data[i].inv_url_1,
                            get_invoice_platformBuf.data[i].inv_url_2,
                            get_invoice_platformBuf.data[i].inv_test_url_1,
                            get_invoice_platformBuf.data[i].inv_test_url_2,
                            TimeConvert.UnixTimeStampToDateTime(get_invoice_platformBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_invoice_platformBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_easy_card_blacklist_info()
        {
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/easy_card/blacklist/info", "", "Authorization", "Basic " + m_StrEncoded);
            POS_ECMAPI.m_EasyCardBlacklist = JsonClassConvert.EasyCardBlacklist2Class(StrOutput);
        }

        public static void get_company_payment_type()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM company_payment_type";
            DataTable company_payment_typeDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((company_payment_typeDataTable != null) && (company_payment_typeDataTable.Rows.Count > 0))
            {
                String StrMAXDate = company_payment_typeDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/payment/payment_type", StrParams, "Authorization", "Basic " + m_StrEncoded);
            //StrOutput = StrOutput.Replace("\"del_unix_time\":null", "\"del_unix_time\":0");
            //StrOutput = StrOutput.Replace("\"stop_unix_time\":null", "\"stop_unix_time\":0");
            //StrOutput = StrOutput.Replace("\"sort\":null", "\"sort\":0");
            StrOutput = JsonDataModified(StrOutput);
            get_company_payment_type get_company_payment_typeBuf = JsonClassConvert.get_company_payment_type2Class(StrOutput);
            if ((get_company_payment_typeBuf != null) && (get_company_payment_typeBuf.data != null))
            {
                if (get_company_payment_typeBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_company_payment_typeBuf.data.Count; i++)
                    {
                        if (get_company_payment_typeBuf.data[i].del_flag != "Y")
                        {
                            SQL = String.Format("INSERT INTO company_payment_type (SID,payment_code,payment_name,payment_module_code,def_paid_flag,def_paid_amount,no_change_flag,del_flag,del_time,stop_flag,stop_time,sort,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ON CONFLICT(SID) DO UPDATE SET payment_code='{1}',payment_name='{2}',payment_module_code='{3}',def_paid_flag='{4}',def_paid_amount='{5}',no_change_flag='{6}',del_flag='{7}',del_time='{8}',stop_flag='{9}',stop_time='{10}',sort='{11}',created_time='{12}',updated_time='{13}'",
                                get_company_payment_typeBuf.data[i].SID,
                                get_company_payment_typeBuf.data[i].payment_code,
                                get_company_payment_typeBuf.data[i].payment_name,
                                get_company_payment_typeBuf.data[i].payment_module_code,
                                get_company_payment_typeBuf.data[i].def_paid_flag,
                                get_company_payment_typeBuf.data[i].def_paid_amount,
                                get_company_payment_typeBuf.data[i].no_change_flag,
                                get_company_payment_typeBuf.data[i].del_flag,
                                "1970-01-01",
                                get_company_payment_typeBuf.data[i].stop_flag,
                                "1970-01-01",
                                (get_company_payment_typeBuf.data[i].sort==null)?0:get_company_payment_typeBuf.data[i].sort,
                                TimeConvert.UnixTimeStampToDateTime(get_company_payment_typeBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                TimeConvert.UnixTimeStampToDateTime(get_company_payment_typeBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        }
                    }
                }
            }

            m_intStep++;
        }

        public static void get_payment_module_params()
        {
            String StrParams = "";
            String SQL = "";

            //---
            //強制清空兩個對應資料表;強迫全部更新
            SQL = "DELETE FROM payment_module_params";
            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            SQL = "DELETE FROM payment_module";
            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            //---強制清空兩個對應資料表;強迫全部更新
            
            SQL = "SELECT MAX(updated_time) FROM payment_module_params";
            DataTable payment_module_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((payment_module_paramsDataTable != null) && (payment_module_paramsDataTable.Rows.Count > 0))
            {
                String StrMAXDate = payment_module_paramsDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company/payment/payment_module", StrParams, "Authorization", "Basic " + m_StrEncoded);
            //StrOutput = StrOutput.Replace("null", "0");
            StrOutput = JsonDataModified(StrOutput);
            get_payment_module_params get_payment_module_paramsBuf = JsonClassConvert.get_payment_module_params2Class(StrOutput);
            if ((get_payment_module_paramsBuf != null) && (get_payment_module_paramsBuf.data != null))
            {
                if (get_payment_module_paramsBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_payment_module_paramsBuf.data.Count; i++)
                    {
                        if (get_payment_module_paramsBuf.data[i].del_flag!="Y")
                        {
                            SQL = String.Format("INSERT INTO payment_module_params (SID,payment_module_code,params,del_flag,del_time,stop_flag,stop_time,sort,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ON CONFLICT(SID) DO UPDATE SET payment_module_code='{1}',params='{2}',del_flag='{3}',del_time='{4}',stop_flag='{5}',stop_time='{6}',sort='{7}',created_time='{8}',updated_time='{9}'",
                                get_payment_module_paramsBuf.data[i].SID,
                                get_payment_module_paramsBuf.data[i].payment_module_code,
                                get_payment_module_paramsBuf.data[i].@params,
                                get_payment_module_paramsBuf.data[i].del_flag,
                                "1970-01-01",
                                get_payment_module_paramsBuf.data[i].stop_flag,
                                "1970-01-01",
                                (get_payment_module_paramsBuf.data[i].sort==null)?0: get_payment_module_paramsBuf.data[i].sort,
                                TimeConvert.UnixTimeStampToDateTime(get_payment_module_paramsBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                TimeConvert.UnixTimeStampToDateTime(get_payment_module_paramsBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                            if((get_payment_module_paramsBuf.data[i].pub_params!=null) && (get_payment_module_paramsBuf.data[i].pub_params.Length>0))
                            {
                                SQL = String.Format("INSERT INTO payment_module (payment_module_code,def_params,created_time,updated_time,active_state) VALUES('{0}','{1}','{2}','{3}','Y')  ON CONFLICT(payment_module_code) DO UPDATE SET def_params='{1}',created_time='{2}',updated_time='{3}',active_state='Y'",
                                    get_payment_module_paramsBuf.data[i].payment_module_code,
                                    get_payment_module_paramsBuf.data[i].pub_params,
                                    TimeConvert.UnixTimeStampToDateTime(get_payment_module_paramsBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                    TimeConvert.UnixTimeStampToDateTime(get_payment_module_paramsBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }

                        }

                    }
                }
            }

            m_intStep++;
        }

        public static void get_order_type_data()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM order_type_data";
            DataTable order_type_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((order_type_dataDataTable != null) && (order_type_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = order_type_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/products/price/get_order_type", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_order_type_data get_order_type_dataBuf = JsonClassConvert.get_order_type_data2Class(StrOutput);
            if ((get_order_type_dataBuf != null) && (get_order_type_dataBuf.data != null))
            {
                if (get_order_type_dataBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_order_type_dataBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO order_type_data (SID,price_type_sid,type_name,order_type_code,payment_def,def_payment_code,invoice_state,display_state,sort,stop_flag,stop_time,del_flag,del_time,created_time,updated_time,params) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}') ON CONFLICT(SID,price_type_sid) DO UPDATE SET type_name='{2}',order_type_code='{3}',payment_def='{4}',def_payment_code='{5}',invoice_state='{6}',display_state='{7}',sort='{8}',stop_flag='{9}',stop_time='{10}',del_flag='{11}',del_time='{12}',created_time='{13}',updated_time='{14}',params='{15}'",
                            get_order_type_dataBuf.data[i].price_sid,
                            get_order_type_dataBuf.data[i].price_type_sid,
                            get_order_type_dataBuf.data[i].type_name,
                            get_order_type_dataBuf.data[i].order_type_code,
                            get_order_type_dataBuf.data[i].payment_def,
                            get_order_type_dataBuf.data[i].def_payment_code,
                            get_order_type_dataBuf.data[i].invoice_state,
                            get_order_type_dataBuf.data[i].display_state,
                            get_order_type_dataBuf.data[i].sort,
                            get_order_type_dataBuf.data[i].stop_flag,
                            ((get_order_type_dataBuf.data[i].stop_time=="")||(get_order_type_dataBuf.data[i].stop_time == null) )? "1970-01-01": get_order_type_dataBuf.data[i].stop_time,
                            get_order_type_dataBuf.data[i].del_flag,
                            ((get_order_type_dataBuf.data[i].del_time=="") || (get_order_type_dataBuf.data[i].del_time == null)) ?"1970-01-01": get_order_type_dataBuf.data[i].del_time,
                            TimeConvert.UnixTimeStampToDateTime(get_order_type_dataBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_order_type_dataBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            get_order_type_dataBuf.data[i].@params);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;
        }

        public static void get_terminal_roles()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM role_data";
            DataTable role_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            if ((role_dataDataTable != null) && (role_dataDataTable.Rows.Count > 0))
            {
                String StrMAXDate = role_dataDataTable.Rows[0][0].ToString();
                if (StrMAXDate.Length > 0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/terminal/roles", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_terminal_roles get_terminal_rolesBuf = JsonClassConvert.get_terminal_roles2Class(StrOutput);
            if ((get_terminal_rolesBuf != null) && (get_terminal_rolesBuf.data != null))
            {
                if (get_terminal_rolesBuf.data.Count > 0)
                {
                    for (int i = 0; i < get_terminal_rolesBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO role_data (SID,role_name,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}') ON CONFLICT(SID) DO UPDATE SET role_name='{1}',del_flag='{2}',del_time='{3}',created_time='{4}',updated_time='{5}'",
                            get_terminal_rolesBuf.data[i].SID,
                            get_terminal_rolesBuf.data[i].role_name,
                            get_terminal_rolesBuf.data[i].del_flag,
                            ((get_terminal_rolesBuf.data[i].del_time=="")|| (get_terminal_rolesBuf.data[i].del_time == null)) ?"1970-01-01": get_terminal_rolesBuf.data[i].del_time,
                            TimeConvert.UnixTimeStampToDateTime(get_terminal_rolesBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_terminal_rolesBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                        SQL = String.Format("DELETE FROM role_func WHERE role_sid={0}", get_terminal_rolesBuf.data[i].SID);
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                        if(get_terminal_rolesBuf.data[i].del_flag!="Y")
                        {
                            for(int j=0;j< get_terminal_rolesBuf.data[i].func_relation.Count;j++)
                            {
                                //SQL = String.Format("INSERT INTO role_func(role_sid, func_sid) VALUES('{0}', '{1}')", get_terminal_rolesBuf.data[i].SID, get_terminal_rolesBuf.data[i].func_relation[j].func_sid);
                                SQL = String.Format("INSERT INTO role_func(role_sid, func_sid) VALUES('{0}', '{1}') ON CONFLICT(role_sid, func_sid) DO UPDATE SET role_sid='{0}', func_sid='{1}'", get_terminal_rolesBuf.data[i].SID, get_terminal_rolesBuf.data[i].func_relation[j].func_sid);
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }

                    }
                }
            }

            m_intStep++;
        }

        public static void get_terminal_func_main()
        {
            String StrParams = "";
            String SQL = "SELECT MAX(updated_time) FROM func_main";
            DataTable func_mainDataTable = SQLDataTableModel.GetDataTable(SQL);

            if((func_mainDataTable!=null)&& (func_mainDataTable.Rows.Count>0))
            {
                String StrMAXDate = func_mainDataTable.Rows[0][0].ToString();
                if(StrMAXDate.Length>0)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime MyDateTime = Convert.ToDateTime(StrMAXDate, culture);
                    long value = TimeConvert.DateTimeToUnixTimeStamp(MyDateTime);
                    StrParams = "{\"last_time\":" + value + "}";
                }
                else
                {
                    StrParams = "{\"last_time\":0}";
                }
            }

            String StrOutput = HttpsFun.RESTfulAPI_get("/api/terminal/func_main", StrParams, "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_terminal_func_main get_terminal_func_mainBuf = JsonClassConvert.get_terminal_func_main2Class(StrOutput);
            if ((get_terminal_func_mainBuf != null) && (get_terminal_func_mainBuf.data != null))
            {
                if(get_terminal_func_mainBuf.data.Count>0)
                {
                    for(int i = 0; i < get_terminal_func_mainBuf.data.Count; i++)
                    {
                        SQL = String.Format("INSERT INTO func_main (SID,func_type,parent_func_sid,func_name,content,sort,stop_flag,stop_time,del_flag,del_time,created_time,updated_time) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ON CONFLICT(SID) DO UPDATE SET func_type='{1}',parent_func_sid='{2}',func_name='{3}',content='{4}',sort='{5}',stop_flag='{6}',stop_time='{7}',del_flag='{8}',del_time='{9}',created_time='{10}',updated_time='{11}'",
                            get_terminal_func_mainBuf.data[i].SID,
                            get_terminal_func_mainBuf.data[i].func_type,
                            get_terminal_func_mainBuf.data[i].parent_func_sid,
                            get_terminal_func_mainBuf.data[i].func_name,
                            get_terminal_func_mainBuf.data[i].content,
                            get_terminal_func_mainBuf.data[i].sort,
                            get_terminal_func_mainBuf.data[i].stop_flag,
                            ((get_terminal_func_mainBuf.data[i].stop_time=="") || (get_terminal_func_mainBuf.data[i].stop_time == null)) ?"1970-01-01": get_terminal_func_mainBuf.data[i].stop_time,
                            get_terminal_func_mainBuf.data[i].del_flag,
                            ((get_terminal_func_mainBuf.data[i].del_time=="") || (get_terminal_func_mainBuf.data[i].del_time == null)) ? "1970-01-01" : get_terminal_func_mainBuf.data[i].del_time,
                            TimeConvert.UnixTimeStampToDateTime(get_terminal_func_mainBuf.data[i].created_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            TimeConvert.UnixTimeStampToDateTime(get_terminal_func_mainBuf.data[i].updated_unix_time).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            m_intStep++;

        }
        public static void get_basic_params()
        {
            String SQL = "";
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/terminal/env_params", "", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_terminal_env_params get_terminal_env_paramsBuf = JsonClassConvert.get_terminal_env_params2Class(StrOutput);
            if((get_terminal_env_paramsBuf != null) && (get_terminal_env_paramsBuf.data != null))
            {
                if(get_terminal_env_paramsBuf.data.Count > 0)
                {
                    SQL = "DELETE FROM basic_params WHERE source_type='CLOUD';";
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    for (int i = 0; i < get_terminal_env_paramsBuf.data.Count; i++)
                    {
                        string param_key = get_terminal_env_paramsBuf.data[i].key.ToString();
                        /*
                        string param_value = JsonSerializer.Serialize(get_terminal_env_paramsBuf.data[i].value);
                        param_value = param_value.Replace("\"PAYMENT_API_URL\":null,", "");
                        param_value = param_value.Replace("\"VDES_API_URL\":null,", "");
                        param_value = param_value.Replace(",\"num_len\":null,", "");
                        param_value = param_value.Replace("\"num_mode\":null,", "");
                        param_value = param_value.Replace("\"num_start\":null,", "");
                        param_value = param_value.Replace("\"num_end\":null,", "");
                        param_value = param_value.Replace("\"reset_mode\":null", "");
                        */
                        string param_value=get_terminal_env_paramsBuf.data[i].value.ToString();
                        param_value = param_value.Replace("\\/", "/");//替換URL斜線

                        SQL = String.Format("INSERT INTO basic_params (param_key,param_value,created_time,updated_time,source_type) VALUES('{0}', '{1}', '{2}', '{3}','CLOUD')",
                            param_key, param_value, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    }
                }
            }

            //---
            //將param_data資料表合併到basic_params
            SQL = "SELECT * FROM basic_params WHERE param_key='POS_SERIAL_PARAM' LIMIT 0,1";
            DataTable dt= SQLDataTableModel.GetDataTable(SQL);
            if(!((dt != null) && (dt.Rows.Count > 0)))
            {
                pos_serial_param pos_serial_paramBuf = new pos_serial_param();

                SQL = "SELECT terminal_server_flag,terminal_server_port,order_no_from,serial_server_name,serial_server_port FROM param_data LIMIT 0,1";
                DataTable param_data = SQLDataTableModel.GetDataTable(SQL);
                if (((param_data != null) && (param_data.Rows.Count > 0)))
                {
                    pos_serial_paramBuf.terminal_server_flag = (param_data.Rows[0]["terminal_server_flag"].ToString() == "N") ? "N":"Y" ;
                    pos_serial_paramBuf.terminal_server_port = Convert.ToInt32(param_data.Rows[0]["terminal_server_port"].ToString());
                    pos_serial_paramBuf.order_no_from = (param_data.Rows[0]["order_no_from"].ToString()=="L")?"L":"S";
                    pos_serial_paramBuf.serial_server_name = (param_data.Rows[0]["serial_server_name"].ToString()=="")?"127.0.0.1": param_data.Rows[0]["serial_server_name"].ToString();
                    pos_serial_paramBuf.serial_server_port = Convert.ToInt32(param_data.Rows[0]["serial_server_port"].ToString());
                }

                String param_value = JsonClassConvert.pos_serial_param2String(pos_serial_paramBuf);
                
                SQL = String.Format("INSERT INTO basic_params (param_key,param_value,created_time,updated_time,source_type) VALUES('{0}', '{1}', '{2}', '{3}','POS')",
                    "POS_SERIAL_PARAM", param_value, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            }
            //---將param_data資料表合併到basic_params
            m_intStep++;
        }

        public static void get_company()
        {
            String StrOutput = HttpsFun.RESTfulAPI_get("/api/company","", "Authorization", "Basic " + m_StrEncoded);
            StrOutput = JsonDataModified(StrOutput);
            get_company get_companyBuf = JsonClassConvert.get_company2Class(StrOutput);
            if((get_companyBuf != null) && (get_companyBuf.data != null))
            {
                if(get_companyBuf.data.Count>0)
                {
                    String SQL = "DELETE FROM company;";
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                    SQL = String.Format("INSERT INTO company (SID,company_no,company_name,company_shortname,EIN,business_name,company_owner,tel,fax,zip_code,country_code,province_code,city_code,district_code,address,def_order_type,def_tax_sid,def_unit_sid,vtstore_order_url,take_service_flag,take_service_type,take_service_val,del_flag,created_time,updated_time,def_params) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}','{25}')",
                        get_companyBuf.data[0].company_sid,
                        get_companyBuf.data[0].company_no,
                        get_companyBuf.data[0].name,
                        get_companyBuf.data[0].shortname,
                        get_companyBuf.data[0].company_ein,
                        get_companyBuf.data[0].business_name,
                        get_companyBuf.data[0].owner,
                        get_companyBuf.data[0].tel,
                        get_companyBuf.data[0].fax,
                        get_companyBuf.data[0].zip_code,
                        get_companyBuf.data[0].country_code,
                        get_companyBuf.data[0].province_code,
                        get_companyBuf.data[0].city_code,
                        get_companyBuf.data[0].district_code,
                        get_companyBuf.data[0].address,
                        get_companyBuf.data[0].def_order_type,
                        get_companyBuf.data[0].def_tax_sid,
                        get_companyBuf.data[0].def_unit_sid,
                        get_companyBuf.data[0].vtstore_order_url,
                        get_companyBuf.data[0].take_service_flag,
                        get_companyBuf.data[0].take_service_type,
                        get_companyBuf.data[0].take_service_val,
                        "N",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        get_companyBuf.data[0].def_params);
                    SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                }
            }
            m_intStep++;
        }//SyncDBData
    }
}
