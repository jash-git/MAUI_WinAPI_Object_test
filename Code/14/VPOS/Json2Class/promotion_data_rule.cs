using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
	{
		"trigger_type": "num", //觸發類型 : “empty” 無條件觸發;	“amount” 訂單金額;“num” 商品數量
		"trigger_value": "6", //金額(數量)條件	[float]
		"trigger_repeat": "Y",   //[“Y”, “N”]	條件滿足是否可重複折扣
		
		"trigger_amount_result"	: "Y/N" //觸發金額依據	“Y”	折扣後金額 “N”	訂單總額		
		"trigger_num_target": "total", //觸發數量依據	“total”	總數量;	“single”	同一商品數量
			
		"discount_target": "custom", //折扣對象	“order”	總額;	“product”	訂單所有商品;	“custom”	指定對象
		"discount_type": "price", //折扣類型	“rate”	折扣(%);	“fee”	折價(元);	“price”	固定價(元)
		"discount_value": "0", //折扣數	[float]
		"discount_limit": "0" //	折扣上限金額	[float]
		"discount_custom_num": "1",//折扣對象數量
		"discount_custom_target": "avg"//折扣對象依據	“min”	取最低價位;	“max”	取最高價位;	“avg”	取平均 (最接近低於平均者)
		
		"include_condiment": "N",  //[“Y”, “N”]	是否包含配料金額計算		
	}
    */
    public class promotion_data_rule
    {
        public string trigger_type { get; set; }
        public string trigger_value { get; set; }
        public string trigger_repeat { get; set; }

        public string trigger_amount_result { get; set; }
        public string trigger_num_target { get; set; }


        public string discount_target { get; set; }
        public string discount_type { get; set; }
        public string discount_value { get; set; }
        public string discount_limit { get; set; }
        public string discount_custom_num { get; set; }
        public string discount_custom_target { get; set; }

        public string include_condiment { get; set; }
    }

    public class promotion_calc_data01 //產品 數量統計 價格 
    {
        /*
        SELECT item_sid,SUM(item_count),item_cost
        FROM order_content_data 
        WHERE (order_no='20221124-0001') AND (item_type='P' OR item_type='T') AND (parent_item_no=0)
        GROUP BY item_sid
        */
        public int m_intProductSID;//SID;"item_sid"
        public int m_intProductCount;//數量;"item_count"
        public double m_dblProductPrice;//本身價格;"item_cost"
        //public double m_dblCondimentPrice;//配料價格;"condiment_price"
        //public double m_dblProductSubtotal;//(item_cost+condiment_price)*item_count;"item_subtotal"
        //public double m_dblProductAmount;//(item_subtotal-discount_fee);"item_amount"
        public int m_intState;//[-1:不可用;0:未使用;X:剩餘量]
    }

    public class promotion_join_data
    {
        /*
        SELECT a.SID,a.promotion_name,a.coexist_flag,a.promotion_type,a.promotion_data,a.promotion_start_time,a.promotion_end_time FROM promotion_data AS a WHERE a.stop_flag='N' AND a.del_flag='N' ORDER BY a.promotion_sort
        */
        public int m_intSID;
        public String m_StrName;
        public String m_Strcoexist_flag;
        public String m_Strtype;
        public promotion_data_rule m_promotion_data_rule = new promotion_data_rule();//促銷規則資料結構
        public DateTime m_DTstart_time;
        public DateTime m_DTend_time;   
        public List<int> m_ListProductID=new List<int>();
        public List<int> m_ListOrderTypeID=new List<int>();
    }

    public class discount_custom_target
    {
        public int sid;//SID
        public int count;//數量
        public double value;
        public double cost;//價格
        public double total;//item_cost+condiment_price
    }

    //---
    //
    public class ODPVArray
    {
        public int promotion_sid { get; set; }
        public string promotion_name { get; set; }
        public string promotion_info { get; set; }
        public int promotion_count { get; set; }
        public int promotion_fee { get; set; }
    }
    public class ODPromotionValue//order_data內的promotion_value欄位
    {
        /*
        [
	        {
		        "promotion_sid": 13,
		        "promotion_name": "外送買6送1",
		        "promotion_info": "共計折扣 200 元 (4杯)",
		        "promotion_count": 4,
		        "promotion_fee": 200
	        },
	        {
		        "promotion_sid": 13,
		        "promotion_name": "外送買6送1",
		        "promotion_info": "共計折扣 200 元 (4杯)",
		        "promotion_count": 4,
		        "promotion_fee": 200
	        }
        ] 
         */
        /*
        ODPromotionValue ODPromotionValueBuf = JsonClassConvert.ODPromotionValue2Class(@"[{""promotion_sid"":13,""promotion_name"":""\u5916\u9001\u8CB76\u90011"",""promotion_info"":""\u5171\u8A08\u6298\u6263 200 \u5143 (4\u676F)"",""promotion_count"":4,""promotion_fee"":200},{""promotion_sid"":13,""promotion_name"":""\u5916\u9001\u8CB76\u90011"",""promotion_info"":""\u5171\u8A08\u6298\u6263 200 \u5143 (4\u676F)"",""promotion_count"":4,""promotion_fee"":200}]");
        String StrBuf = JsonClassConvert.ODPromotionValue2String(ODPromotionValueBuf);
        */
        public List<ODPVArray> Array { get; set; }
        public ODPromotionValue()
        {
            Array = new List<ODPVArray>();
        }
    }
    //---
}
