using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VPOS
{
    public class SyncThread//背景同步(上傳)資料到 cloud ; 抓取外部(線上)訂單資料
    {
        private static int m_intStepCount = 0;

        private static int m_intWaitCount = 0;//等待存取DB計數器
        public static bool m_blnWait = false;//預防同時存取DB旗標

        public static bool m_blnRunLoop = false;//執行序執行與否狀態變數
        public static void ThreadMain()//執行序的主函數
        {
            m_blnRunLoop = true;

            do
            {
                m_intStepCount++;

                if (m_blnWait)
                {
                    m_intWaitCount++;
                    if (m_intWaitCount >= 5)
                    {
                        m_blnWait = false;
                        m_intWaitCount = 0;
                    }
                }

                if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
                {
                    continue;
                }

                //---
                //抓取 線上訂單資料/ 外部訂單資料
                GetVTSTOREData();//每秒都執行一次 [抓取點點食訂單]
                GetUbereatsData();//每秒都執行一次 [抓取Ubereats訂單]
                GetFoodpandaData();//每秒都執行一次 [抓取Foodpanda訂單]
                //---抓取 線上訂單資料/ 外部訂單資料
                
                //---
                //抓取掃碼點單資訊
                GetQrorderPrintData();//每秒都執行一次 [抓取掃碼點單列印資料]
                //---抓取掃碼點單資訊

                if (m_intStepCount % 2 == 0)//2秒執行一次
                {
                }//2秒執行一次

                if (m_intStepCount % 3 == 0)//3秒執行一次
                {
                }//3秒執行一次

                if (m_intStepCount % 4 == 0)//4秒執行一次
                {
                }//4秒執行一次

                if (m_intStepCount % 5 == 0)//5秒執行一次
                {
                    KeepAlive();//回報設備存活
                    UploadData2Cloud();//上傳回報POS資料
                    m_intStepCount = 0;//清空旗標,重新計數
                }//5秒執行一次

                Thread.Sleep(1000);//每次執行中間間隔時間
            } while (m_blnRunLoop);

        }//public static void mainSync()

        public static bool m_blnGetVTSTOREData = true;
        private static void GetVTSTOREData()
        {
            if(m_blnGetVTSTOREData & m_blnGetUbereatsData & m_blnGetFoodpandaData & m_blnGetQrorderData & SQLDataTableModel.m_blnVTSTOREOpen)//同一DB 所以一方在前台操作 背景都要停止更新 
            {
                VTSTOREAPI.ordersnew();
            }        
        }

        public static bool m_blnGetUbereatsData = true;
        private static void GetUbereatsData()
        {
            if (m_blnGetVTSTOREData & m_blnGetUbereatsData & m_blnGetFoodpandaData & m_blnGetQrorderData & SQLDataTableModel.m_blnUBER_EATSOpen)//同一DB 所以一方在前台操作 背景都要停止更新 
            {
                UbereatsAPI.ordersnew();
            }
        }

        public static bool m_blnGetFoodpandaData = true;
        private static void GetFoodpandaData()
        {
            if (m_blnGetVTSTOREData & m_blnGetUbereatsData & m_blnGetFoodpandaData & m_blnGetQrorderData & SQLDataTableModel.m_blnFOODPANDAOpen)//同一DB 所以一方在前台操作 背景都要停止更新 
            {
                FoodpandaAPI.ordersnew();
            }
        }

        private static void GetQrorderPrintData()//抓取掃碼點單列印資料
        {
            ArrayList ALQueueSidState = new ArrayList();
            if (SQLDataTableModel.m_blnVTEAMQrorderOpen)
            {
                if(VTEAMQrorderAPI.get_print_queue_data())//取得
                {
                    for(int i=0;i< VTEAMQrorderAPI.m_get_print_queue_data.data.Count;i++)
                    {        
                        orders_new orders_newBuf = new orders_new();
                        DB2orders_new.company2Var(ref orders_newBuf);
                        //---
                        //order_data2Var
                        orders_newBuf.amount = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.subtotal!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.subtotal.ToString()):0;
                        orders_newBuf.business_day = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.generate_time));
                        orders_newBuf.call_num = "";
                        orders_newBuf.cancel_class_name = "";
                        orders_newBuf.cancel_flag = "N";
                        orders_newBuf.cancel_time = 0;
                        orders_newBuf.cash_fee = 0;
                        orders_newBuf.change_fee = 0;
                        orders_newBuf.class_name = "";
                        orders_newBuf.company_sid = Convert.ToInt32(SqliteDataAccess.m_terminal_data[0].company_sid);
                        //orders_newBuf.coupons
                        orders_newBuf.coupon_discount = 0;
                        orders_newBuf.cust_ein = "";
                        orders_newBuf.delivery_address = "";
                        orders_newBuf.delivery_city_name = "";
                        orders_newBuf.delivery_district_name = "";
                        orders_newBuf.del_flag = "N";
                        orders_newBuf.discount_fee = 0;
                        orders_newBuf.employee_no = "";
                        orders_newBuf.invoice_flag = "N";
                        orders_newBuf.item_count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.item_count!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.item_count.ToString()):0;
                        orders_newBuf.set_product_sale_count = 0;//套餐數量
                        orders_newBuf.meal_num = "";
                        orders_newBuf.member_flag = "N";
                        orders_newBuf.member_name = "";
                        orders_newBuf.member_no = "";
                        orders_newBuf.member_phone = "";
                        orders_newBuf.member_platform = "";
                        orders_newBuf.order_no = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_no;
                        orders_newBuf.order_no_from = "L";
                        orders_newBuf.order_open_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.generate_time));
                        orders_newBuf.order_state = 0;
                        orders_newBuf.order_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.generate_time));
                        orders_newBuf.order_type = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_type_sid.ToString();
                        orders_newBuf.order_type_code = "";
                        orders_newBuf.order_type_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_type_name;
                        orders_newBuf.outside_description = "";
                        orders_newBuf.outside_order_no = "";
                        orders_newBuf.over_paid = 0;
                        orders_newBuf.paid_flag = "";
                        orders_newBuf.pos_no = SqliteDataAccess.m_terminal_data[0].pos_no;
                        orders_newBuf.promotion_fee = 0;
                        orders_newBuf.refund = 0;
                        orders_newBuf.refund_type_sid = "";
                        orders_newBuf.remarks = "";
                        orders_newBuf.service_fee = 0;
                        orders_newBuf.service_rate = 0;
                        orders_newBuf.stock_pull_amount = 0;
                        orders_newBuf.stock_pull_quantity = 0;
                        orders_newBuf.stock_push_amount = 0;
                        orders_newBuf.stock_push_quantity = 0;
                        orders_newBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.subtotal!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.subtotal.ToString()):0;
                        orders_newBuf.table_code = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.table_code!=null)?VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.table_code:"";
                        orders_newBuf.table_name = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.table_name!=null)?VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.table_name:"";
                        orders_newBuf.takeaways_order_sid = "";
                        orders_newBuf.tax_fee = 0;
                        orders_newBuf.terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                        orders_newBuf.trans_reversal = 0;
                        orders_newBuf.upload_ip_address = "";//SERVER端 填入欄位
                        orders_newBuf.upload_terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;//terminal_sid
                        //---order_data2Var
                        orders_newBuf.strQrcodeInfor = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_url!=null) ? orders_newBuf.strQrcodeInfor = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_url:"";//掃碼點單網址

                        //---
                        //order_content_data2Var
                        if ((VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list.Count>0) &&(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items.Count>0))
                        {
                            for (int j = 0; j < VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items.Count;j++)
                            {
                                switch (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].type)
                                {
                                    case "K"://包裝
                                        Package PackageBuf = new Package();

                                        PackageBuf.amount = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount.ToString()):0;
                                        PackageBuf.count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        PackageBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        PackageBuf.package_code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].code;
                                        PackageBuf.package_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].name;
                                        PackageBuf.price = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].price!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].price.ToString()):0;

                                        orders_newBuf.packages.Add(PackageBuf);
                                        break;
                                    case "P"://產品
                                        OrderItem OrderItemBuf = new OrderItem();

                                        OrderItemBuf.amount = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount!=null)? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount.ToString()):0;
                                        OrderItemBuf.category_code = "";
                                        OrderItemBuf.category_name = "";
                                        OrderItemBuf.count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        OrderItemBuf.customer_name = "";
                                        OrderItemBuf.discount_code = "";
                                        OrderItemBuf.discount_fee = 0;
                                        OrderItemBuf.discount_name = "";
                                        OrderItemBuf.discount_rate = 0;
                                        OrderItemBuf.discount_type = "";
                                        OrderItemBuf.external_id = "";
                                        OrderItemBuf.external_mode = "";
                                        OrderItemBuf.item_no = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].item_no!=null!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].item_no.ToString()):0;
                                        OrderItemBuf.item_type = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].type;
                                        OrderItemBuf.price = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].price!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].price.ToString()):0;
                                        //OrderItemBuf.printers
                                        //OrderItemBuf.print_flag
                                        OrderItemBuf.product_code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].code;
                                        OrderItemBuf.product_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].name;
                                        OrderItemBuf.CompareString = OrderItemBuf.product_name + OrderItemBuf.product_code;
                                        OrderItemBuf.product_type = OrderItemBuf.item_type;
                                        OrderItemBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        OrderItemBuf.stock_pull_amount = 0;
                                        OrderItemBuf.stock_pull_code = "";
                                        OrderItemBuf.stock_pull_name = "";
                                        OrderItemBuf.stock_pull_price = 0;
                                        OrderItemBuf.stock_pull_quantity = 0;
                                        OrderItemBuf.stock_push_amount = 0;
                                        OrderItemBuf.stock_push_price = 0;
                                        OrderItemBuf.stock_push_quantity = 0;
                                        OrderItemBuf.stock_remainder_quantity = 0;
                                        OrderItemBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].item_no!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].subtotal.ToString()):0;
                                        OrderItemBuf.tax_fee = 0;
                                        OrderItemBuf.tax_rate = 0;
                                        OrderItemBuf.tax_type = "";

                                        OrderItemBuf.discount_info = null;
                                        if ((VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments.Count>0))
                                        {
                                            for (int k=0; k < VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments.Count; k++)
                                            {
                                                Condiment CondimentBuf = new Condiment();

                                                CondimentBuf.amount = DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].subtotal.ToString());
                                                CondimentBuf.condiment_code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].code;
                                                CondimentBuf.condiment_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].name;
                                                OrderItemBuf.CompareString += $";{CondimentBuf.condiment_name + CondimentBuf.condiment_code}";
                                                CondimentBuf.count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].quantity.ToString()):0;
                                                CondimentBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].quantity.ToString()):0;
                                                CondimentBuf.price = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].price!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].price.ToString()):0;
                                                CondimentBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].subtotal!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].condiments[k].subtotal.ToString()):0;

                                                OrderItemBuf.condiments.Add(CondimentBuf);
                                            }
                                        }
                                        orders_newBuf.order_items.Add(OrderItemBuf);
                                        break;
                                    case "T"://套餐
                                        OrderItem TOrderItemBuf = new OrderItem();

                                        TOrderItemBuf.amount = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].amount.ToString()):0;
                                        TOrderItemBuf.category_code = "";
                                        TOrderItemBuf.category_name = "";
                                        TOrderItemBuf.count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        TOrderItemBuf.customer_name = "";
                                        TOrderItemBuf.discount_code = "";
                                        TOrderItemBuf.discount_fee = 0;
                                        TOrderItemBuf.discount_name = "";
                                        TOrderItemBuf.discount_rate = 0;
                                        TOrderItemBuf.discount_type = "";
                                        TOrderItemBuf.external_id = "";
                                        TOrderItemBuf.external_mode = "";
                                        TOrderItemBuf.item_no = DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].item_no.ToString());
                                        TOrderItemBuf.item_type = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].type;
                                        TOrderItemBuf.price = DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].price.ToString());
                                        //OrderItemBuf.printers
                                        //OrderItemBuf.print_flag
                                        TOrderItemBuf.product_code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].code;
                                        TOrderItemBuf.product_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].name;
                                        TOrderItemBuf.product_type = TOrderItemBuf.item_type;
                                        TOrderItemBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].quantity.ToString()):0;
                                        TOrderItemBuf.stock_pull_amount = 0;
                                        TOrderItemBuf.stock_pull_code = "";
                                        TOrderItemBuf.stock_pull_name = "";
                                        TOrderItemBuf.stock_pull_price = 0;
                                        TOrderItemBuf.stock_pull_quantity = 0;
                                        TOrderItemBuf.stock_push_amount = 0;
                                        TOrderItemBuf.stock_push_price = 0;
                                        TOrderItemBuf.stock_push_quantity = 0;
                                        TOrderItemBuf.stock_remainder_quantity = 0;
                                        TOrderItemBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].subtotal!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].subtotal.ToString()):0;
                                        TOrderItemBuf.tax_fee = 0;
                                        TOrderItemBuf.tax_rate = 0;
                                        TOrderItemBuf.tax_type = "";
                                        for(int l=0;l< VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals.Count;l++)
                                        {
                                            SetMeal SetMealBuf = new SetMeal();
                                            for (int m=0; m<VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product.Count;m++)
                                            {
                                                Product ProductBuf = new Product();
                                                ProductBuf.item_no = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].item_no!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].item_no.ToString()):0;
                                                ProductBuf.type = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].type;
                                                ProductBuf.code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].code;
                                                ProductBuf.name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].name;
                                                ProductBuf.price = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].price!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].price.ToString()):0;
                                                ProductBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].quantity!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].quantity.ToString()):0;
                                                ProductBuf.amount = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].amount!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].amount.ToString()):0;
                                                ProductBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].subtotal!=null)?DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].subtotal.ToString()):0;

                                                if((VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments.Count>0))
                                                {
                                                    for (int k = 0; k < VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments.Count; k++)
                                                    {
                                                        Condiment CondimentBuf = new Condiment();

                                                        CondimentBuf.amount = DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].subtotal.ToString());
                                                        CondimentBuf.condiment_code = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].code;
                                                        CondimentBuf.condiment_name = VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].name;
                                                        CondimentBuf.count = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].quantity != null) ? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].quantity.ToString()) : 0;
                                                        CondimentBuf.quantity = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].quantity != null) ? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].quantity.ToString()) : 0;
                                                        CondimentBuf.price = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].price != null) ? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].price.ToString()) : 0;
                                                        CondimentBuf.subtotal = (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].subtotal != null) ? DB2Model.String2Int32(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items[j].set_meals[l].product[m].condiments[k].subtotal.ToString()) : 0;

                                                        ProductBuf.condiments.Add(CondimentBuf);
                                                    }
                                                }

                                                SetMealBuf.product.Add(ProductBuf);
                                            }

                                            TOrderItemBuf.set_meals.Add(SetMealBuf);
                                        }
                                        orders_newBuf.order_items.Add(TOrderItemBuf);
                                        break;
                                }
                            }//for (int j = 0; j < VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items.Count;j++)
                        }//if ((VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list.Count>0) &&(VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items!=null) && (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_data.order_list[0].items.Count>0))
                        //---order_content_data2Var
                        
                        String Strorders_new = JsonClassConvert.ordersnew2String(orders_newBuf);
                        PrintData PrintDataBuf = null;
                        switch (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_type)
                        {
                            case "BILL"://BILL: 收據
                                //---
                                //產生對應列印運算FIFO值
                                PrintDataBuf = new PrintData(12, Strorders_new);//12:Qrorder_BILL
                                lock (PrintThread.m_FIFOLock[0])
                                {
                                    ALQueueSidState.Add(VTEAMQrorderAPI.m_get_print_queue_data.data[i].queue_sid+ ";finish");//紀錄queue_sid
                                    PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                                }
                                //---產生對應列印運算FIFO值
                                break;
                            case "WORK_TICKET"://WORK_TICKET: 工作票
                                //---
                                //產生對應列印運算FIFO值
                                PrintDataBuf = new PrintData(13, Strorders_new);//13:Qrorder_WORK_TICKET
                                lock (PrintThread.m_FIFOLock[0])
                                {
                                    ALQueueSidState.Add(VTEAMQrorderAPI.m_get_print_queue_data.data[i].queue_sid + ";finish");//紀錄queue_sid
                                    PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                                }
                                //---產生對應列印運算FIFO值                                
                                break;
                            case "LABEL"://LABEL: 標籤貼紙
                                //---
                                //產生對應列印運算FIFO值
                                PrintDataBuf = new PrintData(14, Strorders_new);//14:Qrorder_LABEL
                                lock (PrintThread.m_FIFOLock[0])
                                {
                                    ALQueueSidState.Add(VTEAMQrorderAPI.m_get_print_queue_data.data[i].queue_sid + ";finish");//紀錄queue_sid
                                    PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                                }
                                //---產生對應列印運算FIFO值                                  
                                break;
                            case "QR_CODE"://QR_CODE: 點單憑證
                                //---
                                //產生對應列印運算FIFO值
                                PrintDataBuf = new PrintData(15, Strorders_new);//15:Qrorder_QrCode
                                lock (PrintThread.m_FIFOLock[0])
                                {
                                    ALQueueSidState.Add(VTEAMQrorderAPI.m_get_print_queue_data.data[i].queue_sid + ";finish");//紀錄queue_sid
                                    PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                                }
                                //---產生對應列印運算FIFO值       
                                break;
                        }//switch (VTEAMQrorderAPI.m_get_print_queue_data.data[i].print_type)
                    }//for(int i=0;i< VTEAMQrorderAPI.m_get_print_queue_data.data.Count;i++)
                    
                    if(ALQueueSidState.Count>0)
                    {
                        VTEAMQrorderAPI.update_print_queue_data(ALQueueSidState);
                    }
                }//if(VTEAMQrorderAPI.get_print_queue_data())//取得
            }//if (SQLDataTableModel.m_blnVTEAMQrorderOpen)
        }

        public static bool m_blnGetQrorderData = true;
        /*
        private static void GetQrorderData()
        {
            if (m_blnGetVTSTOREData & m_blnGetUbereatsData & m_blnGetFoodpandaData & m_blnGetQrorderData & SQLDataTableModel.m_blnVTEAMQrorderOpen)//同一DB 所以一方在前台操作 背景都要停止更新 
            {
                VTEAMQrorderAPI.get_order_list();//讀取待結帳的掃碼訂單清單
            }
        }
        */
        public static String Create_NOD_DOD_Json(String data_no)
        {
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);
            DB2orders_new.order_data2Var(data_no, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(data_no, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(data_no, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(data_no, ref orders_newBuf);

            String Strorders_new = JsonClassConvert.ordersnew2String(orders_newBuf);

            return Strorders_new;
        }

        private static void UploadData2Cloud()//上傳訂單資訊到雲端
        {
            if (SyncDBFun.UploadRowsCount() > 0)
            {
                for (int i = 0; i < SyncDBFun.m_upload_dataDataTable.Rows.Count; i++)
                {
                    String SID, data_no, upload_type;
                    int try_count = 0;

                    String StrResult = "";

                    upload_type = SyncDBFun.m_upload_dataDataTable.Rows[i]["upload_type"].ToString();
                    SID = SyncDBFun.m_upload_dataDataTable.Rows[i]["SID"].ToString();
                    data_no = SyncDBFun.m_upload_dataDataTable.Rows[i]["data_no"].ToString();
                    try_count = DB2Model.String2Int32(SyncDBFun.m_upload_dataDataTable.Rows[i]["try_count"].ToString());

                    if ((upload_type == "NOD") || (upload_type == "DOD")) // 新增訂單/取消訂單
                    {
                        /*
                        orders_new orders_newBuf = new orders_new();
                        DB2orders_new.company2Var(ref orders_newBuf);
                        DB2orders_new.order_data2Var(data_no, ref orders_newBuf);
                        DB2orders_new.order_content_data2Var(data_no, ref orders_newBuf);
                        DB2orders_new.order_payment_data2Var(data_no, ref orders_newBuf);

                        String Strorders_new = JsonClassConvert.ordersnew2String(orders_newBuf);
                        */
                        String Strorders_new = Create_NOD_DOD_Json(data_no);

                        HttpsFun.setDomainMode(1);//POS //HttpsFun.setDomainMode(2);//vdes
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/orders/new", Strorders_new, "Authorization", "Basic " + SyncDBData.m_StrEncoded); //StrResult = HttpsFun.RESTfulAPI_postBody("/api/vtcloud/orders/new", Strorders_new, "Authorization", "Bearer " + VTEAMCloudAPI.m_Straccess_token);
                    }//if((upload_type=="NOD") || (upload_type == "DOD"))

                    if (upload_type == "COD")//作廢訂單(已結帳後作廢)
                    {
                        orders_cancel orders_cancelBuf = new orders_cancel();
                        DB2orders_cancel.order_data2Var(data_no, ref orders_cancelBuf);
                        String Strorders_cancel = JsonClassConvert.orderscancelInput2String(orders_cancelBuf);
                        HttpsFun.setDomainMode(1);//POS HttpsFun.setDomainMode(2);//vdes
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/orders/cancel", Strorders_cancel, "Authorization", "Basic " + SyncDBData.m_StrEncoded); //StrResult = HttpsFun.RESTfulAPI_postBody("/api/vtcloud/orders/cancel", Strorders_cancel, "Authorization", "Bearer " + VTEAMCloudAPI.m_Straccess_token);
                    }//if (upload_type == "COD")

                    if (upload_type == "CRP")//交班報表
                    {
                        daily_report daily_reportBuf = new daily_report();
                        DB2daily_report.daily_report2Var(data_no, ref daily_reportBuf);
                        String Strdaily_report = JsonClassConvert.daily_report2String(daily_reportBuf);
                        HttpsFun.setDomainMode(1);//POS
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/report/class", Strdaily_report, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                    }//if(upload_type == "CRP")

                    if (upload_type == "DRP")//關帳報表
                    {
                        daily_report daily_reportBuf = new daily_report();
                        DB2daily_report.daily_report2Var(data_no, ref daily_reportBuf);
                        String Strdaily_report = JsonClassConvert.daily_report2String(daily_reportBuf);
                        HttpsFun.setDomainMode(1);//POS
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/report/daily", Strdaily_report, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                    }//if (upload_type == "DRP")

                    if (upload_type == "NEP")//新增收支紀錄
                    {
                        expense_new expense_newBuf = new expense_new();
                        DB2expense_new.expense_new2Var(data_no, ref expense_newBuf);
                        String Strexpense_new = JsonClassConvert.expense_new2String(expense_newBuf);
                        HttpsFun.setDomainMode(1);//POS
                        //API: upload/expense/new
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/upload/expense/new", Strexpense_new, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                    }

                    if (upload_type == "DEP")//刪除收支紀錄
                    {
                        expense_cancel expense_cancelBuf = new expense_cancel();
                        DB2expense_cancel.expense_cancel2Var(data_no, ref expense_cancelBuf);
                        String Strexpense_cancel = JsonClassConvert.expense_cancel2String(expense_cancelBuf);
                        HttpsFun.setDomainMode(1);//POS
                        //API: upload/expense/cancel
                        StrResult = HttpsFun.RESTfulAPI_postBody("/api/upload/expense/cancel", Strexpense_cancel, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                    }

                    if (upload_type == "INV_B2C_SALE") //電子發票交易資料 //資料結構 API: POS_Order_2_Invoice_B2C_Order
                    {
                        string Strorders_new = SyncThread.Create_NOD_DOD_Json(data_no);//直接使用訂單上傳VTEAM_CLOUD的JSON資料完整結構，包含訂單主檔，子檔、支付，載具等資訊
                        string StrAPIResult = "";

                        if (POS_INVAPI.POS_Order_2_Invoice_B2C_Order(Strorders_new,ref StrAPIResult))//轉換後的資料是拿來列印或上傳到 cloud的
                        {
                            HttpsFun.setDomainMode(1);//POS
                            StrResult = HttpsFun.RESTfulAPI_postBody("/api/invoice/b2c/upload/INV_B2C_SALE", StrAPIResult, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                        }
                    }

                    if (upload_type == "INV_B2C_CANCEL") //電子發票作廢資料 //資料結構 API: POS_Order_2_Invoice_B2C_Order
                    {
                        string Strorders_new = SyncThread.Create_NOD_DOD_Json(data_no);//直接使用訂單上傳VTEAM_CLOUD的JSON資料完整結構，包含訂單主檔，子檔、支付，載具等資訊
                        string StrAPIResult = "";

                        if (POS_INVAPI.POS_Order_2_Invoice_B2C_Order(Strorders_new, ref StrAPIResult))//轉換後的資料是拿來列印或上傳到 cloud的
                        {
                            HttpsFun.setDomainMode(1);//POS
                            StrResult = HttpsFun.RESTfulAPI_postBody("/api/invoice/b2c/upload/INV_B2C_CANCEL", StrAPIResult, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                        }
                    }

                    if (upload_type == "INV_B2C_REPORT")
                    {
                        String SQL = $"SELECT inv_summery_info FROM daily_report WHERE report_no='{data_no}'";
                        DataTable daily_reportDataTable = new DataTable();
                        daily_reportDataTable = SQLDataTableModel.GetDataTable(SQL);
                        if(daily_reportDataTable!=null && daily_reportDataTable.Rows.Count>0)
                        {
                            string StrAPIResult = "";
                            string POS_Report = daily_reportDataTable.Rows[0][0].ToString();
                            if((JsonClassConvert.DRInvSummeryInfo2Class(POS_Report).details!=null) && (JsonClassConvert.DRInvSummeryInfo2Class(POS_Report).details.Count>0))//有details才需要執行
                            {
                                if (POS_INVAPI.POS_Report_2_Invoice_B2C_Summary(POS_Report, ref StrAPIResult))
                                {
                                    HttpsFun.setDomainMode(1);//POS
                                    StrResult = HttpsFun.RESTfulAPI_postBody("/api/invoice/b2c/upload/INV_B2C_REPORT", StrAPIResult, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
                                }
                            }
                            else
                            {
                                StrResult = @"{""status"": ""OK"",""message"":""""}";
                            }
                        }
                        else
                        {
                            StrResult = @"{""status"": ""OK"",""message"":""""}";
                        }
                    }

                    orders_newResult orders_newResultBuf = JsonClassConvert.ordersnewResult2Class(StrResult);
                    String upload_state = "N";
                    String upload_msg = "";
                    if (orders_newResultBuf != null)
                    {
                        upload_msg = orders_newResultBuf.message;
                        if (orders_newResultBuf.status == "OK")
                        {
                            upload_state = "S";
                        }
                        else
                        {
                            if (orders_newResultBuf.message.Contains("been uploaded"))
                            {
                                upload_state = "S";
                            }
                            else
                            {
                                upload_state = "N";
                                try_count++;
                            }
                        }
                    }

                    if (!m_blnWait)
                    {
                        SyncDBFun.dataUpdate(SID, data_no, upload_state, upload_msg, try_count);
                    }

                    SyncDBFun.m_intUploadRowsCount--;
                }//for (int i = 0; i < SyncDBFun.m_upload_dataDataTable.Rows.Count; i++)

            }//if (SyncDBFun.UploadRowsCount() > 0)
        }

        private static void KeepAlive()//回報設備存活
        {
            SqliteDataAccess.m_terminal_data = SqliteDataAccess.terminal_dataLoad();//設備參數資料
            GTDDescript descript=new GTDDescript();
            descript.user_sid = Convert.ToInt32(MainPage.m_StrUserSID);
            descript.now_class_sid = Convert.ToInt32(SqliteDataAccess.m_terminal_data[0].now_class_sid);
            descript.business_day = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(SqliteDataAccess.m_terminal_data[0].business_day));
            descript.last_order_no = SqliteDataAccess.m_terminal_data[0].last_order_no;
            descript.last_class_report_no = SqliteDataAccess.m_terminal_data[0].last_class_report_no;
            descript.last_daily_report_no = SqliteDataAccess.m_terminal_data[0].last_daily_report_no;
            descript.app_version = MainPage.m_StrVersion;
            descript.last_check_update_time = TimeConvert.DateTimeToUnixTimeStamp(DateTime.Now);
            String StrData = JsonClassConvert.GTDDescript2String(descript);
            HttpsFun.setDomainMode(1);//POS
            //API: terminal/sender/descript
            String StrResult = HttpsFun.RESTfulAPI_postBody("/api/terminal/sender/descript", StrData, "Authorization", "Basic " + SyncDBData.m_StrEncoded);
            Vaut VautBuf = JsonClassConvert.Vaut2Class(StrResult);
            if(VautBuf != null)
            {
                //---
                //程式啟動時，檢查 vatu.script檔案中，是否有等待更新的清單。
                String Strvatu_script = "";
                if (File.Exists(LogFile.m_StrSysPath + "vatu.script"))
                {
                    StreamReader sr = new StreamReader(LogFile.m_StrSysPath + "vatu.script");
                    Strvatu_script = sr.ReadLine();
                    sr.Close();// 關閉串流

                }
                //---程式啟動時，檢查 vatu.script檔案中，是否有等待更新的清單。

                String terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;
                String api_token = SqliteDataAccess.m_terminal_data[0].api_token_id;

                if ((VautBuf.data!=null) && (VautBuf.data.Count>0))
                {//線上更新有值
                    if (Strvatu_script.Trim().Length>4)
                    {//Local端有值
                        if (Strvatu_script.Contains(VautBuf.data[0].deploy_sid))
                        {//VPOS關閉再開啟就會更新
                            /*
                            //Local端值=雲端值 -> 直接更新 & 關閉POS
                            ProcessStartInfo start = new ProcessStartInfo();
                            start.FileName = FileLib.path + "VATU.exe";
                            start.Arguments = $"RUN_UPDATE {terminal_sid} {api_token}";
                            start.UseShellExecute = false;
                            start.RedirectStandardOutput = false;
                            start.UseShellExecute = true;
                            start.WindowStyle = ProcessWindowStyle.Normal;
                            Process p01 = Process.Start(start);

                            Application.Exit();
                            return;
                            */
                        }
                        else
                        {//Local端值!=雲端值 -> 刪除Local端的檔案。
                            File.Delete(LogFile.m_StrSysPath + "vatu.script");
                        }
                    }
                    else
                    {//線上有值但Local沒有值
                        //---
                        //Local端取值
                        ProcessStartInfo start = new ProcessStartInfo();
                        start.FileName = FileLib.path + "VATU.exe";
                        start.Arguments = $"CHECK_UPDATE {terminal_sid} {api_token}";
                        start.UseShellExecute = false;
                        start.RedirectStandardOutput = false;
                        start.UseShellExecute = true;
                        start.WindowStyle = ProcessWindowStyle.Hidden;
                        Process p01 = Process.Start(start);
                        //---Local端取值
                    }
                }
                else//線上更新沒有值
                {//若Local端有該檔案，但雲端派發的資料沒有，則刪除Local端的檔案。
                    File.Delete(LogFile.m_StrSysPath + "vatu.script");
                }
            }
        }

    }//public class SyncThread
}
