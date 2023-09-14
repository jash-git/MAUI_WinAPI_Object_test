using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    新增 組織資料表 def_params 欄位
        "action_state": "ASK", 詢問
        "action_state": "NONE",不列印
        "action_state": "PRINT",直接印
        bill 收據 Y,N
        label 標籤 Y,N
        work_ticket 工作票 Y,N

        WEB 操作: 資料管理 -> 組織管理 -> 點選要編輯組織的編輯按鈕 -> 預設參數

        對應JSON資料:
        {
            "temp_order_param": 
            {
                "action_state": "ASK",
                "print_select": {}
            }
        }

        {
            "temp_order_param":
            {
                "action_state": "NONE",
                "print_select": {}
            }
        }

        {
            "temp_order_param":
            {
                "action_state": "PRINT",
                "print_select":
                {
                    "bill": "Y",
                    "label": "Y",
                    "work_ticket": "Y"
                }
            }
        } 
     */

    public class company_def_params
    {
        public TempOrderParam temp_order_param { get; set; }
    }

    public class PrintSelect
    {
        public string bill { get; set; }
        public string label { get; set; }
        public string work_ticket { get; set; }
    }

    public class TempOrderParam
    {
        public string action_state { get; set; }
        public PrintSelect print_select { get; set; }
    }
}
