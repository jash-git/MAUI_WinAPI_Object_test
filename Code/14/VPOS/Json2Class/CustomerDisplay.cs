using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    {
        "OrderInfo": {
            "order_no": "20230714-0002",
            "status": "訂購中",
            "amount": 35,
            "ordercount": 1,
            "PaidCash": "0",
            "PayStateLabel": "",
            "PayState": "",
            "Cust_EIN": "",
            "Inv_Carrier_Value": "",
            "Table_Name": "",
            "Meal_Num": "",
            "Member_Name": "",
            "Member_Phone": ""
        },
        "ItemInfo": {
            "ITEMSTATE": "N",
            "ProductName": "阿薩姆冰茶",
            "ItemNo": 1,
            "ParentItemNo": 0,
            "ItemType": "P",
            "Count": 1,
            "Cost": 35,
            "SubTotal": 35,
            "Amount": 35,
            "DiscountType": "N",
            "DiscountRate": 0,
            "DiscountFee": 0
        }
    }
    */

    public class CDItemInfo
    {
        public string ITEMSTATE { get; set; }
        public string ProductName { get; set; }
        public int ItemNo { get; set; }
        public int ParentItemNo { get; set; }
        public string ItemType { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
        public int SubTotal { get; set; }
        public int Amount { get; set; }
        public string DiscountType { get; set; }
        public int DiscountRate { get; set; }
        public int DiscountFee { get; set; }
        public string CONDIMENTINFO { get; set; }
    }

    public class CDOrderInfo
    {
        public string order_no { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public int ordercount { get; set; }
        public string PaidCash { get; set; }
        public string PayStateLabel { get; set; }
        public string PayState { get; set; }
        public string Cust_EIN { get; set; }
        public string Inv_Carrier_Value { get; set; }
        public string Table_Name { get; set; }
        public string Meal_Num { get; set; }
        public string Member_Name { get; set; }
        public string Member_Phone { get; set; }
        public string ClearFlag { get; set;}
        public string ORDERFINISH { get; set; }
    }

    public class CustomerDisplay
    {
        public CDOrderInfo OrderInfo { get; set; }
        public CDItemInfo ItemInfo { get; set; }
        public CustomerDisplay()
        {
            OrderInfo = new CDOrderInfo();
            ItemInfo = new CDItemInfo();

        }

    }
}
