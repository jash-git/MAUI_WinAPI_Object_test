using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GQOLDatum
    {
        public string transaction_id { get; set; }
        public string generate_time { get; set; }
        public int company_sid { get; set; }
        public string order_no { get; set; }
        public string order_type_name { get; set; }
        public string table_code { get; set; }
        public string table_name { get; set; }
        public int item_count { get; set; }
        public int subtotal { get; set; }
        public string meal_num { get; set; }
        public string guests_num { get; set; }
        public int order_count { get; set; }
    }
    public class get_qrorder_order_list
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GQOLDatum> data { get; set; }

        public get_qrorder_order_list()
        {
            data=new List<GQOLDatum>();
        }
    }
}
