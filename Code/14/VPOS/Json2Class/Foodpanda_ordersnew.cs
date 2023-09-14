using System;
using System.Collections.Generic;
using System.Linq;
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
                "store_id": "0C1ADCB0036011ED852573C2DCD3483C",
                "external_order_id": "2c86a660-d359-11ed-8041-b384d94734df",
                "order_no": "w41v-cuip",
                "call_num": "180",
                "order_time": 1680661503,
                "order_mode": "O",
                "order_state": "ACCEPTED",
                "order_type": "DELIVERY",
                "delivery": {
                    "expected_delivery_time": 1680662939,
                    "rider_pickup_time": 1680662078,
                    "delivery_fee": 0,
                    "delivery_fees": []
                },
                "customer": {
                    "first_name": "**********",
                    "last_name": "**********",
                    "phone": "**********",
                    "email": ""
                },
                "remarks": "+ ** 請提供免洗餐具、吸管。",
                "items": [
                    {
                        "type": "P",
                        "code": "10040010",
                        "name": "芯月烏奶",
                        "unit_price": 55,
                        "price": 70,
                        "quantity": 1,
                        "condiments": [
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20W00007",
                                "name": "常溫",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            },
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20S00000",
                                "name": "無糖【健康】",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            },
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20X00001",
                                "name": "白玉珍珠",
                                "quantity": 1,
                                "price": 15,
                                "subtotal": 15,
                                "amount": 15
                            }
                        ],
                        "subtotal": 70,
                        "amount": 70,
                        "remark": ""
                    },
                    {
                        "type": "P",
                        "code": "10020010",
                        "name": "四季春茶",
                        "unit_price": 40,
                        "price": 40,
                        "quantity": 1,
                        "condiments": [
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20W00006",
                                "name": "去冰【小碎冰】",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            },
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20S00000",
                                "name": "無糖【健康】",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            }
                        ],
                        "subtotal": 40,
                        "amount": 40,
                        "remark": ""
                    },
                    {
                        "type": "P",
                        "code": "10060017",
                        "name": "白玉紅茶歐蕾",
                        "unit_price": 90,
                        "price": 90,
                        "quantity": 1,
                        "condiments": [
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20W00008",
                                "name": "溫【50度】",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            },
                            {
                                "group_code": "",
                                "group_name": "",
                                "code": "20S00003",
                                "name": "微糖【3分】",
                                "quantity": 1,
                                "price": 0,
                                "subtotal": 0,
                                "amount": 0
                            }
                        ],
                        "subtotal": 90,
                        "amount": 90,
                        "remark": ""
                    }
                ],
                "item_count": 3,
                "package_fee": 1,
                "packages": [
                    {
                        "code": "ContainerCharge",
                        "name": "塑膠袋",
                        "price": 1,
                        "quantity": 1,
                        "amount": 1,
                        "subtotal": 1
                    }
                ],
                "subtotal": 201,
                "payment_type": "FOODPANDA",
                "promotion_fee": 0,
                "promotions": [],
                "platform_proms": [],
                "amount": 201
            }
        ]
    }
    */
    public class FPAON_Condiment
    {
        public string group_code { get; set; }
        public string group_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
    }

    public class FPAON_Product
    {
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string customer_name { get; set; }
        public List<FPAON_Condiment> condiments { get; set; }
    }

    public class FPAON_SetMeal
    {
        public string att_name { get; set; }
        public List<FPAON_Product> products { get; set; }
    }

    public class FPAON_Customer
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class FPAON_Datum
    {
        public string store_id { get; set; }
        public string external_order_id { get; set; }
        public string order_no { get; set; }
        public string call_num { get; set; }
        public int order_time { get; set; }
        public string order_mode { get; set; }
        public string order_state { get; set; }
        public string order_type { get; set; }
        public FPAON_Delivery delivery { get; set; }
        public FPAON_Customer customer { get; set; }
        public string remarks { get; set; }
        public List<FPAON_Item> items { get; set; }
        public int item_count { get; set; }
        public int package_fee { get; set; }
        public List<FPAON_Package> packages { get; set; }
        public int subtotal { get; set; }
        public string payment_type { get; set; }
        public int promotion_fee { get; set; }
        public List<FPAON_Promotions> promotions { get; set; }
        public List<object> platform_proms { get; set; }
        public int amount { get; set; }
    }

    public class FPAON_Delivery
    {
        public int expected_delivery_time { get; set; }
        public int rider_pickup_time { get; set; }
        public int delivery_fee { get; set; }
        public List<object> delivery_fees { get; set; }
    }

    public class FPAON_Promotions
    {
        public string code { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
    }

    public class FPAON_Item
    {
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int unit_price { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public List<FPAON_Condiment> condiments { get; set; }
        public List<FPAON_SetMeal> set_meals { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
        public string remark { get; set; }
    }

    public class FPAON_Package
    {
        public string code { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
        public int subtotal { get; set; }
    }

    public class Foodpanda_ordersnew
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<FPAON_Datum> data { get; set; }
    }
}
