using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
        {
            "status": "INVALID_AUTHORIZATION",
            "message": "Invalid POS [Api_Token] Value."
        } 

        {
            "status": "ACCEPTED",
            "message": "",
            "data": {
                "company_sid": 7,
                "terminal_sid": "VT-POS-2019-0000005",
                "terminal_name": "開發測試POS",
                "app_version": "1.5.0.3",
                "license_type": "POS",
                "invoice_active_state": "N",
                "pos_no": "",
                "pid": "",
                "rid": "870f2d7a-f795-47aa-357a-9bec9fdf702a",
                "reg_state": "W",
                "reg_submit_time": "2022-06-14 13:58:27",
                "reg_submit_unix_time": 1655186307,
                "reg_accept_time": null,
                "reg_accept_unix_time": null,
                "api_token": "0252cdc0-eba7-11ec-b78c-018c3f1b4592",
                "machine_code": null,
                "keyhook_enable": "N",
                "stop_flag": "N",
                "del_flag": "N",
                "last_order_no": "20220218-0003",
                "last_class_report_no": "",
                "last_daily_report_no": "",
                "client_id": "",
                "client_secret": "",
                "descript": {
                    "user_sid": 1,
                    "now_class_sid": 1,
                    "business_day": 1645156857,
                    "last_order_no": "20220218-0003",
                    "last_class_report_no": "",
                    "last_daily_report_no": "",
                    "app_version": "1.5.0.3",
                    "last_check_update_time": 1645159042
                },
                "company_active": "Y"
            }
        }
    */
    public class GTDData
    {
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
        public string terminal_name { get; set; }
        public string app_version { get; set; }
        public string license_type { get; set; }
        public string invoice_active_state { get; set; }
        public string pos_no { get; set; }
        public string pid { get; set; }
        public string rid { get; set; }
        public string reg_state { get; set; }
        public string reg_submit_time { get; set; }
        public int reg_submit_unix_time { get; set; }
        public string reg_accept_time { get; set; }
        public int reg_accept_unix_time { get; set; }
        public string api_token { get; set; }
        public object machine_code { get; set; }
        public string keyhook_enable { get; set; }
        public string stop_flag { get; set; }
        public string del_flag { get; set; }
        public string last_order_no { get; set; }
        public string last_class_report_no { get; set; }
        public string last_daily_report_no { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public GTDDescript descript { get; set; }
        public string company_active { get; set; }
        public string @params { get; set; }//新增 終端設備資料表 params at 20221020
    }

    public class GTDDescript
    {
        public int user_sid { get; set; }
        public int now_class_sid { get; set; }
        public long business_day { get; set; }
        public string last_order_no { get; set; }
        public string last_class_report_no { get; set; }
        public string last_daily_report_no { get; set; }
        public string app_version { get; set; }
        public long last_check_update_time { get; set; }
    }

    public class get_terminal_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public GTDData data { get; set; }
    }
}
