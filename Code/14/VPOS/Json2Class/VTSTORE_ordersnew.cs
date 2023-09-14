using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    {
        "status": "OK",
        "message": "",
        "data": [
            {
                "external_order_id": "20230000325",
                "order_no": "20230000325",
                "call_num": "",
                "order_time": 1678766634,
                "order_mode": "ONLINE",
                "order_type": "PICK_UP",
                "order_state": "CREATED",
                "pre_order": "N",
                "cust_reserve_time": 1678767000,
                "store_completion_time": "",
                "customer": {
                    "first_name": "",
                    "last_name": "",
                    "full_name": "鐘浚豪",
                    "mobile_phone": "0923312231",
                    "email": "",
                    "tax_number": "",
                    "carrier_type": "",
                    "carrier_code": ""
                },
                "delivery_fee": 0,
                "remarks": "",
                "item_count": 4,
                "items": [
                    {
                        "code": "10020012",
                        "name": "復刻卡拉鮮蛋吐司",
                        "type": "PRODUCT",
                        "price": 60,
                        "quantity": 1,
                        "subtotal": 65,
                        "amount": 65,
                        "customer_name": "鐘浚豪"
                    },
                    {
                        "code": "10020012",
                        "name": "復刻卡拉鮮蛋吐司",
                        "type": "PRODUCT",
                        "price": 60,
                        "quantity": 1,
                        "subtotal": 65,
                        "amount": 65,
                        "customer_name": "鐘浚豪"
                    }
                ],
                "package_fee": 1,
                "packages": [
                    {
                        "code": "S001",
                        "name": "小塑膠袋",
                        "price": 1,
                        "quantity": 1,
                        "amount": 1
                    },
                    {
                        "code": "106",
                        "name": "確認服務",
                        "price": 0,
                        "quantity": 1,
                        "amount": 0
                    }
                ],
                "subtotal": 131,
                "promotion_fee": 0,
                "amount": 131,
                "payment_type": "NONE",
                "receiver_id": ""
            },
            {
                "external_order_id": "20230000330",
                "order_no": "20230000330",
                "call_num": "",
                "order_time": 1678851941,
                "order_mode": "ONLINE",
                "order_type": "PICK_UP",
                "order_state": "CREATED",
                "pre_order": "N",
                "cust_reserve_time": 1678852200,
                "store_completion_time": "",
                "customer": {
                    "first_name": "",
                    "last_name": "",
                    "full_name": "567",
                    "mobile_phone": "567",
                    "email": "",
                    "tax_number": "",
                    "carrier_type": "",
                    "carrier_code": ""
                },
                "delivery_fee": 0,
                "remarks": "",
                "item_count": 4,
                "items": [
                    {
                        "code": "10010031",
                        "name": "珍珠太后牛乳",
                        "type": "PRODUCT",
                        "price": 65,
                        "quantity": 1,
                        "subtotal": 65,
                        "amount": 65,
                        "customer_name": "567"
                    },
                    {
                        "code": "10010008",
                        "name": "研磨豆漿",
                        "type": "PRODUCT",
                        "price": 45,
                        "quantity": 1,
                        "subtotal": 45,
                        "amount": 45,
                        "customer_name": "567"
                    }
                ],
                "package_fee": 1,
                "packages": [
                    {
                        "code": "S001",
                        "name": "小塑膠袋",
                        "price": 1,
                        "quantity": 1,
                        "amount": 1
                    },
                    {
                        "code": "106",
                        "name": "確認服務",
                        "price": 0,
                        "quantity": 1,
                        "amount": 0
                    }
                ],
                "subtotal": 111,
                "promotion_fee": 0,
                "amount": 111,
                "payment_type": "NONE",
                "receiver_id": ""
            },
            {
                "external_order_id": "20230000333",
                "order_no": "20230000333",
                "call_num": "",
                "order_time": 1678956884,
                "order_mode": "ONLINE",
                "order_type": "DELIVERY",
                "order_state": "CREATED",
                "pre_order": "N",
                "cust_reserve_time": 1678959000,
                "store_completion_time": "",
                "customer": {
                    "first_name": "",
                    "last_name": "",
                    "full_name": "AAAA",
                    "mobile_phone": "123456789",
                    "email": "",
                    "tax_number": "",
                    "carrier_type": "",
                    "carrier_code": ""
                },
                "delivery_fee": 0,
                "delivery": {
                    "city": "臺中市",
                    "district": "南區",
                    "post_code": "",
                    "address": "臺中市南區復興路三段288號",
                    "remarks": ""
                },
                "remarks": "",
                "item_count": 4,
                "items": [
                    {
                        "code": "T001",
                        "name": "測試套餐",
                        "type": "PRODUCT",
                        "price": 100,
                        "quantity": 1,
                        "subtotal": 100,
                        "amount": 100,
                        "customer_name": "AAAA"
                    },
                    {
                        "code": "10060009",
                        "name": "玉檸香綠",
                        "type": "PRODUCT",
                        "price": 60,
                        "quantity": 1,
                        "subtotal": 70,
                        "amount": 70,
                        "customer_name": "AAAA"
                    }
                ],
                "package_fee": 1,
                "packages": [
                    {
                        "code": "S001",
                        "name": "小塑膠袋",
                        "price": 1,
                        "quantity": 1,
                        "amount": 1
                    },
                    {
                        "code": "106",
                        "name": "確認服務",
                        "price": 0,
                        "quantity": 1,
                        "amount": 0
                    }
                ],
                "subtotal": 171,
                "promotion_fee": 0,
                "amount": 171,
                "payment_type": "NONE",
                "receiver_id": ""
            }
        ]
    }
    */
    public class VTSON_Condiment
    {
        public string code { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class VTSON_Product
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string customer_name { get; set; }
        public List<VTSON_Condiment> condiments { get; set; }
    }

    public class VTSON_SetMeal
    {
        public string att_name { get; set; }
        public List<VTSON_Product> products { get; set; }
    }
    public class VTSON_Customer
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

    public class VTSON_Datum
    {
        public string dborder_no { get; set; }
        public string external_order_id { get; set; }
        public string order_no { get; set; }
        public string call_num { get; set; }
        public double order_time { get; set; }
        public string order_mode { get; set; }
        public string order_type { get; set; }
        public string order_state { get; set; }
        public string pre_order { get; set; }
        public double cust_reserve_time { get; set; }
        public double store_completion_time { get; set; }
        public VTSON_Customer customer { get; set; }
        public VTSON_Table table { get; set; }
        public int service_val { get; set; }
        public int service_fee { get; set; }
        public int delivery_fee { get; set; }
        public string remarks { get; set; }
        public int item_count { get; set; }
        public List<VTSON_Item> items { get; set; }
        public int package_fee { get; set; }
        public List<VTSON_Package> packages { get; set; }

        public List<VTSON_Promotions> promotions { get; set; }
        public int subtotal { get; set; }
        public int promotion_fee { get; set; }
        public int amount { get; set; }
        public string payment_type { get; set; }
        public string receiver_id { get; set; }
        public VTSON_Delivery delivery { get; set; }
    }

    public class VTSON_Delivery
    {
        public string city { get; set; }
        public string district { get; set; }
        public string post_code { get; set; }
        public string address { get; set; }
        public string remarks { get; set; }
    }

    public class VTSON_Item
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string customer_name { get; set; }
        public List<VTSON_Condiment> condiments { get; set; }
        public List<VTSON_SetMeal> set_meals { get; set; }
    }

    public class VTSON_Package
    {
        public string code { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class VTSON_Table
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class VTSON_Promotions
    {
        public string code { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
    }

    public class VTSTORE_ordersnew
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<VTSON_Datum> data { get; set; }
    }
}
