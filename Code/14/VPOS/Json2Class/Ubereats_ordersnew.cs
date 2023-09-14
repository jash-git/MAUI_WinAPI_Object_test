using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class UBEON_Condiment
    {
        public string code { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class UBEON_Product
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string customer_name { get; set; }
        public List<UBEON_Condiment> condiments { get; set; }
    }

    public class UBEON_SetMeal
    {
        public string att_name { get; set; }
        public List<UBEON_Product> products { get; set; }
    }
    public class UBEON_Customer
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public string mobile_phone { get; set; }
        public string email { get; set; }
        public string tax_number { get; set; }
        public string carrier_type { get; set; }
        public string carrier_code { get; set; }
    }

    public class UBEON_Datum
    {
        public string dborder_no { get; set; }
        public string store_id { get; set; }
        public string external_order_id { get; set; }
        public string order_no { get; set; }
        public string call_num { get; set; }
        public double order_time { get; set; }
        public string order_mode { get; set; }
        public string order_type { get; set; }
        public string order_state { get; set; }
        public string pre_order { get; set; }
        public double cust_reserve_time { get; set; }
        public object store_completion_time { get; set; }
        public UBEON_Customer customer { get; set; }
        public string cust_name { get; set; }
        public string cust_phone { get; set; }
        public string cust_tax_number { get; set; }
        public UBEON_Table table { get; set; }
        public int service_val { get; set; }
        public int service_fee { get; set; }
        public int delivery_fee { get; set; }
        public string remarks { get; set; }
        public int item_count { get; set; }
        public List<UBEON_Item> items { get; set; }
        public int package_fee { get; set; }
        public List<UBEON_Package> packages { get; set; }
        public List<UBEON_Promotions> promotions { get; set; }
        public int subtotal { get; set; }
        public int promotion_fee { get; set; }
        public int amount { get; set; }
        public string payment_type { get; set; }
        public string receiver_id { get; set; }
        public UBEON_Delivery delivery { get; set; }
    }

    public class UBEON_Delivery
    {
        public string city { get; set; }
        public string district { get; set; }
        public string post_code { get; set; }
        public string address { get; set; }
        public string remarks { get; set; }
    }

    public class UBEON_Item
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int unit_price { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string customer_name { get; set; }
        public List<UBEON_Condiment> condiments { get; set; }
        public List<UBEON_SetMeal> set_meals { get; set; }
    }

    public class UBEON_Package
    {
        public string code { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class UBEON_Table
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class UBEON_Promotions
    {
        public string code { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
    }

    public class Ubereats_ordersnew
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<UBEON_Datum> data { get; set; }
    }
}
