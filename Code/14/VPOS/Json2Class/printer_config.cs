using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    [printer_config]
        啟用狀態
        列印範本		  

        連接模式: TCP/IP RS232 Driver
            連線IP;連線PORT
            連線埠;傳輸速率
            印表機名稱 列表

        列印份數
        新單訂單不列印

        錢箱綁定方式:Cash Drawer/指定印表機
            FEC XP-3685
            收據/發票

        {
            "on_off_state": "Y/N",	
            "print_template": "",		

            "conn_type": "TCP/RS232/Driver",
            "conn_data": "xxxx;yyyy / ZZZZZZZZZ",

            "bill_print_count": "1",
            "new_bill_no_print": "N",

            "cash_box_type": "CASH_DRAWER / PRINTER",
            "cash_box_model": "XXXXXX"
        } 
    */
    public class printer_config
    {
        public string on_off_state { get; set; }
        public string print_template { get; set; }
        public string conn_type { get; set; }
        public string conn_data { get; set; }
        public string serial_port { get; set; }
        public int baud_rate { get; set; }
        public string driver_name { get; set; }
        public int tcp_port { get; set; }
        public string tcp_ip_address { get; set; }
        public object bill_print_count { get; set; }
        public string new_bill_no_print { get; set; }
        public string cash_box_type { get; set; }
        public string cash_box_model { get; set; }

        public printer_config()
        {
            on_off_state = "N";
            print_template = "";

            conn_type = "";
            conn_data = "";
            
            bill_print_count = "1";
            new_bill_no_print = "N";

            cash_box_type = "";
            cash_box_model = "";
        }
    }

    public class printer_value
    {
        /*
        SELECT a.SID,a.printer_code,a.printer_name,a.output_type,IFNULL(b.param_value,"") FROM printer_data AS a
        LEFT JOIN printer_config AS b ON a.SID=b.printer_sid
        ORDER BY a.SID
        */
        public int SID;
        public String printer_code;
        public String printer_name;
        public String output_type;//出單類型(B=收據、I=發票、R=報表、W=工作單/標籤機、N=號碼機、S=智能食譜)
        public String template_type;//20230215 新增的欄位
        public String template_sid;//20230215 新增的欄位
        public printer_config printer_config_value;

        public printer_value()
        {
            SID = 0;
            printer_code = "";
            printer_name = "";
            output_type = "";
            template_type = "";
            template_sid = "";
        printer_config_value=new printer_config();
        }
    }
}
