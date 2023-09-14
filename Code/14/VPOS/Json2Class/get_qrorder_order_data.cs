using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GQODData
    {
        public string transaction_id { get; set; }
        public string order_no { get; set; }
        public string generate_time { get; set; }
        public string order_type_name { get; set; }
        public string table_code { get; set; }
        public string table_name { get; set; }
        public int table_split_num { get; set; }
        public int order_count { get; set; }
        public int item_count { get; set; }
        public int subtotal { get; set; }
        public string state_flag { get; set; }
        public string meal_num { get; set; }
        public string guests_num { get; set; }
        public List<GPQDItem> items { get; set; }//public List<object> items { get; set; }
        public GQODData() 
        {
            items=new List<GPQDItem>();
        }
    }

    public class get_qrorder_order_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public GQODData data { get; set; }
        public get_qrorder_order_data()
        {
            data=new GQODData();
        }
    }
}
