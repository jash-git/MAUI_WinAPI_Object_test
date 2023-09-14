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
        "message": "Get Qr Order Params Success",
        "data": {
            "qr_order_active_flag": "Y",
            "dup_order_flag": "Y",
            "terminal_flag": "N",
            "terminal_sid": "VT-POS-2020-00002"
        }
    }    
    */
    public class GQPData
    {
        public string qr_order_active_flag { get; set; }//啟用掃碼點單旗標（若設定為不啟用，則只會帶回此參數）
        public string dup_order_flag { get; set; }//允許重複開單旗標
        public string terminal_flag { get; set; }//使用POS結帳旗標
        public string terminal_sid { get; set; }//列印終端SID（設定單據列印方式為指定POS主機才會帶回，否則帶回空字串）
    }
    public class get_qrorder_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public GQPData data { get; set; }
    }
}
