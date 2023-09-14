using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VPOS
{
		public class account_data
		{
			public int SID { get; set; }
			public string account_code { get; set; }
			public string account_name { get; set; }
			public string type { get; set; }
			public int sort { get; set; }
			public string stop_flag { get; set; }
			public int stop_time { get; set; }
			public string del_flag { get; set; }
			public int del_time { get; set; }
			public string created_time { get; set; }
			public string updated_time { get; set; }
		}
		public class basic_params
		{
			public string param_key { get; set; }
			public string param_value { get; set; }
			public string created_time { get; set; }
			public string updated_time { get; set; }
		}
		public class class_data
		{
			public int SID { get; set; }
			public string class_name { get; set; }
			public string time_start { get; set; }
			public string time_end { get; set; }
			public int sort { get; set; }
			public string del_flag { get; set; }
			public string del_time { get; set; }
			public string created_time { get; set; }
			public string updated_time { get; set; }
		}
		public class company
		{
			public int SID { get; set; }
			public string company_no { get; set; }
			public string authorized_store_no { get; set; }
			public string company_name { get; set; }
			public string company_shortname { get; set; }
			public string EIN { get; set; }
			public string business_name { get; set; }
			public string company_owner { get; set; }
			public string tel { get; set; }
			public string fax { get; set; }
			public string zip_code { get; set; }
			public string country_code { get; set; }
			public string province_code { get; set; }
			public string city_code { get; set; }
			public string district_code { get; set; }
			public string address { get; set; }
			public int def_order_type { get; set; }
			public int def_tax_sid { get; set; }
			public int def_unit_sid { get; set; }
			public string vtstore_order_url { get; set; }
			public string take_service_flag { get; set; }
			public string take_service_type { get; set; }
			public decimal take_service_val { get; set; }
			public string del_flag { get; set; }
			public string del_time { get; set; }
			public string created_time { get; set; }
			public string updated_time { get; set; }
		}
		public class company_customized_params
		{
			public string customized_code { get; set; }
			public string customized_name { get; set; }
			public string active_state { get; set; }
			public string @params {get;set;}
			public string created_time { get; set; }
			public string updated_time { get; set; }
	}
	public class company_invoice_params
	{
		public int company_sid { get; set; }
		public int platform_sid { get; set; }
		public string env_type { get; set; }
		public string active_state { get; set; }
		public string branch_no { get; set; }
		public string reg_id { get; set; }
		public string qrcode_aes_key { get; set; }
		public int inv_renew_count { get; set; }
		public string auth_account { get; set; }
		public string auth_password { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
		public int booklet { get; set; }

    }
	public class company_payment_type
	{
		public int SID { get; set; }
		public string payment_code { get; set; }
		public string payment_name { get; set; }
		public string payment_module_code { get; set; }
		public string def_paid_flag { get; set; }
		public decimal def_paid_amount { get; set; }
		public string no_change_flag { get; set; }
		public string del_flag { get; set; }
		public DateTime del_time { get; set; }
		public string stop_flag { get; set; }
		public DateTime stop_time { get; set; }
		public int sort { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class condiment_data//少糖,condiment_data.csv [調味品 數據列表]  - 右側選單02
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string condiment_code { get; set; }
		public string condiment_name { get; set; }
		public decimal condiment_price { get; set; }
		public int unit_sid { get; set; }
		public int group_sid { get; set; }
		public int sort { get; set; }
		public string stop_time { get; set; }
		public string stop_flag { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class condiment_group
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string group_name { get; set; }
		public string required_flag { get; set; }
		public string single_flag { get; set; }
		public string newline_flag { get; set; }
		public string count_flag { get; set; }
		public int min_count { get; set; }
		public int max_count { get; set; }
		public int sort { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class discount_hotkey
	{
		public int SID { get; set; }
		public string hotkey_name { get; set; }
		public string hotkey_code { get; set; }
		public string discount_code { get; set; }
		public string val_mode { get; set; }
		public float val { get; set; }
		public string round_calc_type { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string sort { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class func_main
	{
		public string SID { get; set; }
		public string func_type { get; set; }
		public string parent_func_sid { get; set; }
		public string func_name { get; set; }
		public string content { get; set; }
		public Int16 sort { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class invoice_platform
	{
		public int SID { get; set; }
		public string platform_name { get; set; }
		public string inv_url_1 { get; set; }
		public string inv_url_2 { get; set; }
		public string inv_test_url_1 { get; set; }
		public string inv_test_url_2 { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class member_platform_params
	{
		public int SID { get; set; }
		public string platform_type { get; set; }
		public string @params {get;set;}
		public int sort { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }	
	}
	public class order_content_data
	{
		public string order_no { get; set; }
		public int item_no { get; set; }
		public string data_type { get; set; }
		public string item_type { get; set; }
		public string item_code { get; set; }
		public string item_group_code { get; set; }
		public string item_group_name { get; set; }
		public string show_detail_flag { get; set; }
		public int condiment_group_sid { get; set; }
		public int parent_item_no { get; set; }
		public int item_sid { get; set; }
		public int item_spec_sid { get; set; }
		public string item_name { get; set; }
		public decimal item_cost { get; set; }
		public decimal item_count { get; set; }
		public decimal condiment_price { get; set; }
		public decimal item_subtotal { get; set; }
		public string discount_type { get; set; }
		public string discount_code { get; set; }
		public string discount_name { get; set; }
		public decimal discount_rate { get; set; }
		public decimal discount_fee { get; set; }
		public string discount_info { get; set; }
		public int stock_remainder_quantity { get; set; }
		public decimal stock_push_price { get; set; }
		public int stock_push_quantity { get; set; }
		public decimal stock_push_amount { get; set; }
		public string stock_pull_code { get; set; }
		public string stock_pull_name { get; set; }
		public decimal stock_pull_price { get; set; }
		public int stock_pull_quantity { get; set; }
		public decimal stock_pul_amount { get; set; }
		public string external_id { get; set; }
		public string external_mode { get; set; }
		public string external_description { get; set; }
		public int tax_sid { get; set; }
		public decimal tax_rate { get; set; }
		public string tax_type { get; set; }
		public decimal tax_fee { get; set; }
		public decimal item_amount { get; set; }
		public object subtotal_flag { get; set; }
		public int subtotal_item_no { get; set; }
		public string cust_name { get; set; }
		public string other_info { get; set; }
		public string print_flag { get; set; }
		public int print_count { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public decimal stock_pull_amount { get; set; }
	}
	public class order_data
	{
		public string order_no { get; set; }
		public string order_no_from { get; set; }
		public string order_time { get; set; }
		public int order_type { get; set; }
		public string order_type_name { get; set; }
		public string order_type_code { get; set; }
		public string order_open_time { get; set; }
		public int order_state { get; set; }
		public string order_mode { get; set; }
		public string terminal_sid { get; set; }
		public string pos_no { get; set; }
		public int class_sid { get; set; }
		public string class_name { get; set; }
		public int user_sid { get; set; }
		public string employee_no { get; set; }
		public string table_code { get; set; }
		public string table_name { get; set; }
		public string meal_num { get; set; }
		public string call_num { get; set; }
		public string member_flag { get; set; }
		public string member_platform { get; set; }
		public string member_no { get; set; }
		public string member_name { get; set; }
		public string member_phone { get; set; }
		public string member_info { get; set; }
		public string takeaways_order_sid { get; set; }
		public string takeaways_order_info { get; set; }
		public string outside_order_no { get; set; }
		public string outside_description { get; set; }
		public string delivery_city_name { get; set; }
		public string delivery_district_name { get; set; }
		public string delivery_address { get; set; }
		public decimal item_count { get; set; }
		public decimal subtotal { get; set; }
		public decimal discount_fee { get; set; }
		public decimal promotion_fee { get; set; }
		public string promotion_value { get; set; }
		public decimal coupon_discount { get; set; }
		public string coupon_value { get; set; }
		public int stock_push_quantity { get; set; }
		public decimal stock_push_amount { get; set; }
		public int stock_pull_quantity { get; set; }
		public decimal stock_pull_amount { get; set; }
		public decimal service_rate { get; set; }
		public decimal service_fee { get; set; }
		public decimal trans_reversal { get; set; }
		public decimal over_paid { get; set; }
		public decimal tax_fee { get; set; }
		public decimal amount { get; set; }
		public int paid_type_sid { get; set; }
		public string paid_flag { get; set; }
		public string paid_time { get; set; }
		public string paid_info { get; set; }
		public decimal cash_fee { get; set; }
		public decimal change_fee { get; set; }
		public string invoice_flag { get; set; }
		public string invoice_info { get; set; }
		public string cust_ein { get; set; }
		public string cancel_flag { get; set; }
		public string cancel_time { get; set; }
		public int cancel_class_sid { get; set; }
		public string cancel_class_name { get; set; }
		public string cash_refund_flag { get; set; }
		public decimal refund { get; set; }
		public int refund_type_sid { get; set; }
		public string cancel_upload_flag { get; set; }
		public string cancel_upload_time { get; set; }
		public string del_flag { get; set; }
		public string business_day { get; set; }
		public string class_close_flag { get; set; }
		public string class_report_no { get; set; }
		public string daily_close_flag { get; set; }
		public string daily_report_no { get; set; }
		public string order_temp_info { get; set; }
		public string upload_flag { get; set; }
		public string upload_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
		public string remarks { get; set; }
	}
	public class order_type_data
	{
		public int SID { get; set; }
		public int price_type_sid { get; set; }
		public string type_name { get; set; }
		public string order_type_code { get; set; }
		public int payment_def { get; set; }
		public string def_payment_code { get; set; }
		public int invoice_state { get; set; }
		public string display_state { get; set; }
		public int sort { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
		public string @params { get; set; }
    }
	public class packaging_data
	{
		public int SID { get; set; }
		public int packaging_type_sid { get; set; }
		public string code { get; set; }
		public string name { get; set; }
		public decimal price { get; set; }
		public string memo { get; set; }
		public int sort { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class packaging_type
	{
		public int SID { get; set; }
		public string name { get; set; }
		public int sort { get; set; }
		public string show_flag { get; set; }
		public string required_flag { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class payment_module
	{
		public string payment_module_code { get; set; }
		public string payment_module_name { get; set; }
		public string def_params { get; set; }
		public string active_state { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class payment_module_params
	{
		public int SID { get; set; }
		public string payment_module_code { get; set; }
		public string @params {get;set;}
		public string del_flag { get; set; }
		public DateTime del_time { get; set; }
		public string stop_flag { get; set; }
		public DateTime stop_time { get; set; }
		public int sort { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }	
	}
	public class price_type
	{
		public int price_type_sid { get; set; }
		public int company_sid { get; set; }
		public object price_type_name { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class printer_data
	{
		public int SID { get; set; }
		public string printer_code { get; set; }
		public string printer_name { get; set; }
		public string output_type { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class printer_group_data
	{
		public int SID { get; set; }
		public string printer_group_name { get; set; }
		public int printer_sid { get; set; }
		public int order_type_sid { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class printer_group_relation
	{
		public int printer_group_sid { get; set; }
		public int product_sid { get; set; }
	}
	public class product_category //來找茶,product_category.csv [產品分類]  - 右側選單00
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string category_code { get; set; }
		public string category_name { get; set; }
		public int sort { get; set; }
        public string display_flag { get; set; }//商品類別依據設定[是否顯示]，決定POS端是否出現該類別
        public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class product_category_relation
	{
		public int category_sid { get; set; }
		public int product_sid { get; set; }
	}
	public class product_condiment_relation
	{
		public int product_sid { get; set; }
		public int condiment_sid { get; set; }
	}
	public class product_data//阿嬌儀的獨門私釀,product_data.csv [產品 數據列表]  - 右側選單01
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string product_code { get; set; }
		public string barcode { get; set; }
		public string product_name { get; set; }
		public string product_shortname { get; set; }
		public string product_type { get; set; }
		public object price_mode { get; set; }
		public decimal product_price { get; set; }
		public int unit_sid { get; set; }
		public int tax_sid { get; set; }
		public int sort { get; set; }
		public string memo { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public object price_update_time { get; set; }
		public string category_update_time { get; set; }
		public string condiment_update_time { get; set; }
		public string promotion_update_time { get; set; }
		public string spec_update_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}

	public class product_psr_psd_data
	{
		/*
		SELECT pd.*,IFNULL(psr.spec_sid,0) AS spec_sid,psr.alias_name,psd.spec_name FROM product_data AS pd 

		LEFT JOIN product_spec_relation AS psr ON pd.SID=psr.product_sid

		LEFT JOIN product_spec_data AS psd ON psd.init_product_sid=pd.SID

		WHERE pd.del_flag = 'N' AND pd.stop_flag = 'N' AND pd.SID IN (299,300,301,302,303,304,305) ORDER BY pd.sort
		*/
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string product_code { get; set; }
		public string barcode { get; set; }
		public string product_name { get; set; }
		public string product_shortname { get; set; }
		public string product_type { get; set; }
		public object price_mode { get; set; }
		public decimal product_price { get; set; }
		public int unit_sid { get; set; }
		public int tax_sid { get; set; }
		public int sort { get; set; }
		public string memo { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public object price_update_time { get; set; }
		public string category_update_time { get; set; }
		public string condiment_update_time { get; set; }
		public string promotion_update_time { get; set; }
		public string spec_update_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
		public int spec_sid { get; set; }
		public string alias_name { get; set; }
		public string spec_name { get; set; }
	}

	public class product_price_type_relation
	{
		public int product_sid { get; set; }
		public int price_type_sid { get; set; }
		public decimal price { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class product_promotion_relation
	{
		public int product_sid { get; set; }
		public int promotion_sid { get; set; }
	}
	public class product_set_relation
	{
		public int set_sid { get; set; }
		public int attribute_sid { get; set; }
		public int category_sid { get; set; }
		public int product_sid { get; set; }
		public string main_flag { get; set; }
		public string default_flag { get; set; }
	}
	public class product_spec_data
	{
		public int SID { get; set; }
		public string spec_name { get; set; }
		public int init_product_sid { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class product_spec_relation
	{
		public int spec_sid { get; set; }
		public int product_sid { get; set; }
		public string alias_name { get; set; }
		public int sort { get; set; }
	}
	public class product_unit
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string unit_name { get; set; }
		public int sort { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class promotion_data
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string promotion_name { get; set; }
		public string promotion_start_time { get; set; }
		public string promotion_end_time { get; set; }
		public int promotion_sort { get; set; }
		public string coexist_flag { get; set; }
		public string promotion_type { get; set; }
		public string promotion_data1 { get; set; }//public string promotion_data { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class promotion_order_type_relation
	{
		public int promotion_sid { get; set; }
		public int order_type_sid { get; set; }
	}
	public class role_data
	{
		public int SID { get; set; }
		public string role_name { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class role_func
	{
		public int role_sid { get; set; }
		public string func_sid { get; set; }
	}
	public class serial_code_data
	{
		public string serial_type { get; set; }
		public string serial_name { get; set; }
		public string code_first_char { get; set; }
		public string code_split_char { get; set; }
		public int code_num_len { get; set; }
		public string code_str { get; set; }
		public int code_num { get; set; }
		public string serial_owner { get; set; }
		public string updated_time { get; set; }
	}
	public class set_attribute_data
	{
		public int SID { get; set; }
		public int set_sid { get; set; }
		public string attribute_name { get; set; }
		public string main_price_type { get; set; }
		public decimal main_price { get; set; }
		public decimal main_max_price { get; set; }
		public string sub_price_type { get; set; }
		public decimal sub_price { get; set; }
		public decimal sub_max_price { get; set; }
		public string required_flag { get; set; }
		public int limit_count { get; set; }
		public string repeat_flag { get; set; }
		public int sort { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class store_table_data
	{
		public int SID { get; set; }
		public string table_code { get; set; }
		public string table_name { get; set; }
		public int table_capacity { get; set; }
		public int table_sort { get; set; }
		public string stop_flag { get; set; }
		public string stop_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class takeaways_params
	{
		public string platform_sid { get; set; }
		public string active_state { get; set; }
		public string @params {get;set;}
		public string created_time { get; set; }
		public string updated_time { get; set; }		
	}
	public class takeaways_platform
	{
		public string SID { get; set; }
		public string platform_name { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class tax_data
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public string tax_name { get; set; }
		public decimal tax_rate { get; set; }
		public string tax_type { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class terminal_data
	{
		public string SID { get; set; }
		public string company_sid { get; set; }
		public string terminal_name { get; set; }
		public string pos_no { get; set; }
		public string pid { get; set; }
		public string rid { get; set; }
		public string app_version { get; set; }
		public string reg_flag { get; set; }
		public string reg_submit_time { get; set; }
		public string reg_accept_time { get; set; }
		public string api_token_id { get; set; }
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string now_class_sid { get; set; }
		public string petty_cash { get; set; }
		public string business_day { get; set; }
		public string business_close_time { get; set; }
		public string invoice_flag { get; set; }
		public string invoice_batch_num { get; set; }
		public string invoice_active_state { get; set; }
		public string last_order_no { get; set; }
		public string use_call_num { get; set; }
		public string use_call_date { get; set; }
		public string online_time { get; set; }
		public string keyhook_enable { get; set; }
		public string last_check_update_time { get; set; }
		public string last_class_report_no { get; set; }
		public string last_daily_report_no { get; set; }
		public string use_call_num_date { get; set; }
	}
	public class user_data
	{
		public int SID { get; set; }
		public int company_sid { get; set; }
		public int role_sid { get; set; }
		public string user_account { get; set; }
		public string user_pwd { get; set; }
		public string user_name { get; set; }
		public string employee_no { get; set; }
		public string job_title { get; set; }
		public string tel { get; set; }
		public string cellphone { get; set; }
		public string state_flag { get; set; }
		public string state_time { get; set; }
		public string del_flag { get; set; }
		public string del_time { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}
	public class param_data
	{
		public string terminal_sid { get; set; }
		public string terminal_server_flag { get; set; }
		public int terminal_server_port { get; set; }
		public string order_no_from { get; set; }
		public string serial_server_name { get; set; }
		public int serial_server_port { get; set; }
		public string print_server_name { get; set; }
		public int print_server_port { get; set; }
		public string created_time { get; set; }
		public string updated_time { get; set; }
	}

	public class product_Var
	{
		public int product_sid;
		public double product_price;
		public string product_code;
		public string product_name;
		public string alias_name;

		public String m_Strproduct_type;//類型

		/*
		public int m_intdiscount_type;// 折扣/折讓 旗標
		public String m_Strdiscount_name;// 折扣/折讓 說明文字
		public String m_Strdiscount_code;
		public int m_intdiscount_rate;// 折扣率
		public int m_intdiscount_fee;// 折讓金額
		public String m_Strdiscount_info; //JSON
		*/

		public int m_inttax_sid; //稅率編號 ; 產品若沒有稅率資料[m_tax_sid=0]，就直接從company和tax_data取出預設值
		public int m_inttax_rate; //稅率
		public String m_Strtax_type;//稅率類型
		//public int m_inttax_fee; //稅率金額

		public product_Var()
        {
			product_sid = -1;
			product_price = 0;
			product_code = "";
			product_name = "";
			alias_name = "";

			m_Strproduct_type = "";//類型

			/*
			m_intdiscount_type = -1;// 折扣/折讓 旗標
			m_Strdiscount_name = "";// 折扣/折讓 說明文字
			m_Strdiscount_code = "";
			m_intdiscount_rate = 0;// 折扣率
			m_intdiscount_fee = 0;// 折讓金額
			m_Strdiscount_info = ""; //JSON
			*/

			m_inttax_sid = -1; //稅率編號
			m_inttax_rate = 0; //稅率
			m_Strtax_type = "";//稅率類型
			//m_inttax_fee = 0; //稅率金額

		}
	}
	public class product_spec_Var
	{
		public int product_sid;
		public double product_price;
		public string product_code;
		public string product_name;
		public bool blnspec;

		public String m_Strproduct_type;//類型

		/*
		public int m_intdiscount_type;// 折扣/折讓 旗標
		public String m_Strdiscount_name;// 折扣/折讓 說明文字
		public String m_Strdiscount_code;
		public int m_intdiscount_rate;// 折扣率
		public int m_intdiscount_fee;// 折讓金額
		public String m_Strdiscount_info; //JSON
		*/

		public int m_inttax_sid; //稅率編號 ; 產品若沒有稅率資料[m_tax_sid=0]，就直接從company和tax_data取出預設值
		public int m_inttax_rate; //稅率
		public String m_Strtax_type;//稅率類型
		//public int m_inttax_fee; //稅率金額
		

		public List<product_Var> product;
		public product_spec_Var()
		{
			product_sid = -1;
			product_price = 0;
			product_code = "";
			product_name = "";
			blnspec = false;

			m_Strproduct_type = "";//類型

			/*
			m_intdiscount_type = -1;// 折扣/折讓 旗標
			m_Strdiscount_name = "";// 折扣/折讓 說明文字
			m_Strdiscount_code = "";
			m_intdiscount_rate = 0;// 折扣率
			m_intdiscount_fee = 0;// 折讓金額
			m_Strdiscount_info = ""; //JSON
			*/

			m_inttax_sid = -1; //稅率編號
			m_inttax_rate = 0; //稅率
			m_Strtax_type = "";//稅率類型
			//m_inttax_fee = 0; //稅率金額

			product = new List<product_Var>();
		}
	}

	public class condiment_Var
	{
		public int condiment_sid;
		public string condiment_code;
		public string condiment_name;
		public double condiment_price;
		public int group_sid;
		public List<int> same_group_sid = new List<int>();
		public int min_count;
		public int max_count;

		public String m_Strcondiment_type;//類型

		/*
		public int m_intdiscount_type;// 折扣/折讓 旗標
		public String m_Strdiscount_name;// 折扣/折讓 說明文字
		public String m_Strdiscount_code;
		public int m_intdiscount_rate;// 折扣率
		public int m_intdiscount_fee;// 折讓金額
		public String m_Strdiscount_info; //JSON
		*/

		public int m_inttax_sid; //稅率編號 ; 產品若沒有稅率資料[m_tax_sid=0]，就直接從company和tax_data取出預設值
		public int m_inttax_rate; //稅率
		public String m_Strtax_type;//稅率類型
		//public int m_inttax_fee; //稅率金額

		public condiment_Var()
        {
			condiment_sid = -1;
			condiment_code = "";
			condiment_name = "";
			condiment_price = 0;
			group_sid = -1;
			same_group_sid = new List<int>();
			min_count = -1;
			max_count = -1;

			m_Strcondiment_type = "";//類型

			/*
			m_intdiscount_type = -1;// 折扣/折讓 旗標
			m_Strdiscount_name = "";// 折扣/折讓 說明文字
			m_Strdiscount_code = "";
			m_intdiscount_rate = 0;// 折扣率
			m_intdiscount_fee = 0;// 折讓金額
			m_Strdiscount_info = ""; //JSON
			*/

			m_inttax_sid = -1; //稅率編號
			m_inttax_rate = 0; //稅率
			m_Strtax_type = "";//稅率類型
			//m_inttax_fee = 0; //稅率金額
		}
	}

	public class DB2Model
    {
		public static int String2Int32(String Value)
        {
			int intResult = 0;
			if((Value != null)&&(Value.Length > 0))
            {
				intResult = (int)Convert.ToDouble(Value);
            }
			return intResult;
        }
		public static double String2Double(String Value)
		{
			double dblResult = 0;
			if ((Value != null) && (Value.Length > 0))
			{
				dblResult = Convert.ToDouble(Value);
			}
			return dblResult;
		}
	}//DB2Model
}