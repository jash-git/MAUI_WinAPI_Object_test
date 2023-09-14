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
        "message": "Get New Order Data Success",
        "data": [
            {
                "order_no": "20230000242",
                "company_sid": 2,
                "company_name": "來點餐飲料店",
                "order_time": "2023-03-01 16:40:16",
                "customer_name": "鐘浚豪",
                "customer_phone": "0923312231",
                "order_mode": "O",
                "order_type_name": "",
                "order_type": "T",
                "table_code": "",
                "table_name": "",
                "customer_business_no": "",
                "carrier_code": "",
                "carrier_type": "",
                "customer_reserve_time": "2023-03-01 16:50:00",
                "store_reserve_time": "2023-03-03 16:50:00",
                "delivery_city_name": "",
                "delivery_district_name": "",
                "delivery_address": "",
                "order_note": "",
                "item_count": 3,
                "subtotal": "106.00",
                "promotion_discount": "21.00",
                "coupon_discount": "0.00",
                "service_val": null,
                "service_fee": "0.00",
                "amount": "85.00",
                "state_code": "O",
                "pre_order": "N",
                "paid_type": "A",
                "paid_time": "2023-03-01 16:40:16",
                "transaction_id": "",
                "payment_time": ""
            }
        ]
    }
    */
    public class VTSOIDatum
    {
        public string order_no { get; set; }
        public int company_sid { get; set; }
        public string company_name { get; set; }
        public string order_time { get; set; }
        public string customer_name { get; set; }
        public string customer_phone { get; set; }
        public string order_mode { get; set; }
        public string order_type_name { get; set; }
        public string order_type { get; set; }
        public string table_code { get; set; }
        public string table_name { get; set; }
        public string customer_business_no { get; set; }
        public string carrier_code { get; set; }
        public string carrier_type { get; set; }
        public string customer_reserve_time { get; set; }
        public string store_reserve_time { get; set; }
        public string delivery_city_name { get; set; }
        public string delivery_district_name { get; set; }
        public string delivery_address { get; set; }
        public string order_note { get; set; }
        public int item_count { get; set; }
        public object subtotal { get; set; }
        public string promotion_discount { get; set; }
        public string coupon_discount { get; set; }
        public object service_val { get; set; }
        public string service_fee { get; set; }
        public object amount { get; set; }
        public string state_code { get; set; }
        public string pre_order { get; set; }
        public string paid_type { get; set; }
        public string paid_time { get; set; }
        public string transaction_id { get; set; }
        public string payment_time { get; set; }
    }

    public class VTSTORE_ordersinfo
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<VTSOIDatum> data { get; set; }
    }

}
