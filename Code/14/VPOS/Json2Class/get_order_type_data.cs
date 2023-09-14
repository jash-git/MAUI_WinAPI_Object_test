using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GOTDDatum
    {
        public int price_sid { get; set; }
        public int company_sid { get; set; }
        public int price_type_sid { get; set; }
        public string type_name { get; set; }
        public string order_type_code { get; set; }
        public int payment_def { get; set; }
        public string def_payment_code { get; set; }
        public int invoice_state { get; set; }
        public string display_state { get; set; }
        public int sort { get; set; }
        public string stop_flag { get; set; }
        public string stop_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
        public string @params { get; set; }
}

    public class get_order_type_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GOTDDatum> data { get; set; }
    }
}
