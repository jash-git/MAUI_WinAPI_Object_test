using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class SetMeal
    {
        public string att_name { get; set; }
        public List<Product> product { get; set; }
		public SetMeal() 
		{
			att_name = "";
            product = new List<Product>();
        }
    }

    public class Product
    {
        public object item_no { get; set; }
        public object category_code { get; set; }
        public object category_name { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public object price { get; set; }
        public object quantity { get; set; }
        public object amount { get; set; }
        public object subtotal { get; set; }
        public object del_flag { get; set; }
        public object state_memo { get; set; }
        public object update_user_sid { get; set; }
        public object update_user_name { get; set; }
        public List<Condiment> condiments { get; set; }

        public Product()
		{
            condiments= new List<Condiment>();
        }
    }
    public class Condiment
	{
		public string condiment_code { get; set; }
		public string condiment_name { get; set; }
		public int price { get; set; }
		public int count { get; set; }
		public int quantity { get; set; }
		public int subtotal { get; set; }
		public int amount { get; set; }
	}
	public class DiscountInfo
	{
		public int product_sid { get; set; }
		public string product_code { get; set; }
		public int subtotal { get; set; }
		public string showname { get; set; }
		public string hotkey_code { get; set; }
		public string hotkey_name { get; set; }
		public string discount_type { get; set; }
		public string discount_code { get; set; }
		public string discount_name { get; set; }
		public string val_mode { get; set; }
		public int val { get; set; }
		public int quantity { get; set; }
		public int amount { get; set; }
	}

	public class OrderItem
	{
		public int item_no { get; set; }
		public string category_code { get; set; }
		public string category_name { get; set; }
		public string item_type { get; set; }
		public string product_type { get; set; }
		public string product_code { get; set; }
		public string product_name { get; set; }
		public int price { get; set; }
		public int count { get; set; }
		public int quantity { get; set; }
		public int subtotal { get; set; }
		public string discount_code { get; set; }
		public string discount_name { get; set; }
		public string discount_type { get; set; }
		public int discount_rate { get; set; }
		public int discount_fee { get; set; }
		public DiscountInfo discount_info { get; set; }
		public string external_id { get; set; }
		public string external_mode { get; set; }
		public int stock_remainder_quantity { get; set; }
		public int stock_push_price { get; set; }
		public int stock_push_quantity { get; set; }
		public int stock_push_amount { get; set; }
		public string stock_pull_code { get; set; }
		public string stock_pull_name { get; set; }
		public int stock_pull_price { get; set; }
		public int stock_pull_quantity { get; set; }
		public int stock_pull_amount { get; set; }
		public string tax_type { get; set; }
		public int tax_rate { get; set; }
		public int tax_fee { get; set; }
		public int amount { get; set; }
		public string customer_name { get; set; }
		public string print_flag { get; set; }
		public List<SetMeal> set_meals { get; set; }
        public List<Condiment> condiments { get; set; }
		public List<Printer> printers { get; set; }
        public List<MaterialList> material_list { get; set; }//材料列表
		public string CompareString { get; set; }//產品名稱;配料名稱01;配料名稱02;.....
        public OrderItem()
		{
			set_meals = new List<SetMeal>();
            condiments = new List<Condiment>();
			printers = new List<Printer>();
            material_list = new List<MaterialList>();
        }

	}

	public class Package
	{
		public string package_code { get; set; }
		public string package_name { get; set; }
		public int price { get; set; }
		public int count { get; set; }
		public int quantity { get; set; }
        public int amount { get; set; }
	}

	public class J2C_Payment
	{
		public int payment_sid { get; set; }
		public string payment_code { get; set; }
		public string payment_name { get; set; }
		public string payment_module_code { get; set; }
		public int payment_amount { get; set; }
		public int received_fee { get; set; }
		public int change_fee { get; set; }
		public int payment_time { get; set; }
		public string payment_info { get; set; }
	}

	public class Printer
	{
		public int printer_group_sid { get; set; }
		public int printer_order_type { get; set; }
		public int product_sid { get; set; }
		public string product_code { get; set; }
	}

	public class Invoice_Data
	{
        public string inv_period { get; set; }
        public string inv_no { get; set; }
        public string cust_ein { get; set; }
        public string donate_flag { get; set; }
        public string donate_code { get; set; }
        public string carrier_type { get; set; }
        public string carrier_code_1 { get; set; }
        public string carrier_code_2 { get; set; }
        public int batch_num { get; set; }
        public string random_code { get; set; }
        public string qrcode_aes_key { get; set; }
    }

    public class orders_new//訂單 上傳/列印 資料結構
    {
		public string store_name { get; set; }
        public string pos_ver { get; set; }//version
		public string device_code { get; set; }
		public string order_no { get; set; }
		public string order_no_from { get; set; }
		public int order_time { get; set; }
		public int order_open_time { get; set; }
		public int order_state { get; set; }
		public string order_type { get; set; }
		public string order_type_name { get; set; }
		public string order_type_code { get; set; }
		public string terminal_sid { get; set; }
		public string pos_no { get; set; }
		public string class_name { get; set; }
		public string employee_no { get; set; }
		public string table_code { get; set; }
		public string table_name { get; set; }
		public string call_num { get; set; }
		public string meal_num { get; set; }
		public string member_flag { get; set; }
		public string member_no { get; set; }
		public string member_platform { get; set; }
		public string member_name { get; set; }
		public string member_phone { get; set; }
		public string outside_order_no { get; set; }
		public string outside_description { get; set; }
		public string takeaways_order_sid { get; set; }
		public string delivery_city_name { get; set; }
		public string delivery_district_name { get; set; }
		public string delivery_address { get; set; }
		public int item_count { get; set; }
		public int subtotal { get; set; }
		public int discount_fee { get; set; }
		public int promotion_fee { get; set; }
		public int coupon_discount { get; set; }
		public int stock_push_quantity { get; set; }
		public int stock_push_amount { get; set; }
		public int stock_pull_quantity { get; set; }
		public int stock_pull_amount { get; set; }
		public int service_fee { get; set; }
		public int service_rate { get; set; }
		public int trans_reversal { get; set; }
		public int over_paid { get; set; }
		public int tax_fee { get; set; }
		public int amount { get; set; }
		public string paid_flag { get; set; }
		public int cash_fee { get; set; }
		public int change_fee { get; set; }
		public string cust_ein { get; set; }
		public string invoice_flag { get; set; }
		public int business_day { get; set; }
		public string cancel_flag { get; set; }
		public int cancel_time { get; set; }
		public string cancel_class_name { get; set; }
		public string del_flag { get; set; }
		public int refund { get; set; }
		public string refund_type_sid { get; set; }
		public string remarks { get; set; }
		public List<OrderItem> order_items { get; set; }
		public int product_sale_countCS0102 { get; set; }//product_sale_count
		public List<Package> packages { get; set; }
		public int set_product_sale_count { get; set; }
		public int package_sale_count { get; set; }
		public List<J2C_Payment> payments { get; set; }
		public List<object> coupons { get; set; }
		public int company_sid { get; set; }
		public string upload_terminal_sid { get; set; }
		public string upload_ip_address { get; set; }
		public string license_type { get; set; }
		public Invoice_Data invoice_data { get; set; }
		public string strQrcodeInfor { get; set; }

        public orders_new()
		{
			order_items = new List<OrderItem>();
			packages = new List<Package>();
			payments = new List<J2C_Payment>();
			coupons = new List<object>();
            invoice_data = new Invoice_Data();
			strQrcodeInfor = "";
        }

		public void mergeItems(bool blnSort=true)
		{
            List<OrderItem> orderItemBuf = new List<OrderItem>();
            orderItemBuf.Clear();
            if(order_items.Count>0)
			{
                OrderItem orderItem = order_items[0];
                orderItemBuf.Add(orderItem);
            }

            for (int i = 1; i < order_items.Count; i++)
			{
				bool blnfind = false;
                OrderItem orderItemM = order_items[i];

                for (int j=0;j< orderItemBuf.Count;j++)
				{
                    if (orderItemM.CompareString == orderItemBuf[j].CompareString)
					{
                        orderItemBuf[j].count += orderItemM.count;
                        orderItemBuf[j].quantity += orderItemM.count;
                        blnfind = true;
                        break;
					}
				}

				if(!blnfind)//未找到
				{
                    orderItemBuf.Add(orderItemM);
                }
			}

			if(blnSort)
			{
                orderItemBuf.Sort((x, y) => {
                    //int ret = (x.item_no> y.item_no)? x.item_no: y.item_no;
                    int ret = String.Compare(x.product_name, y.product_name);
                    return ret;
                });
            }

            order_items = orderItemBuf;
        }


        public void printer_groupFilter(int intOrderTypeSID,int intPrinterSID,bool blnLable=true)//印表群組過濾函數
		{
			/*
			當N筆印表群組 綁定 相同印表機&訂單類型時 就會讓同一筆商品資料的列印資料數量變成 N
			輸出到標籤機的箝制動作
			*/
            List<OrderItem> orderItemBuf = new List<OrderItem>();
			orderItemBuf.Clear();

			DataRow[] foundRows = null;

			if(blnLable)//貼紙過濾
			{
                foundRows = SQLDataTableModel.m_printer_groupDataTable3.Select($"printer_sid='{intPrinterSID}' AND order_type_sid='{intOrderTypeSID}'");
                
				for (int j = 0; j < order_items.Count; j++)//for (int i = 0; i < foundRows.Length; i++)
                {
                    for (int i = 0; i < foundRows.Length; i++)//for (int j = 0; j < order_items.Count; j++)
                    {
                        OrderItem orderItem = order_items[j];

                        if (foundRows[i]["product_code"].ToString() == orderItem.product_code)
                        {
                            orderItemBuf.Add(orderItem);
                            break;
                        }
                    }
                }

                order_items = orderItemBuf;
            }
            else//號碼單過濾
			{
                foundRows = SQLDataTableModel.m_printer_groupDataTable2.Select($"printer_sid='{intPrinterSID}' AND order_type_sid='{intOrderTypeSID}'");
                if (foundRows.Length == 0 ) 
				{
					order_no = "";//表示不用執行列印
                }
			}

        }

    }
	public class orders_newResult
	{
		public string status { get; set; }
		public string message { get; set; }
	}

	public class DB2orders_new//訂單資料庫資料轉 訂單上傳/列印資料結構
    {
		public static void order_invoice_data2Var(String data_no, ref orders_new orders_newBuf)
		{
            String SQL = "";
            SQL = String.Format("SELECT * FROM order_invoice_data WHERE order_no='{0}' LIMIT 0,1", data_no);
            DataTable order_invoice_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_invoice_dataDataTable.Rows.Count > 0)
			{
				orders_newBuf.invoice_data.inv_period = order_invoice_dataDataTable.Rows[0]["inv_period"].ToString();
                orders_newBuf.invoice_data.inv_no = order_invoice_dataDataTable.Rows[0]["inv_no"].ToString();
                orders_newBuf.invoice_data.cust_ein = order_invoice_dataDataTable.Rows[0]["cust_ein"].ToString();
                orders_newBuf.invoice_data.donate_flag = order_invoice_dataDataTable.Rows[0]["donate_flag"].ToString();
                orders_newBuf.invoice_data.donate_code = order_invoice_dataDataTable.Rows[0]["donate_code"].ToString();
                orders_newBuf.invoice_data.carrier_type = order_invoice_dataDataTable.Rows[0]["carrier_type"].ToString();
                orders_newBuf.invoice_data.carrier_code_1 = order_invoice_dataDataTable.Rows[0]["carrier_code_1"].ToString();
                orders_newBuf.invoice_data.carrier_code_2 = order_invoice_dataDataTable.Rows[0]["carrier_code_2"].ToString();
                orders_newBuf.invoice_data.batch_num = DB2Model.String2Int32(order_invoice_dataDataTable.Rows[0]["batch_num"].ToString());
                orders_newBuf.invoice_data.random_code = order_invoice_dataDataTable.Rows[0]["random_code"].ToString();
                orders_newBuf.invoice_data.qrcode_aes_key = order_invoice_dataDataTable.Rows[0]["qrcode_aes_key"].ToString();
            }

        }

        public static void company2Var(ref orders_new orders_newBuf)
		{
            String SQL = "SELECT SID,company_name FROM company LIMIT 0,1";
            DataTable companyDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (companyDataTable.Rows.Count > 0)
			{
                orders_newBuf.store_name = companyDataTable.Rows[0]["company_name"].ToString();
            }

            orders_newBuf.device_code = MainPage.m_StrDeviceCode;
            orders_newBuf.license_type = "POS";
            orders_newBuf.pos_ver = $"{MainPage.m_StrVersion}";
        }

        public static void order_data2Var(String data_no,ref orders_new orders_newBuf)
        {
			String SQL = "";
			SQL = String.Format("SELECT * FROM order_data WHERE order_no='{0}' LIMIT 0,1", data_no);
			DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
			if(order_dataDataTable.Rows.Count>0)
            {
				orders_newBuf.amount = DB2Model.String2Int32(order_dataDataTable.Rows[0]["amount"].ToString());
				orders_newBuf.business_day = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_dataDataTable.Rows[0]["business_day"].ToString()));
				orders_newBuf.call_num = order_dataDataTable.Rows[0]["call_num"].ToString();
				orders_newBuf.cancel_class_name = order_dataDataTable.Rows[0]["cancel_class_name"].ToString();
				orders_newBuf.cancel_flag = order_dataDataTable.Rows[0]["cancel_flag"].ToString();
				orders_newBuf.cancel_time = (order_dataDataTable.Rows[0]["cancel_flag"].ToString()=="N")?0: (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_dataDataTable.Rows[0]["cancel_time"].ToString()));
				orders_newBuf.cash_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["cash_fee"].ToString());
				orders_newBuf.change_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["change_fee"].ToString());
				orders_newBuf.class_name = order_dataDataTable.Rows[0]["class_name"].ToString();
				orders_newBuf.company_sid = Convert.ToInt32(SqliteDataAccess.m_terminal_data[0].company_sid);
				//orders_newBuf.coupons
				orders_newBuf.coupon_discount = DB2Model.String2Int32(order_dataDataTable.Rows[0]["coupon_discount"].ToString());
				orders_newBuf.cust_ein = order_dataDataTable.Rows[0]["cust_ein"].ToString();
				orders_newBuf.delivery_address = order_dataDataTable.Rows[0]["delivery_address"].ToString();
				orders_newBuf.delivery_city_name = order_dataDataTable.Rows[0]["delivery_city_name"].ToString();
				orders_newBuf.delivery_district_name = order_dataDataTable.Rows[0]["delivery_district_name"].ToString();
				orders_newBuf.del_flag = order_dataDataTable.Rows[0]["del_flag"].ToString();
				orders_newBuf.discount_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["discount_fee"].ToString());
				orders_newBuf.employee_no = order_dataDataTable.Rows[0]["employee_no"].ToString();
				orders_newBuf.invoice_flag = order_dataDataTable.Rows[0]["invoice_flag"].ToString();
				orders_newBuf.item_count = DB2Model.String2Int32(order_dataDataTable.Rows[0]["item_count"].ToString());
				orders_newBuf.set_product_sale_count = 0;//套餐數量
				orders_newBuf.meal_num = order_dataDataTable.Rows[0]["meal_num"].ToString();
				orders_newBuf.member_flag = order_dataDataTable.Rows[0]["member_flag"].ToString();
				orders_newBuf.member_name = order_dataDataTable.Rows[0]["member_name"].ToString();
				orders_newBuf.member_no = order_dataDataTable.Rows[0]["member_no"].ToString();
				orders_newBuf.member_phone = order_dataDataTable.Rows[0]["member_phone"].ToString();
				orders_newBuf.member_platform = order_dataDataTable.Rows[0]["member_platform"].ToString();
				orders_newBuf.order_no = order_dataDataTable.Rows[0]["order_no"].ToString();
				orders_newBuf.order_no_from = order_dataDataTable.Rows[0]["order_no_from"].ToString();
				orders_newBuf.order_open_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_dataDataTable.Rows[0]["order_open_time"].ToString()));
				orders_newBuf.order_state = DB2Model.String2Int32(order_dataDataTable.Rows[0]["order_state"].ToString());
				orders_newBuf.order_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_dataDataTable.Rows[0]["order_time"].ToString()));
				orders_newBuf.order_type = order_dataDataTable.Rows[0]["order_type"].ToString();
				orders_newBuf.order_type_code = order_dataDataTable.Rows[0]["order_type_code"].ToString();
				orders_newBuf.order_type_name = order_dataDataTable.Rows[0]["order_type_name"].ToString();
				orders_newBuf.outside_description = order_dataDataTable.Rows[0]["outside_description"].ToString();
				orders_newBuf.outside_order_no = order_dataDataTable.Rows[0]["outside_order_no"].ToString();
				orders_newBuf.over_paid = DB2Model.String2Int32(order_dataDataTable.Rows[0]["over_paid"].ToString());
				orders_newBuf.paid_flag = order_dataDataTable.Rows[0]["paid_flag"].ToString();
				orders_newBuf.pos_no = order_dataDataTable.Rows[0]["pos_no"].ToString();
				orders_newBuf.promotion_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["promotion_fee"].ToString());
				orders_newBuf.refund = DB2Model.String2Int32(order_dataDataTable.Rows[0]["refund"].ToString());
				orders_newBuf.refund_type_sid = order_dataDataTable.Rows[0]["refund_type_sid"].ToString();
				orders_newBuf.remarks = order_dataDataTable.Rows[0]["remarks"].ToString();
				orders_newBuf.service_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["service_fee"].ToString());
				orders_newBuf.service_rate = DB2Model.String2Int32(order_dataDataTable.Rows[0]["service_rate"].ToString());
				orders_newBuf.stock_pull_amount = DB2Model.String2Int32(order_dataDataTable.Rows[0]["stock_pull_amount"].ToString());
				orders_newBuf.stock_pull_quantity = DB2Model.String2Int32(order_dataDataTable.Rows[0]["stock_pull_quantity"].ToString());
				orders_newBuf.stock_push_amount = DB2Model.String2Int32(order_dataDataTable.Rows[0]["stock_push_amount"].ToString());
				orders_newBuf.stock_push_quantity = DB2Model.String2Int32(order_dataDataTable.Rows[0]["stock_push_quantity"].ToString());
				orders_newBuf.subtotal = DB2Model.String2Int32(order_dataDataTable.Rows[0]["subtotal"].ToString());
				orders_newBuf.table_code = order_dataDataTable.Rows[0]["table_code"].ToString();
				orders_newBuf.table_name = order_dataDataTable.Rows[0]["table_name"].ToString();
				orders_newBuf.takeaways_order_sid = order_dataDataTable.Rows[0]["takeaways_order_sid"].ToString();
				orders_newBuf.tax_fee = DB2Model.String2Int32(order_dataDataTable.Rows[0]["tax_fee"].ToString());
				orders_newBuf.terminal_sid = order_dataDataTable.Rows[0]["terminal_sid"].ToString();
				orders_newBuf.trans_reversal = DB2Model.String2Int32(order_dataDataTable.Rows[0]["trans_reversal"].ToString());
				orders_newBuf.upload_ip_address = "";//SERVER端 填入欄位
				orders_newBuf.upload_terminal_sid = order_dataDataTable.Rows[0]["terminal_sid"].ToString();

			}//if(order_dataDataTable.Rows.Count>0)

		}//public static void order_data2Var(String data_no,ref orders_new orders_newBuf)

		public static void order_content_data2Var(String data_no, ref orders_new orders_newBuf)
        {
			//orders_newBuf.order_items
			//orders_newBuf.packages
			String SQL = "";
			DataTable order_content_dataKDataTable = new DataTable();//包材
			DataTable order_content_dataPDataTable = new DataTable();//產品 & 套餐
            DataTable order_content_dataTPDataTable = new DataTable();//套餐的產品
            DataTable order_content_dataCDataTable = new DataTable();//調味品

			SQL = String.Format("SELECT * FROM order_content_data WHERE order_no='{0}' AND item_type='K' AND del_flag='N' ORDER BY {1}", data_no, "item_no");
			order_content_dataKDataTable = SQLDataTableModel.GetDataTable(SQL);

			orders_newBuf.package_sale_count = order_content_dataKDataTable.Rows.Count;//數量設定
			for (int i = 0; i < order_content_dataKDataTable.Rows.Count; i++)
			{
				Package PackageBuf = new Package();

				PackageBuf.amount = DB2Model.String2Int32(order_content_dataKDataTable.Rows[i]["item_subtotal"].ToString());
				PackageBuf.count = DB2Model.String2Int32(order_content_dataKDataTable.Rows[i]["item_count"].ToString());
                PackageBuf.quantity = DB2Model.String2Int32(order_content_dataKDataTable.Rows[i]["item_count"].ToString());
                PackageBuf.package_code = order_content_dataKDataTable.Rows[i]["item_code"].ToString();
				PackageBuf.package_name = order_content_dataKDataTable.Rows[i]["item_name"].ToString();
				PackageBuf.price = DB2Model.String2Int32(order_content_dataKDataTable.Rows[i]["item_cost"].ToString());

				orders_newBuf.packages.Add(PackageBuf);

			}//for (int i = 0; i < order_content_dataKDataTable.Rows.Count; i++)				
			orders_newBuf.product_sale_countCS0102 = orders_newBuf.item_count - orders_newBuf.packages.Count;

			SQL = String.Format("SELECT * FROM order_content_data WHERE order_no='{0}' AND (item_type='P' OR item_type='T') AND parent_item_no='0' AND del_flag='N' ORDER BY {1}", data_no, "item_no");
			order_content_dataPDataTable = SQLDataTableModel.GetDataTable(SQL);
			for (int i = 0; i < order_content_dataPDataTable.Rows.Count; i++)
            {
				OrderItem OrderItemBuf=new OrderItem();

				OrderItemBuf.amount = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_amount"].ToString());
				OrderItemBuf.category_code = "";
				OrderItemBuf.category_name = "";
				OrderItemBuf.count = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_count"].ToString());
				OrderItemBuf.customer_name = "";
				OrderItemBuf.discount_code = order_content_dataPDataTable.Rows[i]["discount_code"].ToString();
				OrderItemBuf.discount_fee = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["discount_fee"].ToString());
				OrderItemBuf.discount_name = order_content_dataPDataTable.Rows[i]["discount_name"].ToString();
				OrderItemBuf.discount_rate = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["discount_rate"].ToString());
				OrderItemBuf.discount_type = order_content_dataPDataTable.Rows[i]["discount_type"].ToString();
				OrderItemBuf.external_id = order_content_dataPDataTable.Rows[i]["external_id"].ToString();
				OrderItemBuf.external_mode = order_content_dataPDataTable.Rows[i]["external_mode"].ToString();
				OrderItemBuf.item_no = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_no"].ToString());
				OrderItemBuf.item_type = order_content_dataPDataTable.Rows[i]["item_type"].ToString();
				OrderItemBuf.price = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_cost"].ToString());
				//OrderItemBuf.printers
				//OrderItemBuf.print_flag
				OrderItemBuf.product_code = order_content_dataPDataTable.Rows[i]["item_code"].ToString();
				OrderItemBuf.product_name = order_content_dataPDataTable.Rows[i]["item_name"].ToString();
				OrderItemBuf.CompareString = OrderItemBuf.product_name + OrderItemBuf.product_code;
                OrderItemBuf.product_type = OrderItemBuf.item_type;
				OrderItemBuf.quantity = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_count"].ToString());
				OrderItemBuf.stock_pull_amount = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_pull_amount"].ToString());
				OrderItemBuf.stock_pull_code = order_content_dataPDataTable.Rows[i]["stock_pull_code"].ToString();
				OrderItemBuf.stock_pull_name = order_content_dataPDataTable.Rows[i]["stock_pull_name"].ToString();
				OrderItemBuf.stock_pull_price = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_pull_price"].ToString());
				OrderItemBuf.stock_pull_quantity = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_pull_quantity"].ToString());
				OrderItemBuf.stock_push_amount = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_push_amount"].ToString());
				OrderItemBuf.stock_push_price = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_push_price"].ToString());
				OrderItemBuf.stock_push_quantity = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_push_quantity"].ToString());
				OrderItemBuf.stock_remainder_quantity = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["stock_remainder_quantity"].ToString());
				OrderItemBuf.subtotal = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["item_subtotal"].ToString());
				OrderItemBuf.tax_fee = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["tax_fee"].ToString());
				OrderItemBuf.tax_rate = DB2Model.String2Int32(order_content_dataPDataTable.Rows[i]["tax_rate"].ToString());
				OrderItemBuf.tax_type = order_content_dataPDataTable.Rows[i]["tax_type"].ToString();
				
				String discount_info = order_content_dataPDataTable.Rows[i]["discount_info"].ToString();
				if( (discount_info != null) && (discount_info.Length > 0) )
                {
					OrderItemBuf.discount_info = JsonClassConvert.DiscountInfo2Class(discount_info);
					if (OrderItemBuf.discount_info.amount == 0)
					{
						OrderItemBuf.discount_info = null;
					}
				}
				else
                {
					OrderItemBuf.discount_info = null;
				}

				if(OrderItemBuf.product_type=="P")
				{
                    //SQL = String.Format("SELECT * FROM order_content_data WHERE order_no='{0}' AND item_type='C' AND del_flag='N' AND parent_item_no='{1}' ORDER BY {2}", data_no, OrderItemBuf.item_no, "item_name");
                    SQL = String.Format("Select a.* From order_content_data a LEFT JOIN condiment_data b ON b.condiment_code=a.item_code LEFT JOIN condiment_group c ON c.SID=b.group_sid Where a.del_flag='N' AND a.item_type='C' AND a.order_no='{0}' AND a.parent_item_no='{1}' Order By c.sort,a.item_name", data_no, OrderItemBuf.item_no);//收據(工作票、智能食譜)列印配料資料時，依據配料群組的排序方式進行列印
                    order_content_dataCDataTable = SQLDataTableModel.GetDataTable(SQL);
                    for (int j = 0; j < order_content_dataCDataTable.Rows.Count; j++)
                    {
                        Condiment CondimentBuf = new Condiment();

                        CondimentBuf.amount = DB2Model.String2Int32(order_content_dataCDataTable.Rows[j]["item_subtotal"].ToString());
                        CondimentBuf.condiment_code = order_content_dataCDataTable.Rows[j]["item_code"].ToString();
                        CondimentBuf.condiment_name = order_content_dataCDataTable.Rows[j]["item_name"].ToString();
                        OrderItemBuf.CompareString += $";{CondimentBuf.condiment_name + CondimentBuf.condiment_code}";
                        CondimentBuf.count = DB2Model.String2Int32(order_content_dataCDataTable.Rows[j]["item_count"].ToString());
                        CondimentBuf.quantity = DB2Model.String2Int32(order_content_dataCDataTable.Rows[j]["item_count"].ToString());
                        CondimentBuf.price = DB2Model.String2Int32(order_content_dataCDataTable.Rows[j]["item_cost"].ToString());
                        CondimentBuf.subtotal = DB2Model.String2Int32(order_content_dataCDataTable.Rows[j]["item_cost"].ToString());

                        OrderItemBuf.condiments.Add(CondimentBuf);
                    }//for(int j = 0; j < order_content_dataCDataTable.Rows.Count;j++)
                }
				else
				{//T
                    //SetMeal SetMealBuf=new SetMeal();
                    SQL = String.Format("SELECT * FROM order_content_data WHERE order_no='{0}' AND item_type='P' AND parent_item_no='{1}' AND del_flag='N' ORDER BY {2}", data_no, OrderItemBuf.item_no, "item_no");
                    order_content_dataTPDataTable = SQLDataTableModel.GetDataTable(SQL);
                    for (int j = 0; j < order_content_dataTPDataTable.Rows.Count; j++)
					{
                        SetMeal SetMealBuf = new SetMeal();
                        Product ProductBuf = new Product();
                        ProductBuf.amount = DB2Model.String2Int32(order_content_dataTPDataTable.Rows[j]["item_amount"].ToString());
                        ProductBuf.category_code = "";
                        ProductBuf.category_name = "";
                        ProductBuf.item_no = DB2Model.String2Int32(order_content_dataTPDataTable.Rows[j]["item_no"].ToString());
                        ProductBuf.type = order_content_dataTPDataTable.Rows[j]["item_type"].ToString();
                        ProductBuf.price = DB2Model.String2Int32(order_content_dataTPDataTable.Rows[j]["item_cost"].ToString());
                        ProductBuf.code = order_content_dataTPDataTable.Rows[j]["item_code"].ToString();
                        ProductBuf.name = order_content_dataTPDataTable.Rows[j]["item_name"].ToString();
                        ProductBuf.quantity = DB2Model.String2Int32(order_content_dataTPDataTable.Rows[j]["item_count"].ToString());
                        ProductBuf.subtotal = DB2Model.String2Int32(order_content_dataTPDataTable.Rows[j]["item_subtotal"].ToString());

                        SQL = String.Format("Select a.* From order_content_data a LEFT JOIN condiment_data b ON b.condiment_code=a.item_code LEFT JOIN condiment_group c ON c.SID=b.group_sid Where a.del_flag='N' AND a.item_type='C' AND a.order_no='{0}' AND a.parent_item_no='{1}' Order By c.sort,a.item_name", data_no, ProductBuf.item_no);//收據(工作票、智能食譜)列印配料資料時，依據配料群組的排序方式進行列印
                        order_content_dataCDataTable = SQLDataTableModel.GetDataTable(SQL);
                        for (int k = 0; k < order_content_dataCDataTable.Rows.Count; k++)
                        {
                            Condiment CondimentBuf = new Condiment();

                            CondimentBuf.amount = DB2Model.String2Int32(order_content_dataCDataTable.Rows[k]["item_subtotal"].ToString());
                            CondimentBuf.condiment_code = order_content_dataCDataTable.Rows[k]["item_code"].ToString();
                            CondimentBuf.condiment_name = order_content_dataCDataTable.Rows[k]["item_name"].ToString();
                            CondimentBuf.count = DB2Model.String2Int32(order_content_dataCDataTable.Rows[k]["item_count"].ToString());
                            CondimentBuf.quantity = DB2Model.String2Int32(order_content_dataCDataTable.Rows[k]["item_count"].ToString());
                            CondimentBuf.price = DB2Model.String2Int32(order_content_dataCDataTable.Rows[k]["item_cost"].ToString());
                            CondimentBuf.subtotal = DB2Model.String2Int32(order_content_dataCDataTable.Rows[k]["item_cost"].ToString());

                            ProductBuf.condiments.Add(CondimentBuf);
                        }//for(int k = 0; k < order_content_dataCDataTable.Rows.Count;k++)
                        SetMealBuf.product.Add(ProductBuf);
                        OrderItemBuf.set_meals.Add(SetMealBuf);
                    }//for (int j = 0; j < order_content_dataTPDataTable.Rows.Count; j++)
                    
                }

				orders_newBuf.order_items.Add(OrderItemBuf);

			}//for (int i = 0; i < order_content_dataPDataTable.Rows.Count; i++)

            //---
            //修改已列印旗標狀態
			//[純粹紀錄是否有被執行列印 取決設備有無被啟動 && (列印執行序是否被執行 || 補印事件是否被執行)]
			//補印過濾機制是取決在上層呼叫程序的運算
            SQL = String.Format("UPDATE order_content_data SET print_flag='Y' WHERE order_no='{0}'", data_no);
			SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
            //---修改已列印旗標狀態
        }//public static void order_content_data2Var(String data_no, ref orders_new orders_newBuf)

        public static void order_payment_data2Var(String data_no, ref orders_new orders_newBuf)
        {
			//orders_newBuf.payments
			String SQL = "";
			DataTable order_payment_dataDataTable = new DataTable();

			SQL = String.Format("SELECT * FROM order_payment_data WHERE order_no='{0}' AND del_flag='N' ORDER BY item_no", data_no);
			order_payment_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
			for(int i=0;i<order_payment_dataDataTable.Rows.Count;i++)
            {
				J2C_Payment J2C_PaymentBuf=new J2C_Payment();

				J2C_PaymentBuf.change_fee = DB2Model.String2Int32(order_payment_dataDataTable.Rows[i]["change_fee"].ToString());
				J2C_PaymentBuf.payment_amount = DB2Model.String2Int32(order_payment_dataDataTable.Rows[i]["amount"].ToString());
				J2C_PaymentBuf.payment_code = order_payment_dataDataTable.Rows[i]["payment_code"].ToString();
				J2C_PaymentBuf.payment_info = order_payment_dataDataTable.Rows[i]["payment_info"].ToString().Replace(":null",":\"\"");
				J2C_PaymentBuf.payment_module_code = order_payment_dataDataTable.Rows[i]["payment_module_code"].ToString();
				J2C_PaymentBuf.payment_name = order_payment_dataDataTable.Rows[i]["payment_name"].ToString();
				J2C_PaymentBuf.payment_sid = DB2Model.String2Int32(order_payment_dataDataTable.Rows[i]["payment_sid"].ToString());
				J2C_PaymentBuf.payment_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_payment_dataDataTable.Rows[i]["payment_time"].ToString()));
				J2C_PaymentBuf.received_fee = DB2Model.String2Int32(order_payment_dataDataTable.Rows[i]["received_fee"].ToString());

				orders_newBuf.payments.Add(J2C_PaymentBuf);
			}
		}

		public static void product_memo2Var(String data_no, ref product_memo product_memoBuf)
		{
            String SQL = "";
            DataTable product_memoDataTable = new DataTable();

            SQL = String.Format("SELECT product_code,memo FROM product_data AS a WHERE  EXISTS ( SELECT item_code AS product_code FROM order_content_data WHERE order_no='{0}' AND (item_type='P' OR item_type='T') AND parent_item_no='0' AND del_flag='N' AND item_code = a.product_code ORDER BY item_no)", data_no);
            product_memoDataTable = SQLDataTableModel.GetDataTable(SQL);
            for (int i = 0; i < product_memoDataTable.Rows.Count; i++)
			{
                pmData pmDataBuf = new pmData();
				pmDataBuf.product_code = product_memoDataTable.Rows[i]["product_code"].ToString();
                pmDataBuf.memo = product_memoDataTable.Rows[i]["memo"].ToString();
                product_memoBuf.data.Add(pmDataBuf);
            }

        }

        public static void MaterialAddBOM(ref orders_new orders_newBuf, int index)//智能食譜增加BOM表資料
		{
			if((orders_newBuf.order_items[index].condiments.Count>0) && (SQLDataTableModel.m_formula_data.condiment_bom.Count>0))
			{
				for(int i=0;i< orders_newBuf.order_items[index].condiments.Count;i++)
				{
					for(int j=0;j< SQLDataTableModel.m_formula_data.condiment_bom.Count;j++)
					{
						if (orders_newBuf.order_items[index].condiments[i].condiment_code== SQLDataTableModel.m_formula_data.condiment_bom[j].condiment_code)
						{
                            MaterialList MaterialListBuf = new MaterialList();
                            MaterialListBuf.material_code = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_code;//材料編號
                            MaterialListBuf.material_name = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_name;//材料名
                            MaterialListBuf.material_value = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_value;//數量
                            MaterialListBuf.material_unit = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_unit;//單位
                            MaterialListBuf.print_bill = "Y";//帳單列印N
							MaterialListBuf.is_display = "Y";//列印配方
                            orders_newBuf.order_items[index].material_list.Add(MaterialListBuf);//對應產品增加該筆材料用量
                            break;
						}
					}
				}
			}
		}

        public static void MaterialCalculate (ref orders_new orders_newBuf, int index, FormulaDatum FormulaDatumBuf)//智能食譜計算
		{
			for(int i=0; i < FormulaDatumBuf.material_list.Count; i++)
			{
                MaterialList MaterialListBuf = new MaterialList();
                //MaterialListBuf= SQLDataTableModel.m_formula_data.formula_data[j].material_list[k];//依序取出該產品智能食譜的材料用量表
                MaterialListBuf.material_code = FormulaDatumBuf.material_list[i].material_code;//材料編號
                MaterialListBuf.material_name = FormulaDatumBuf.material_list[i].material_name;//材料名
                MaterialListBuf.material_value = FormulaDatumBuf.material_list[i].material_value;//數量
                MaterialListBuf.material_unit = FormulaDatumBuf.material_list[i].material_unit;//單位
                MaterialListBuf.print_bill = FormulaDatumBuf.material_list[i].print_bill;//帳單列印N
                MaterialListBuf.is_display = FormulaDatumBuf.material_list[i].is_display;//列印配方

                //---
                //進行配料觸發相對應計算
                for (int l = 0; l < orders_newBuf.order_items[index].condiments.Count; l++)
                {
                    for (int m = 0; m < FormulaDatumBuf.material_list[i].formula_list.Count; m++)
                    {
                        if (orders_newBuf.order_items[index].condiments[l].condiment_code == FormulaDatumBuf.material_list[i].formula_list[m].condiment_code)//配料編號比對
                        {
                            if (FormulaDatumBuf.material_list[i].formula_list[m].decline_level != "Y")//非降階(必須運算)
                            {
                                //---
                                //非降階運算
                                try
                                {
                                    switch (FormulaDatumBuf.material_list[i].formula_list[m].operator_key)
                                    {
                                        case "+":
                                            MaterialListBuf.material_value = "" + (Convert.ToDouble(MaterialListBuf.material_value) + Convert.ToDouble(FormulaDatumBuf.material_list[i].formula_list[m].operator_value));
                                            break;
                                        case "-":
                                            MaterialListBuf.material_value = "" + (Convert.ToDouble(MaterialListBuf.material_value) - Convert.ToDouble(FormulaDatumBuf.material_list[i].formula_list[m].operator_value));
                                            break;
                                        case "*":
                                            MaterialListBuf.material_value = "" + (Convert.ToDouble(MaterialListBuf.material_value) * Convert.ToDouble(FormulaDatumBuf.material_list[i].formula_list[m].operator_value));
                                            break;
                                        case "/":
                                            MaterialListBuf.material_value = "" + (Convert.ToDouble(MaterialListBuf.material_value) / Convert.ToDouble(FormulaDatumBuf.material_list[i].formula_list[m].operator_value));
                                            break;
                                        case "=":
                                            MaterialListBuf.material_value = FormulaDatumBuf.material_list[i].formula_list[m].operator_value;
                                            break;
                                    }
                                }
                                catch
                                {
                                    MaterialListBuf.material_value = "0.0";
                                }
                                //---非降階運算
                            }
                            break;//已經運算，跳離m迴圈
                        }//if (orders_newBuf.order_items[i].condiments[l].condiment_code == SQLDataTableModel.m_formula_data.formula_data[j].material_list[k].formula_list[m].condiment_code)

                    }//for(int m=0;m< SQLDataTableModel.m_formula_data.formula_data[j].material_list[k].formula_list.Count;m++)

                }//for (int l=0;l< orders_newBuf.order_items[i].condiments.Count;l++)
                 //---進行配料觸發相對應計算

                if (MaterialListBuf != null)
                {
                    orders_newBuf.order_items[index].material_list.Add(MaterialListBuf);//對應產品增加該筆材料用量
                }
            }
        }

        private static void MaterialChangeData(String StrProductCode, ref FormulaDatum FormulaDatumBuf)//降階資料替換
        {
            for (int i = 0; i < SQLDataTableModel.m_formula_data.formula_data.Count; i++)//依序取出智能食譜品項
            {
                if (StrProductCode == SQLDataTableModel.m_formula_data.formula_data[i].product_code)//產品編號比對
                {
                    FormulaDatumBuf.Clone(SQLDataTableModel.m_formula_data.formula_data[i]);
					break;
                }
            }
        }

        private static void MaterialDecline(OrderItem OrderItemBuf, ref FormulaDatum FormulaDatumBuf)//降階運算
		{
			//---
			//沒有降階資料直接返回
			if(FormulaDatumBuf.decline_list==null)
			{
				return;
			}
			if(FormulaDatumBuf.decline_list.Count==0)
			{
				return;
			}
			//---沒有降階資料直接返回

			//---
			//計算降階次數
			int count = 0;
			int Max = 2;
            for (int i = 0; i < OrderItemBuf.condiments.Count; i++)
			{
                for (int j = 0; j < FormulaDatumBuf.material_list.Count; j++)
				{
					for(int k=0; k < FormulaDatumBuf.material_list[j].formula_list.Count; k++)
					{
                        if (OrderItemBuf.condiments[i].condiment_code == FormulaDatumBuf.material_list[j].formula_list[k].condiment_code)//配料編號比對
						{
                            if (FormulaDatumBuf.material_list[j].formula_list[k].decline_level == "Y")//確定要降階運算
							{
								count++;//階階計算標+1
								break;//跳離k迴圈
							}
                        }
                    }//for(int k=0; k < FormulaDatumBuf.material_list[j].formula_list.Count; k++)

                    if (count>= Max)
					{
						break;//跳離j迴圈
                    }
                }//for (int j = 0; j < FormulaDatumBuf.material_list.Count; j++)

                if (count >= Max)
                {
                    break;//跳離i迴圈
                }
            }//for (int i = 0; i < OrderItemBuf.condiments.Count; i++)
             //---計算降階次數

            //---
            //切換降階資料
            if (count>0)
			{
				if(FormulaDatumBuf.decline_list.Count==1)
				{
					MaterialChangeData(FormulaDatumBuf.decline_list[0].to_product_code, ref FormulaDatumBuf);//降階資料替換
                }
				else
				{
					int level = 1;
					int index = 0;
					for(int i=0; i < FormulaDatumBuf.decline_list.Count;i++)
					{
						level = Convert.ToInt32(FormulaDatumBuf.decline_list[i].decline_level);
						index = i;
						if(count <= level)
						{
							break;
						}
                    }

                    MaterialChangeData(FormulaDatumBuf.decline_list[index].to_product_code, ref FormulaDatumBuf);//降階資料替換
                }
			}
			else
			{
				return;
			}
            //---切換降階資料
        }
        
		public static void material2Var(ref orders_new orders_newBuf)//智能食譜資料產生
		{
			for (int i = 0; i < orders_newBuf.order_items.Count; i++)//依序取出訂單項目
			{
				if (orders_newBuf.order_items[i].product_type == "P")//確定該筆資料為產品
				{
					//---
					//判斷該產品是否有對應的智能食譜機制
					for (int j = 0; j < SQLDataTableModel.m_formula_data.formula_data.Count; j++)//依序取出智能食譜品項
					{
						if (orders_newBuf.order_items[i].product_code == SQLDataTableModel.m_formula_data.formula_data[j].product_code)//產品編號比對
						{
							OrderItem OrderItemBuf = orders_newBuf.order_items[i];

                            FormulaDatum FormulaDatumBuf = new FormulaDatum();
							FormulaDatumBuf.Clone(SQLDataTableModel.m_formula_data.formula_data[j]);

							MaterialDecline(OrderItemBuf, ref FormulaDatumBuf);//降階運算
							MaterialCalculate(ref orders_newBuf, i, FormulaDatumBuf);//智能食譜計算
							MaterialAddBOM(ref orders_newBuf, i);//智能食譜增加BOM表資料
                            break;
                        }
					}
                    //---判斷該產品是否有對應的智能食譜機制

                    //---
                    //與智能食譜配料BOM表 材料用量計算
      //              for (int j = 0; j < SQLDataTableModel.m_formula_data.condiment_bom.Count; j++)
      //              {
						//for(int k=0; k< orders_newBuf.order_items[i].condiments.Count; k++)
						//{
						//	if (SQLDataTableModel.m_formula_data.condiment_bom[j].condiment_code == orders_newBuf.order_items[i].condiments[k].condiment_code)
						//	{
      //                          MaterialList MaterialListBuf = new MaterialList();
      //                          //MaterialListBuf= SQLDataTableModel.m_formula_data.formula_data[j].material_list[k];//依序取出該產品智能食譜的材料用量表
      //                          MaterialListBuf.material_code = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_code;//材料編號
      //                          MaterialListBuf.material_name = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_name;//材料名
      //                          MaterialListBuf.material_value = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_value;//數量
      //                          MaterialListBuf.material_unit = SQLDataTableModel.m_formula_data.condiment_bom[j].material_list[0].material_unit;//單位
						//		MaterialListBuf.print_bill = "N";//帳單列印N
      //                          MaterialListBuf.is_display = "Y";//列印配方

      //                          orders_newBuf.order_items[i].material_list.Add(MaterialListBuf);//對應產品增加該筆材料用量
      //                      }
						//}
      //              }
                    //---與智能食譜配料BOM表 材料用量計算
                }
            }
		}

    }
	//對應完整JSON
	//轉換線上工具 https://json2csharp.com/ https://jsonutils.com/
	/*
	{
		"version": "1.5.7.3",
		"device_code": "B38A2B57158BD6B82956F333F6D32F0CB5B08D05D3A1C339CC61815876D76855",
		"order_no": "20220524-0004",
		"order_no_from": "L",
		"order_time": 1653875523,
		"order_open_time": 1653385396,
		"order_state": 1,
		"order_type": "3",
		"order_type_name": "內用",
		"order_type_code": "xxxs",
		"terminal_sid": "VT-POS-2020-00002",
		"pos_no": "VTPOS202000002",
		"class_name": "早班",
		"employee_no": "vteam-1",
		"table_code": "",
		"table_name": "",
		"call_num": "005",
		"meal_num": "",
		"member_flag": "N",
		"member_no": "",
		"member_platform": "",
		"member_name": "",
		"member_phone": "",
		"outside_order_no": "",
		"outside_description": "",
		"takeaways_order_sid": "",
		"delivery_city_name": "",
		"delivery_district_name": "",
		"delivery_address": "",
		"item_count": 5,
		"subtotal": 180,
		"discount_fee": 0,
		"promotion_fee": 0,
		"coupon_discount": 0,
		"stock_push_quantity": 0,
		"stock_push_amount": 0,
		"stock_pull_quantity": 0,
		"stock_pull_amount": 0,
		"service_fee": 0,
		"service_rate": 0,
		"trans_reversal": 0,
		"over_paid": 0,
		"tax_fee": 9,
		"amount": 180,
		"paid_flag": "Y",
		"cash_fee": 0,
		"change_fee": 0,
		"cust_ein": "",
		"invoice_flag": "N",
		"business_day": 1653875512,
		"cancel_flag": "N",
		"cancel_time": 0,
		"cancel_class_name": "",
		"del_flag": "N",
		"refund": 0,
		"refund_type_sid": "",
		"remarks": "",
		"order_items": [
			{
				"item_no": 1,
				"category_code": "",
				"category_name": "",
				"item_type": "P",
				"product_type": "P",
				"product_code": "F03",
				"product_name": "鐵觀音",
				"price": 35,
				"count": 1,
				"condiments": [
					{
						"condiment_code": "C001",
						"condiment_name": "珍珠",
						"price": 10,
						"count": 1,
						"amount": 10
					},
					{
						"condiment_code": "C002",
						"condiment_name": "大珍珠",
						"price": 10,
						"count": 1,
						"amount": 10
					}
				],
				"quantity": 1,
				"subtotal": 35,
				"discount_code": "",
				"discount_name": "",
				"discount_type": "N",
				"discount_rate": 0,
				"discount_fee": 0,
				"external_id": "",
				"external_mode": "",
				"stock_remainder_quantity": 0,
				"stock_push_price": 0,
				"stock_push_quantity": 0,
				"stock_push_amount": 0,
				"stock_pull_code": "",
				"stock_pull_name": "",
				"stock_pull_price": 0,
				"stock_pull_quantity": 0,
				"stock_pull_amount": 0,
				"tax_type": "A",
				"tax_rate": 5,
				"tax_fee": 2,
				"amount": 35,
				"customer_name": "",
				"print_flag": "N",
				"printers": []
			},
			{
				"item_no": 2,
				"category_code": "",
				"category_name": "",
				"item_type": "P",
				"product_type": "P",
				"product_code": "F01",
				"product_name": "阿薩姆冰茶",
				"price": 25,
				"count": 1,
				"quantity": 1,
				"subtotal": 25,
				"discount_code": "",
				"discount_name": "",
				"discount_type": "N",
				"discount_rate": 0,
				"discount_fee": 0,
				"external_id": "",
				"external_mode": "",
				"stock_remainder_quantity": 0,
				"stock_push_price": 0,
				"stock_push_quantity": 0,
				"stock_push_amount": 0,
				"stock_pull_code": "",
				"stock_pull_name": "",
				"stock_pull_price": 0,
				"stock_pull_quantity": 0,
				"stock_pull_amount": 0,
				"tax_type": "A",
				"tax_rate": 5,
				"tax_fee": 1,
				"amount": 25,
				"customer_name": "",
				"print_flag": "N",
				"printers": []
			},
			{
				"item_no": 3,
				"category_code": "",
				"category_name": "",
				"item_type": "P",
				"product_type": "P",
				"product_code": "C01M",
				"product_name": "黃金綠茶(M)",
				"price": 45,
				"count": 1,
				"quantity": 1,
				"subtotal": 45,
				"discount_code": "",
				"discount_name": "",
				"discount_type": "N",
				"discount_rate": 0,
				"discount_fee": 0,
				"external_id": "",
				"external_mode": "",
				"stock_remainder_quantity": 0,
				"stock_push_price": 0,
				"stock_push_quantity": 0,
				"stock_push_amount": 0,
				"stock_pull_code": "",
				"stock_pull_name": "",
				"stock_pull_price": 0,
				"stock_pull_quantity": 0,
				"stock_pull_amount": 0,
				"tax_type": "A",
				"tax_rate": 5,
				"tax_fee": 2,
				"amount": 45,
				"customer_name": "",
				"print_flag": "N",
				"printers": []
			},
			{
				"item_no": 4,
				"category_code": "",
				"category_name": "",
				"item_type": "P",
				"product_type": "P",
				"product_code": "A01",
				"product_name": "阿土伯的透清涼",
				"price": 40,
				"count": 1,
				"quantity": 1,
				"subtotal": 40,
				"discount_code": "",
				"discount_name": "",
				"discount_type": "N",
				"discount_rate": 0,
				"discount_fee": 0,
				"external_id": "",
				"external_mode": "",
				"stock_remainder_quantity": 0,
				"stock_push_price": 0,
				"stock_push_quantity": 0,
				"stock_push_amount": 0,
				"stock_pull_code": "",
				"stock_pull_name": "",
				"stock_pull_price": 0,
				"stock_pull_quantity": 0,
				"stock_pull_amount": 0,
				"tax_type": "A",
				"tax_rate": 5,
				"tax_fee": 2,
				"amount": 40,
				"customer_name": "",
				"print_flag": "N",
				"printers": [
					{
						"printer_group_sid": 9,
						"printer_order_type": 3,
						"product_sid": 1,
						"product_code": "A01"
					}
				]
			},
			{
				"item_no": 5,
				"category_code": "",
				"category_name": "",
				"item_type": "P",
				"product_type": "P",
				"product_code": "F03",
				"product_name": "鐵觀音",
				"price": 35,
				"count": 1,
				"quantity": 1,
				"subtotal": 35,
				"discount_code": "",
				"discount_name": "",
				"discount_type": "N",
				"discount_rate": 0,
				"discount_fee": 0,
				"external_id": "",
				"external_mode": "",
				"stock_remainder_quantity": 0,
				"stock_push_price": 0,
				"stock_push_quantity": 0,
				"stock_push_amount": 0,
				"stock_pull_code": "",
				"stock_pull_name": "",
				"stock_pull_price": 0,
				"stock_pull_quantity": 0,
				"stock_pull_amount": 0,
				"tax_type": "A",
				"tax_rate": 5,
				"tax_fee": 2,
				"amount": 35,
				"customer_name": "",
				"print_flag": "N",
				"printers": []
			}
		],
		"packages": [
			{
				"package_code": "",
				"package_name": "大型塑膠袋",
				"price": 2,
				"count": 1,
				"amount": 2
			}
		],
		"product_sale_count": 5,
		"set_product_sale_count": 0,
		"package_sale_count": 0,
		"payments": [
			{
				"payment_sid": 1,
				"payment_code": "CASH",
				"payment_name": "現金",
				"payment_module_code": "",
				"payment_amount": 180,
				"received_fee": 0,
				"change_fee": 0,
				"payment_time": 1653875523,
				"payment_info": ""
			}
		],
		"coupons": [],
		"company_sid": 7,
		"upload_terminal_sid": "VT-POS-2020-00002",
		"upload_ip_address": "R:[203.69.151.102] ",
		"license_type": "POS"
	} 
	 */
}
