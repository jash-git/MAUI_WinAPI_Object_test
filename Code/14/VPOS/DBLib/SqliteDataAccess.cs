using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;//SQLiteConnection
using Dapper;//DynamicParameters
using System.Data;//IDbConnection
using System.Configuration;//ConfigurationManager

namespace VPOS
{
    public class SqliteDataAccess
    {

        public static string ConnectionStringLoad(string id = "Default")
        {
            //return Cryption.AesDecrypt(ConfigurationManager.ConnectionStrings[id].ConnectionString, "0123456789987654"); //ConfigurationManager.ConnectionStrings[id].ConnectionString;
            String StrResult = "";
            switch (id)
            {
                case "Default":
                    StrResult = Cryption.AesDecrypt("PnJr/hxqamOJC/sUCd4T0dk7zjNWeNME0IanujV7jOybNgYLGna0zIs1f21ITwz3SMfYOt6ttz+MrBBMV/hC0zkfpyVOuqtryGj8H4Z/jKI5JJaKyDN3HS1sRQ0xzk8cjHyolFTENltSoYLRGxrdJNogqCz33hNl6RmsbzdzOclF/GLh3nyHY5eD3xjJINNBFdTqZxtcPJWWVwx8/hJD1q5+IXw/hNYW1zi59t42Fu4=", "0123456789987654");
                    break;
                case "Synchronize":
                    StrResult = Cryption.AesDecrypt("vv7U+j7ISnOI2aLG/KwSrOEcK7bdwzXdk9fGE3URlmwF3bA23QVu9umwPaO7ZxMKdsLwwcUMdQqQQPxIENP2pW9lz1g0lRFnQ+Z17l4bl3BLA7ygZFRoXi56Q2v2Cad0VkBGhjWewFAj+SREepKj7vB3ygnkaQFJGVJHcSS1gF86rCGvBNQ9jCoFEqR325NXA/GpKL7WKj26eq33rVwFGJZ58l+lTJo/N0507Xx1GfwATOAN1t98T8C9t+qjn/yc", "0123456789987654");
                    break;
                case "Takeaways":
                    StrResult = Cryption.AesDecrypt("vnul9w5EOmC8lC0tqJKIO5GhRgtq7O4r444s4xn9+Qeqf0NguXUdv0mSXMFZLqXllS6KIo4u/9utY8vx7rFEIj797eEmXi1hhoEqeDxzpBBpQgOFNpJFu2S0I99QXniaEUdy2vdasnr/51sFHwxKGHxrTnRHkYh1cXTPeI+jYb9VYgf32HXVyQTb4uQoel78LBTFDcl/fEZ+H12HDlyynh7n3yZCmQVD58zjkxmq/8hUOvPH0VlLrlFZnWMgv5gD", "0123456789987654");
                    break;
            }
            return StrResult;
        }

