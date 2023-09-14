using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GCPTDatum
    {
        public int SID { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public string payment_module_code { get; set; }
        public string def_paid_flag { get; set; }
        public string def_paid_amount { get; set; }
        public string no_change_flag { get; set; }
        public string del_flag { get; set; }
        public int del_unix_time { get; set; }
        public string stop_flag { get; set; }
        public int stop_unix_time { get; set; }
        public int sort { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_company_payment_type
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCPTDatum> data { get; set; }
    }
}
