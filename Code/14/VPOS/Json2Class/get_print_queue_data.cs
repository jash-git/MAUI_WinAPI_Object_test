using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VPOS
{
    /*
    {
        "status": "OK",
        "message": "Get Print Queue Data Success",
        "data": [
            {
                "queue_sid": "20230725-64bf7fb24bc9e",
                "print_type": "WORK_TICKET",
                "print_data": {
                    "order_no": "2023072500002",
                    "generate_time": "2023-07-25 15:49:31",
                    "order_type_sid": 67,
                    "order_type_name": "內用",
                    "order_count": 1,
                    "item_count": 2,
                    "subtotal": "235.00000",
                    "table_sid": 10,
                    "table_code": null,
                    "table_name": "A04",
                    "table_split_num": 0,
                    "meal_num": null,
                    "guests_num": null,
                    "state_flag": "ordered",
                    "state_memo": null,
                    "update_user_sid": null,
                    "update_user_name": null,
                    "order_list": [
                        {
                            "cart_no": 10,
                            "order_no": "20230725-64bf7f8281a7b",
                            "order_time": "2023-07-25 15:53:38",
                            "item_count": 2,
                            "subtotal": "235.00",
                            "state_flag": "receive",
                            "state_memo": null,
                            "update_user_sid": null,
                            "update_user_name": null,
                            "user_sid": 33,
                            "user_name": "Admin",
                            "items": [
                                {
                                    "item_no": 1,
                                    "category_code": "17",
                                    "category_name": "來找茶",
                                    "type": "P",
                                    "code": "B01-B",
                                    "name": "古早味紅茶 (大)",
                                    "price": "40.00000",
                                    "quantity": 1,
                                    "amount": "65.00000",
                                    "subtotal": "65.00000",
                                    "del_flag": null,
                                    "state_memo": null,
                                    "update_user_sid": null,
                                    "update_user_name": null,
                                    "condiments": [
                                        {
                                            "code": "I0003",
                                            "name": "去冰",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "S0002",
                                            "name": "半糖",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "Q001",
                                            "name": "珍珠",
                                            "price": "10.00000",
                                            "quantity": 1,
                                            "amount": "10.00000",
                                            "subtotal": "10.00000"
                                        },
                                        {
                                            "code": "Q002",
                                            "name": "3Q",
                                            "price": "15.00000",
                                            "quantity": 1,
                                            "amount": "15.00000",
                                            "subtotal": "15.00000"
                                        }
                                    ]
                                },
                                {
                                    "item_no": 6,
                                    "category_code": "set",
                                    "category_name": "套餐",
                                    "type": "T",
                                    "code": "set1111",
                                    "name": "套餐套餐",
                                    "price": "100.00000",
                                    "quantity": 1,
                                    "amount": "170.00000",
                                    "subtotal": "170.00000",
                                    "del_flag": null,
                                    "state_memo": null,
                                    "update_user_sid": null,
                                    "update_user_name": null,
                                    "set_meals": [
                                        {
                                            "att_name": "主餐",
                                            "product": [
                                                {
                                                    "item_no": 8,
                                                    "category_code": null,
                                                    "category_name": null,
                                                    "type": "P",
                                                    "code": "D03",
                                                    "name": "綠豆鮮奶冰沙",
                                                    "price": "60.00000",
                                                    "quantity": 1,
                                                    "amount": "70.00000",
                                                    "subtotal": "70.00000",
                                                    "del_flag": null,
                                                    "state_memo": null,
                                                    "update_user_sid": null,
                                                    "update_user_name": null,
                                                    "condiments": [
                                                        {
                                                            "code": "I0004",
                                                            "name": "多冰",
                                                            "price": "0.00000",
                                                            "quantity": 1,
                                                            "amount": "0.00000",
                                                            "subtotal": "0.00000"
                                                        },
                                                        {
                                                            "code": "S0005",
                                                            "name": "多糖",
                                                            "price": "0.00000",
                                                            "quantity": 1,
                                                            "amount": "0.00000",
                                                            "subtotal": "0.00000"
                                                        },
                                                        {
                                                            "code": "Q001",
                                                            "name": "珍珠",
                                                            "price": "10.00000",
                                                            "quantity": 1,
                                                            "amount": "10.00000",
                                                            "subtotal": "10.00000"
                                                        },
                                                        {
                                                            "code": "Q002",
                                                            "name": "3Q",
                                                            "price": "15.00000",
                                                            "quantity": 1,
                                                            "amount": "15.00000",
                                                            "subtotal": "15.00000"
                                                        },
                                                        {
                                                            "code": "Q005",
                                                            "name": "蘆薈",
                                                            "price": "15.00000",
                                                            "quantity": 1,
                                                            "amount": "15.00000",
                                                            "subtotal": "15.00000"
                                                        },
                                                        {
                                                            "code": "Q006",
                                                            "name": "布丁",
                                                            "price": "0.00000",
                                                            "quantity": 1,
                                                            "amount": "0.00000",
                                                            "subtotal": "0.00000"
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            "att_name": "飲料",
                                            "product": [
                                                {
                                                    "item_no": 16,
                                                    "category_code": null,
                                                    "category_name": null,
                                                    "type": "P",
                                                    "code": "B01L",
                                                    "name": "黃金珍珠奶茶(L)",
                                                    "price": "60.00000",
                                                    "quantity": 1,
                                                    "amount": "0.00000",
                                                    "subtotal": "0.00000",
                                                    "del_flag": null,
                                                    "state_memo": null,
                                                    "update_user_sid": null,
                                                    "update_user_name": null,
                                                    "condiments": [
                                                        {
                                                            "code": "I0003",
                                                            "name": "去冰",
                                                            "price": "0.00000",
                                                            "quantity": 1,
                                                            "amount": "0.00000",
                                                            "subtotal": "0.00000"
                                                        },
                                                        {
                                                            "code": "S0005",
                                                            "name": "多糖",
                                                            "price": "0.00000",
                                                            "quantity": 1,
                                                            "amount": "0.00000",
                                                            "subtotal": "0.00000"
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            },
            {
                "queue_sid": "20230725-64bf82fb242fc",
                "print_type": "QR_CODE",
                "print_data": {
                    "order_no": "2023072500003",
                    "generate_time": "2023-07-25 16:08:27",
                    "order_type_sid": 70,
                    "order_type_name": "FOODPANDA",
                    "order_count": null,
                    "item_count": null,
                    "subtotal": null,
                    "table_sid": null,
                    "table_code": null,
                    "table_name": "",
                    "table_split_num": 0,
                    "meal_num": null,
                    "guests_num": null,
                    "state_flag": "notOrder",
                    "state_memo": null,
                    "update_user_sid": null,
                    "update_user_name": null
                }
            },
            {
                "queue_sid": "20230725-64bf835d8f778",
                "print_type": "WORK_TICKET",
                "print_data": {
                    "order_no": "2023072500003",
                    "generate_time": "2023-07-25 16:08:27",
                    "order_type_sid": 70,
                    "order_type_name": "FOODPANDA",
                    "order_count": 2,
                    "item_count": 3,
                    "subtotal": "235.00000",
                    "table_sid": null,
                    "table_code": null,
                    "table_name": "",
                    "table_split_num": 0,
                    "meal_num": null,
                    "guests_num": null,
                    "state_flag": "ordered",
                    "state_memo": null,
                    "update_user_sid": null,
                    "update_user_name": null,
                    "order_list": [
                        {
                            "cart_no": 11,
                            "order_no": "20230725-64bf832cc8c07",
                            "order_time": "2023-07-25 16:09:16",
                            "item_count": 2,
                            "subtotal": "105.00",
                            "state_flag": "receive",
                            "state_memo": null,
                            "update_user_sid": null,
                            "update_user_name": null,
                            "user_sid": 33,
                            "user_name": "Admin",
                            "items": [
                                {
                                    "item_no": 1,
                                    "category_code": "17",
                                    "category_name": "來找茶",
                                    "type": "P",
                                    "code": "F01",
                                    "name": "阿薩姆冰茶",
                                    "price": "35.00000",
                                    "quantity": 1,
                                    "amount": "45.00000",
                                    "subtotal": "45.00000",
                                    "del_flag": null,
                                    "state_memo": null,
                                    "update_user_sid": null,
                                    "update_user_name": null,
                                    "condiments": [
                                        {
                                            "code": "I0002",
                                            "name": "微冰",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "S0003",
                                            "name": "微糖",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "Q001",
                                            "name": "珍珠",
                                            "price": "10.00000",
                                            "quantity": 1,
                                            "amount": "10.00000",
                                            "subtotal": "10.00000"
                                        }
                                    ]
                                },
                                {
                                    "item_no": 5,
                                    "category_code": "4",
                                    "category_name": "濃濃奶香味",
                                    "type": "P",
                                    "code": "B01L",
                                    "name": "黃金珍珠奶茶(L)",
                                    "price": "60.00000",
                                    "quantity": 1,
                                    "amount": "60.00000",
                                    "subtotal": "60.00000",
                                    "del_flag": null,
                                    "state_memo": null,
                                    "update_user_sid": null,
                                    "update_user_name": null,
                                    "condiments": [
                                        {
                                            "code": "I0002",
                                            "name": "微冰",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "H001",
                                            "name": "溫",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        },
                                        {
                                            "code": "S0004",
                                            "name": "無糖",
                                            "price": "0.00000",
                                            "quantity": 1,
                                            "amount": "0.00000",
                                            "subtotal": "0.00000"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            },
            {
                "queue_sid": "20230725-64bf8361b71e6",
                "print_type": "WORK_TICKET",
                "print_data": {
                    "order_no": "2023072500003",
                    "generate_time": "2023-07-25 16:08:27",
                    "order_type_sid": 70,
                    "order_type_name": "FOODPANDA",
                    "order_count": 2,
                    "item_count": 3,
                    "subtotal": "235.00000",
                    "table_sid": null,
                    "table_code": null,
                    "table_name": "",
                    "table_split_num": 0,
                    "meal_num": null,
                    "guests_num": null,
                    "state_flag": "ordered",
                    "state_memo": null,
                    "update_user_sid": null,
                    "update_user_name": null,
                    "order_list": [
                        {
                            "cart_no": 12,
                            "order_no": "20230725-64bf8352ca722",
                            "order_time": "2023-07-25 16:09:54",
                            "item_count": 1,
                            "subtotal": "130.00",
                            "state_flag": "receive",
                            "state_memo": null,
                            "update_user_sid": null,
                            "update_user_name": null,
                            "user_sid": 33,
                            "user_name": "Admin",
                            "items": [
                                {
                                    "item_no": 1,
                                    "category_code": "set",
                                    "category_name": "套餐",
                                    "type": "T",
                                    "code": "下午茶優惠",
                                    "name": "下午茶優惠",
                                    "price": "100.00000",
                                    "quantity": 1,
                                    "amount": "130.00000",
                                    "subtotal": "130.00000",
                                    "del_flag": null,
                                    "state_memo": null,
                                    "update_user_sid": null,
                                    "update_user_name": null,
                                    "set_meals": [
                                        {
                                            "att_name": "點心",
                                            "product": [
                                                {
                                                    "item_no": 3,
                                                    "category_code": null,
                                                    "category_name": null,
                                                    "type": "P",
                                                    "code": "奶酥厚片",
                                                    "name": "奶酥厚片",
                                                    "price": "50.00000",
                                                    "quantity": 1,
                                                    "amount": "0.00000",
                                                    "subtotal": "0.00000",
                                                    "del_flag": null,
                                                    "state_memo": null,
                                                    "update_user_sid": null,
                                                    "update_user_name": null
                                                }
                                            ]
                                        },
                                        {
                                            "att_name": "飲品",
                                            "product": [
                                                {
                                                    "item_no": 5,
                                                    "category_code": null,
                                                    "category_name": null,
                                                    "type": "P",
                                                    "code": "蕭邦",
                                                    "name": "蕭邦",
                                                    "price": "100.00000",
                                                    "quantity": 1,
                                                    "amount": "30.00000",
                                                    "subtotal": "30.00000",
                                                    "del_flag": null,
                                                    "state_memo": null,
                                                    "update_user_sid": null,
                                                    "update_user_name": null
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }
        ]
    }
    */

    public class get_print_queue_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPQDDatum> data { get; set; }
    }

    public class GPQDDatum
    {
        public string queue_sid { get; set; }
        public string print_type { get; set; }
        public GPQDPrintData print_data { get; set; }
    }

    public class GPQDPrintData
    {
        public string transaction_id { get; set; }
        public string order_no { get; set; }
        public string generate_time { get; set; }
        public int order_type_sid { get; set; }
        public string order_type_name { get; set; }
        public object order_count { get; set; }
        public object item_count { get; set; }
        public object subtotal { get; set; }
        public object table_sid { get; set; }
        public string table_code { get; set; }
        public string table_name { get; set; }
        public object table_split_num { get; set; }
        public object meal_num { get; set; }
        public object guests_num { get; set; }
        public string state_flag { get; set; }
        public object state_memo { get; set; }
        public string order_url { get; set; }
        public object update_user_sid { get; set; }
        public object update_user_name { get; set; }
        public List<GPQDOrderList> order_list { get; set; }
    }

    public class GPQDOrderList
    {
        public int cart_no { get; set; }
        public string order_no { get; set; }
        public string order_time { get; set; }
        public object item_count { get; set; }
        public object subtotal { get; set; }
        public string state_flag { get; set; }
        public object state_memo { get; set; }
        public object update_user_sid { get; set; }
        public object update_user_name { get; set; }
        public object user_sid { get; set; }
        public string user_name { get; set; }
        public List<GPQDItem> items { get; set; }
    }

    public class GPQDItem
    {
        public object item_no { get; set; }
        public string category_code { get; set; }
        public string category_name { get; set; }
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
        public List<GPQDCondiment> condiments { get; set; }
        public List<GPQDSetMeal> set_meals { get; set; }
    }

    public class GPQDCondiment
    {
        public string code { get; set; }
        public string name { get; set; }
        public object price { get; set; }
        public object quantity { get; set; }
        public object amount { get; set; }
        public object subtotal { get; set; }
    }

    public class GPQDSetMeal
    {
        public string att_name { get; set; }
        public List<GPQDProduct> product { get; set; }
    }

    public class GPQDProduct
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
        public List<GPQDCondiment> condiments { get; set; }
    }


}