        //---
        //DB2List Function
        public static List<account_data> account_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<account_data>("SELECT * FROM account_data WHERE del_flag!='Y' ORDER BY sort", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<basic_params> basic_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<basic_params>("SELECT * FROM basic_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<class_data> class_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<class_data>("SELECT * FROM class_data ORDER BY sort", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<company> companyLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<company>("SELECT * FROM company", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<company_customized_params> company_customized_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<company_customized_params>("SELECT * FROM company_customized_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<company_invoice_params> company_invoice_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<company_invoice_params>("SELECT * FROM company_invoice_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<company_payment_type> company_payment_typeLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<company_payment_type>("SELECT * FROM company_payment_type WHERE del_flag='N' AND stop_flag='N' ORDER BY sort", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<condiment_data> condiment_dataLoad(String Strcondiment_sids="")
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                String SQL = "SELECT * FROM condiment_data";
                if(Strcondiment_sids.Length==0)
                {
                    SQL = "SELECT * FROM condiment_data";
                }
                else
                {
                    SQL += String.Format(" WHERE SID IN ({0})", Strcondiment_sids);
                }
                var output = cnn.Query<condiment_data>(SQL, new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<condiment_group> condiment_groupLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<condiment_group>("SELECT * FROM condiment_group", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<discount_hotkey> discount_hotkeyLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<discount_hotkey>("SELECT * FROM discount_hotkey WHERE del_flag='N';", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<func_main> func_mainLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<func_main>("SELECT * FROM func_main", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<invoice_platform> invoice_platformLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<invoice_platform>("SELECT * FROM invoice_platform", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<member_platform_params> member_platform_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<member_platform_params>("SELECT * FROM member_platform_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<order_content_data> order_content_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<order_content_data>("SELECT * FROM order_content_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<order_data> order_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<order_data>("SELECT * FROM order_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<order_type_data> order_type_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<order_type_data>("SELECT * FROM order_type_data WHERE stop_flag='N' AND del_flag='N' ORDER BY sort;", new DynamicParameters());
                //var output = cnn.Query<order_type_data>("SELECT * FROM order_type_data WHERE display_state='Y' AND stop_flag='N' AND del_flag='N' AND ((stop_time='1970-01-01') OR (datetime()<stop_time)) ORDER BY sort", new DynamicParameters());//order_type_data 加上 stop_time 過濾條件 at 20221021
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<packaging_data> packaging_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<packaging_data>("SELECT * FROM packaging_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<packaging_type> packaging_typeLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<packaging_type>("SELECT * FROM packaging_type", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<payment_module> payment_moduleLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<payment_module>("SELECT * FROM payment_module", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<payment_module_params> payment_module_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<payment_module_params>("SELECT * FROM payment_module_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<price_type> price_typeLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<price_type>("SELECT * FROM price_type WHERE del_flag='N'", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<printer_data> printer_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<printer_data>("SELECT * FROM printer_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<printer_group_data> printer_group_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<printer_group_data>("SELECT * FROM printer_group_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<printer_group_relation> printer_group_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<printer_group_relation>("SELECT * FROM printer_group_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_category> product_categoryLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                //var output = cnn.Query<product_category>("SELECT * FROM product_category", new DynamicParameters());
                var output = cnn.Query<product_category>("SELECT * FROM product_category WHERE stop_flag='N' AND del_flag='N' AND display_flag='Y' ORDER BY sort", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_category_relation> product_category_relationLoad(int id=0)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                String SQL = "SELECT * FROM product_category_relation";
                if(id==0)
                {
                    SQL = "SELECT * FROM product_category_relation";
                }
                else
                {
                    SQL +=String.Format(" WHERE category_sid={0}", id);
                }
                var output = cnn.Query<product_category_relation>(SQL, new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_condiment_relation> product_condiment_relationLoad(int id=0)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                String SQL = "SELECT* FROM product_condiment_relation";
                if(id == 0)
                {
                    SQL = "SELECT* FROM product_condiment_relation";
                }
                else
                {
                    SQL += String.Format(" WHERE product_sid={0} AND condiment_sid>0 ", id);
                }
                var output = cnn.Query<product_condiment_relation>(SQL, new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_data> product_dataLoad(String Strproduct_sids="")
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                String SQL = "SELECT * FROM product_data";
                if(Strproduct_sids.Length==0)
                {
                    //SQL = "SELECT * FROM product_data";
                    SQL = String.Format("{0} WHERE {1} ORDER BY sort", SQL, "del_flag = 'N' AND stop_flag = 'N'");
                }
                else
                {
                    //SQL += String.Format(" WHERE SID IN ({0})", Strproduct_sids);
                    SQL = String.Format("{0} WHERE {1} AND SID IN ({2}) ORDER BY sort", SQL, "del_flag = 'N' AND stop_flag = 'N'", Strproduct_sids);
                }
                var output = cnn.Query<product_data>(SQL, new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }

        public static List<product_psr_psd_data> product_psr_psd_dataLoad(String Strproduct_sids = "")
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                String SQL = "SELECT pd.*,IFNULL(psr.spec_sid,0) AS spec_sid,psr.alias_name,psd.spec_name FROM product_data AS pd LEFT JOIN product_spec_relation AS psr ON pd.SID=psr.product_sid LEFT JOIN product_spec_data AS psd ON psd.init_product_sid=pd.SID WHERE pd.del_flag = 'N' AND pd.stop_flag = 'N'";
                if (Strproduct_sids.Length == 0)
                {
                    SQL = String.Format("{0} ORDER BY pd.sort", SQL);
                }
                else
                {
                    SQL = String.Format("{0}  AND pd.SID IN ({1}) ORDER BY pd.sort", SQL, Strproduct_sids);
                }
                var output = cnn.Query<product_psr_psd_data>(SQL, new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }

        public static List<product_price_type_relation> product_price_type_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_price_type_relation>("SELECT * FROM product_price_type_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_promotion_relation> product_promotion_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_promotion_relation>("SELECT * FROM product_promotion_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_set_relation> product_set_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_set_relation>("SELECT * FROM product_set_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_spec_data> product_spec_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_spec_data>("SELECT * FROM product_spec_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_spec_relation> product_spec_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_spec_relation>("SELECT * FROM product_spec_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<product_unit> product_unitLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<product_unit>("SELECT * FROM product_unit", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<promotion_data> promotion_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                //var output = cnn.Query<promotion_data>("SELECT * FROM promotion_data", new DynamicParameters());
                //ALL: SID,company_sid,promotion_name,promotion_start_time,promotion_end_time,promotion_sort,coexist_flag,promotion_type,promotion_data AS promotion_data1,stop_flag,stop_time,del_flag,del_time,created_time,updated_time
                var output = cnn.Query<promotion_data>("SELECT SID,company_sid,promotion_name,promotion_start_time,promotion_end_time,promotion_sort,coexist_flag,promotion_type,promotion_data AS promotion_data1,stop_flag,stop_time,del_flag,del_time,created_time,updated_time FROM promotion_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<promotion_order_type_relation> promotion_order_type_relationLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<promotion_order_type_relation>("SELECT * FROM promotion_order_type_relation", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<role_data> role_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<role_data>("SELECT * FROM role_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<role_func> role_funcLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<role_func>("SELECT * FROM role_func", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<serial_code_data> serial_code_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<serial_code_data>("SELECT * FROM serial_code_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<set_attribute_data> set_attribute_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<set_attribute_data>("SELECT * FROM set_attribute_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<store_table_data> store_table_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<store_table_data>("SELECT * FROM store_table_data WHERE stop_flag='N' AND del_flag='N' ORDER BY table_sort", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<takeaways_params> takeaways_paramsLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<takeaways_params>("SELECT * FROM takeaways_params", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<takeaways_platform> takeaways_platformLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<takeaways_platform>("SELECT * FROM takeaways_platform", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<tax_data> tax_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<tax_data>("SELECT * FROM tax_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        public static List<terminal_data> terminal_dataLoad()
        {
            List<terminal_data> LTResult = new List<terminal_data>();
            terminal_data terminal_dataBuf = new terminal_data();
            //DataTable dt = SQLDataTableModel.GetDataTable("SELECT SID,company_sid,terminal_name,pos_no,pid,rid,app_version,reg_flag,reg_submit_time,reg_accept_time,api_token_id,client_id,client_secret,now_class_sid,petty_cash,business_day,business_close_time,invoice_flag,invoice_batch_num,invoice_active_state,last_order_no,use_call_num,use_call_date,online_time,keyhook_enable,last_check_update_time,last_class_report_no,last_daily_report_no,use_call_num_date FROM terminal_data LIMIT 0,1");
            //DataTable dt = SQLDataTableModel.GetDataTable("SELECT SID,company_sid,terminal_name,pos_no,pid,rid,app_version,reg_flag,reg_submit_time,reg_accept_time,api_token_id,client_id,client_secret,now_class_sid,petty_cash,business_day,business_close_time,invoice_flag,invoice_batch_num,invoice_active_state,last_order_no,use_call_num,use_call_date,keyhook_enable,last_check_update_time,last_class_report_no,last_daily_report_no,use_call_num_date FROM terminal_data LIMIT 0,1");
            DataTable dt = SQLDataTableModel.GetDataTable("SELECT SID,company_sid,terminal_name,pos_no,pid,rid,app_version,reg_flag,reg_submit_time,reg_accept_time,api_token_id,client_id,client_secret,now_class_sid,petty_cash,business_day,business_close_time,invoice_flag,invoice_batch_num,invoice_active_state,last_order_no,use_call_num,use_call_date,keyhook_enable,last_check_update_time,last_class_report_no,last_daily_report_no FROM terminal_data LIMIT 0,1");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                terminal_dataBuf.SID = dt.Rows[0]["SID"].ToString();
                terminal_dataBuf.company_sid = dt.Rows[0]["company_sid"].ToString();
                terminal_dataBuf.terminal_name = dt.Rows[0]["terminal_name"].ToString();
                terminal_dataBuf.pos_no = dt.Rows[0]["pos_no"].ToString();
                terminal_dataBuf.pid = dt.Rows[0]["pid"].ToString();
                terminal_dataBuf.rid = dt.Rows[0]["rid"].ToString();
                terminal_dataBuf.app_version = dt.Rows[0]["app_version"].ToString();
                terminal_dataBuf.reg_flag = dt.Rows[0]["reg_flag"].ToString();
                terminal_dataBuf.reg_submit_time = dt.Rows[0]["reg_submit_time"].ToString();
                terminal_dataBuf.reg_accept_time = dt.Rows[0]["reg_accept_time"].ToString();
                terminal_dataBuf.api_token_id = dt.Rows[0]["api_token_id"].ToString();
                terminal_dataBuf.client_id = dt.Rows[0]["client_id"].ToString();
                terminal_dataBuf.client_secret = dt.Rows[0]["client_secret"].ToString();
                terminal_dataBuf.now_class_sid = dt.Rows[0]["now_class_sid"].ToString();
                terminal_dataBuf.petty_cash = dt.Rows[0]["petty_cash"].ToString();
                terminal_dataBuf.business_day = Convert.ToDateTime(dt.Rows[0]["business_day"].ToString()).ToString("yyyy-MM-dd");
                terminal_dataBuf.business_close_time = dt.Rows[0]["business_close_time"].ToString();
                terminal_dataBuf.invoice_flag = dt.Rows[0]["invoice_flag"].ToString();
                terminal_dataBuf.invoice_batch_num = dt.Rows[0]["invoice_batch_num"].ToString();
                terminal_dataBuf.invoice_active_state = dt.Rows[0]["invoice_active_state"].ToString();
                terminal_dataBuf.last_order_no = dt.Rows[0]["last_order_no"].ToString();
                terminal_dataBuf.use_call_num = dt.Rows[0]["use_call_num"].ToString();
                terminal_dataBuf.use_call_date = dt.Rows[0]["use_call_date"].ToString();
                //terminal_dataBuf.online_time = dt.Rows[0]["online_time"].ToString();
                terminal_dataBuf.keyhook_enable = dt.Rows[0]["keyhook_enable"].ToString();
                terminal_dataBuf.last_check_update_time = dt.Rows[0]["last_check_update_time"].ToString();
                terminal_dataBuf.last_class_report_no = dt.Rows[0]["last_class_report_no"].ToString();
                terminal_dataBuf.last_daily_report_no = dt.Rows[0]["last_daily_report_no"].ToString();
                //terminal_dataBuf.use_call_num_date = dt.Rows[0]["use_call_num_date"].ToString();
            }

            LTResult.Add(terminal_dataBuf);
            return LTResult;
        }
        public static List<user_data> user_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<user_data>("SELECT * FROM user_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }

        public static void AllTable2ListVar()
        {
            m_account_data = account_dataLoad();
            m_basic_params = basic_paramsLoad();
            m_class_data = class_dataLoad();
            m_company = companyLoad();
            m_company_customized_params = company_customized_paramsLoad();
            m_company_invoice_params = company_invoice_paramsLoad();
            m_company_payment_type = company_payment_typeLoad();
            m_condiment_data = condiment_dataLoad();
            m_condiment_group = condiment_groupLoad();
            m_discount_hotkey = discount_hotkeyLoad();
            m_func_main = func_mainLoad();
            m_invoice_platform = invoice_platformLoad();
            m_member_platform_params = member_platform_paramsLoad();
            m_order_content_data = order_content_dataLoad();
            m_order_data = order_dataLoad();
            m_order_type_data = order_type_dataLoad();
            m_packaging_data = packaging_dataLoad();
            m_packaging_type = packaging_typeLoad();
            m_payment_module = payment_moduleLoad();
            m_payment_module_params = payment_module_paramsLoad();
            m_price_type = price_typeLoad();
            m_printer_data = printer_dataLoad();
            m_printer_group_data = printer_group_dataLoad();
            m_printer_group_relation = printer_group_relationLoad();
            m_product_category = product_categoryLoad();
            m_product_category_relation = product_category_relationLoad();
            m_product_condiment_relation = product_condiment_relationLoad();
            m_product_data = product_dataLoad();
            m_product_price_type_relation = product_price_type_relationLoad();
            m_product_promotion_relation = product_promotion_relationLoad();
            m_product_set_relation = product_set_relationLoad();
            m_product_spec_data = product_spec_dataLoad();
            m_product_spec_relation = product_spec_relationLoad();
            m_product_unit = product_unitLoad();
            m_promotion_data = promotion_dataLoad();
            m_promotion_order_type_relation = promotion_order_type_relationLoad();
            m_role_data = role_dataLoad();
            m_role_func = role_funcLoad();
            m_serial_code_data = serial_code_dataLoad();
            m_set_attribute_data = set_attribute_dataLoad();
            m_store_table_data = store_table_dataLoad();
            m_takeaways_params = takeaways_paramsLoad();
            m_takeaways_platform = takeaways_platformLoad();
            m_tax_data = tax_dataLoad();
            m_terminal_data = terminal_dataLoad();
            m_user_data = user_dataLoad();
        }

        public static void product_category2ListVar()//取得產品分類
        {
            m_product_category = product_categoryLoad();
        }

        public static void product2ListVar(int id)
        {
            String Strcategory_sids = "";
            m_product_category_relation = product_category_relationLoad(id);
            for (int i = 0; i < m_product_category_relation.Count; i++)
            {
                if (i > 0)
                {
                    Strcategory_sids += ",";
                }
                Strcategory_sids += "" + m_product_category_relation[i].product_sid;
            }
            if (Strcategory_sids.Length > 0)
            {
                m_product_psr_psd_data = product_psr_psd_dataLoad(Strcategory_sids);//m_product_data = product_dataLoad(Strcategory_sids);
            }
            else
            {
                m_product_psr_psd_data = null;//m_product_data = null;
            }

        }
        public static void condiment2ListVar(int id)
        {
            String Strcondiment_sids = "";
            m_product_condiment_relation = product_condiment_relationLoad(id);
            for (int i = 0; i < m_product_condiment_relation.Count; i++)
            {
                if (i > 0)
                {
                    Strcondiment_sids += ",";
                }
                Strcondiment_sids += "" + m_product_condiment_relation[i].condiment_sid;
            }
            if (Strcondiment_sids.Length > 0)
            {
                m_condiment_data = condiment_dataLoad(Strcondiment_sids);
            }
            else
            {
                m_condiment_data = null;
            }
        }

        public static List<param_data> param_dataLoad()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionStringLoad()))
            {
                var output = cnn.Query<param_data>("SELECT * FROM param_data", new DynamicParameters());
                cnn.Close();cnn.Dispose();
                return output.ToList();
            }
        }
        //---DB2List Function

        //---
        //DB2Model List Var
        public static List<account_data> m_account_data = new List<account_data>();
        public static List<basic_params> m_basic_params = new List<basic_params>();
        public static List<class_data> m_class_data = new List<class_data>();
        public static List<company> m_company = new List<company>();
        public static List<company_customized_params> m_company_customized_params = new List<company_customized_params>();
        public static List<company_invoice_params> m_company_invoice_params = new List<company_invoice_params>();
        public static List<company_payment_type> m_company_payment_type = new List<company_payment_type>();
        public static List<condiment_data> m_condiment_data = new List<condiment_data>();
        public static List<condiment_group> m_condiment_group = new List<condiment_group>();
        public static List<discount_hotkey> m_discount_hotkey = new List<discount_hotkey>();
        public static List<func_main> m_func_main = new List<func_main>();
        public static List<invoice_platform> m_invoice_platform = new List<invoice_platform>();
        public static List<member_platform_params> m_member_platform_params = new List<member_platform_params>();
        public static List<order_content_data> m_order_content_data = new List<order_content_data>();
        public static List<order_data> m_order_data = new List<order_data>();
        public static List<order_type_data> m_order_type_data = new List<order_type_data>();
        public static List<packaging_data> m_packaging_data = new List<packaging_data>();
        public static List<packaging_type> m_packaging_type = new List<packaging_type>();
        public static List<payment_module> m_payment_module = new List<payment_module>();
        public static List<payment_module_params> m_payment_module_params = new List<payment_module_params>();
        public static List<price_type> m_price_type = new List<price_type>();
        public static List<printer_data> m_printer_data = new List<printer_data>();
        public static List<printer_group_data> m_printer_group_data = new List<printer_group_data>();
        public static List<printer_group_relation> m_printer_group_relation = new List<printer_group_relation>();
        public static List<product_category> m_product_category = new List<product_category>();
        public static List<product_category_relation> m_product_category_relation = new List<product_category_relation>();
        public static List<product_condiment_relation> m_product_condiment_relation = new List<product_condiment_relation>();
        public static List<product_data> m_product_data = new List<product_data>();
        public static int m_intPPP_spec_sid_Count=0;
        public static int m_intPPP_spec_name_Count = 0;
        public static int m_intPPP_product_Count = 0;
        public static List<product_psr_psd_data> m_product_psr_psd_data = new List<product_psr_psd_data>();
        public static List<product_price_type_relation> m_product_price_type_relation = new List<product_price_type_relation>();
        public static List<product_promotion_relation> m_product_promotion_relation = new List<product_promotion_relation>();
        public static List<product_set_relation> m_product_set_relation = new List<product_set_relation>();
        public static List<product_spec_data> m_product_spec_data = new List<product_spec_data>();
        public static List<product_spec_relation> m_product_spec_relation = new List<product_spec_relation>();
        public static List<product_unit> m_product_unit = new List<product_unit>();
        public static List<promotion_data> m_promotion_data = new List<promotion_data>();
        public static List<promotion_order_type_relation> m_promotion_order_type_relation = new List<promotion_order_type_relation>();
        public static List<role_data> m_role_data = new List<role_data>();
        public static List<role_func> m_role_func = new List<role_func>();
        public static List<serial_code_data> m_serial_code_data = new List<serial_code_data>();
        public static List<set_attribute_data> m_set_attribute_data = new List<set_attribute_data>();
        public static List<store_table_data> m_store_table_data = new List<store_table_data>();
        public static List<takeaways_params> m_takeaways_params = new List<takeaways_params>();
        public static List<takeaways_platform> m_takeaways_platform = new List<takeaways_platform>();
        public static List<tax_data> m_tax_data = new List<tax_data>();
        public static List<terminal_data> m_terminal_data = new List<terminal_data>();
        public static List<user_data> m_user_data = new List<user_data>();
        public static List<param_data> m_param_data = new List<param_data>();
        //---DB2Model List Var



    }//Class SqliteDataAccess
}
