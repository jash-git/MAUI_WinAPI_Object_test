using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    新增 訂單類型資料表 params 欄位
        table number 桌號
        meal number 牌號
        guests number 來客數
        service fee 服務費

        calc_val : %
        calc_type : P(原始金額/subtotal) / D(折扣後金額/amount)

        WEB 操作: 銷售管理 -> 訂單類型管理 -> 選擇對應類型名稱的 編輯

        對應JSON資料:
        {
            "table_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "meal_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "guests_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "service_fee_param": {
                "use_flag": "N",
                "calc_type": "",
                "calc_val": 0
            }
        }

        {
            "table_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "meal_num_param": {
                "use_flag": "Y",
                "required": "N"
            },
            "guests_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "service_fee_param": {
                "use_flag": "Y",
                "calc_type": "P",
                "calc_val": "5"
            }
        }

        {
            "table_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "meal_num_param": {
                "use_flag": "N",
                "required": "N"
            },
            "guests_num_param": {
                "use_flag": "Y",
                "required": "N"
            },
            "service_fee_param": {
                "use_flag": "Y",
                "calc_type": "D",
                "calc_val": "10"
            }
        }
    */
    public class ODTP_GuestsNumParam
    {
        public string use_flag { get; set; }
        public string required { get; set; }
    }

    public class ODTP_MealNumParam
    {
        public string use_flag { get; set; }
        public string required { get; set; }
    }

    public class ODTP_ServiceFeeParam
    {
        public string use_flag { get; set; }
        public string calc_type { get; set; }
        public object calc_val { get; set; }
    }

    public class ODTP_TableNumParam
    {
        public string use_flag { get; set; }
        public string required { get; set; }
    }

    public class order_type_data_params
    {
        public ODTP_TableNumParam table_num_param { get; set; }
        public ODTP_MealNumParam meal_num_param { get; set; }
        public ODTP_GuestsNumParam guests_num_param { get; set; }
        public ODTP_ServiceFeeParam service_fee_param { get; set; }
    }
}
