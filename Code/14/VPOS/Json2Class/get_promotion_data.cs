using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPDAssignOrderType
    {
        public int order_type_sid { get; set; }
        public string order_type_code { get; set; }
    }

    public class GPDDatum1
    {
        public int promotion_sid { get; set; }
        public int company_sid { get; set; }
        public string promotion_name { get; set; }
        public string promotion_start_time { get; set; }
        public int promotion_start_unit_time { get; set; }
        public string promotion_end_time { get; set; }
        public int promotion_end_unit_time { get; set; }
        public int promotion_sort { get; set; }
        public string coexist_flag { get; set; }
        public string promotion_type { get; set; }
        public string promotion_data { get; set; }
        public string stop_flag { get; set; }
        public int stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GPDProductList> product_list { get; set; }
        public List<GPDAssignOrderType> assign_order_type { get; set; }
    }

    public class GPDProductList
    {
        public int promotion_sid { get; set; }
        public int product_sid { get; set; }
    }

    public class get_promotion_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPDDatum1> data { get; set; }
    }
}
